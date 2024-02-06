using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    public class UserReadController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;
        private readonly UserManager<User> _userManager;

        public UserReadController(WebToeicDbContext dbcontext, UserManager<User> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/WebToeicF/Reads/{page?}")]
        public async Task<IActionResult> Index(string nameRead = "", string levelRead = "", string orderBy = "", string partRead = "", int currentPage = 1)
        {

            // Search
            nameRead = string.IsNullOrEmpty(nameRead) ? "" : nameRead.ToLower();
            var readData = new ReadVM();

            var reads = await _dbcontext.Readings
                        .Where(emp => (string.IsNullOrEmpty(nameRead) || emp.ReadingsName.ToLower().StartsWith(nameRead))
                                    && (string.IsNullOrEmpty(levelRead) || emp.Level == int.Parse(levelRead))
                                    && (string.IsNullOrEmpty(partRead) || emp.Part == int.Parse(partRead)))
                        .ToListAsync();

            // Pagination
            int totalRecords = reads.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            reads = reads.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            readData.Readings = reads;
            readData.CurrentPage = currentPage;
            readData.TotalPages = totalPages;
            readData.PageSize = pageSize;
            readData.NameRead = nameRead;
          
            return View(readData);
        }

        [HttpGet]
        [Route("/WebToeicF/Reads/DoRead/{id}")]
        public async Task<IActionResult> DoRead(int id)
        {
            // Lay danh sach cau hoi
            var questions = await _dbcontext.ReadingQuestions.Where(rq => rq.ReadingId == id).ToListAsync();

            // Logic Comment 
            var comments = await _dbcontext.CommentReadings
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.Replies)
            .Where(c => c.ReadingId == id)
            .ToListAsync();

            // Structure comments into a tree
            var rootComments = comments
                .Where(c => c.ParentCommentId == null)
                .AsParallel()
                .ToList();

            // Lấy Id của người dùng
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //var userId = user.Id;
            //var userName = user.UserName;

            var read = await _dbcontext.Readings
                        .FirstOrDefaultAsync(g => g.Id == id);


            var qVM = new GetDoReadVM
            {    // phan 1   
                idRead = id,
                readQuestions = questions,
                userAnswers = new List<UserAnswersVM> { },

                // phan data cho cmt
                Reading = read,
                CommentReads = rootComments,

            };
            return View(qVM);
      
        }

        [HttpPost]
        [Route("/WebToeicF/Reads/DoRead/{id}")]   
        public async Task<IActionResult> DoReadPost(int id, [FromForm] Dictionary<int, string> userAnswers)
        {
            var correctAnswers = _dbcontext.ReadingQuestions
                                .Where(tq => tq.ReadingId == id)
                                .Select(tq => new { tq.Id, tq.CorrectAnswer })
                                .ToDictionary(tq => tq.Id, tq => tq.CorrectAnswer);

            int correctCount = 0;
            List<GetDoReadVM> result = new List<GetDoReadVM>();

            foreach (var userAnswer in userAnswers)
            {
                var questionId = userAnswer.Key;
                var selectedAnswer = userAnswer.Value;
          
                var correctAns = _dbcontext.ReadingQuestions
                                    .Where(tq => tq.Id == questionId)
                                    .Select(tq => tq.CorrectAnswer)
                                    .FirstOrDefault();

                if (correctAnswers.ContainsKey(questionId))
                {
                    var correctAnswer = correctAnswers[questionId];

                    // So sánh đáp án người dùng với đáp án đúng
                    if (selectedAnswer == correctAnswer)
                    {
                        correctCount++;
                    }

                }
                result.Add(new GetDoReadVM
                {
                    QuestionId = questionId,
                    CorrectAnswer = correctAns,
                    SelectedAnswer = selectedAnswer
                });

            }


            return Json(new { correctCount, answers = result });
        }

        [Authorize]
        [HttpPost]
        [Route("/WebToeicF/Reads/CmtDoRead/{id}")]
        public async Task<IActionResult> CreateCommentRead(int id, GetDoReadVM model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại từ HttpContext
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userName = user.UserName;
                // Lấy Id của người dùng
                var userId = user.Id;
                // Lay ten bai doc
                var read = await _dbcontext.Readings.FirstOrDefaultAsync(x => x.Id == id);
                var readName = read.ReadingsName;

                var newCmtRead = new CommentReading
                {
                    ParentCommentId = model.ParentCommentIdR,
                    UserId = userId,
                    ReadingId = id,
                    Content = model.ContentR,
                    Time = DateTime.Now,
                    UserNameCmtR = userName,
                    ReadingNameCmtR = readName,
                };

                await _dbcontext.CommentReadings.AddAsync(newCmtRead);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("DoRead", new { id = id });
            }

            // TODO: Throw some error
            return RedirectToAction("DoRead", new { id = id });
        }


    }
}
