using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;
using static WebToeic.WebAppMVC.ViewModels.UserAnswersVM;

namespace WebToeic.WebAppMVC.Controllers
{
    public class UserListenController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;
        private readonly UserManager<User> _userManager;

        public UserListenController(WebToeicDbContext dbcontext, UserManager<User> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/WebToeicF/Listens/{page?}")]
        public async Task<IActionResult> Index(string nameListen = "", string levelListen = "", string partListen = "", int currentPage = 1)
        {

            // Search
            nameListen = string.IsNullOrEmpty(nameListen) ? "" : nameListen.ToLower();
            var listenData = new ListenVM();

            var listens = await _dbcontext.Listenings
                        .Where(emp => (string.IsNullOrEmpty(nameListen) || emp.ListeningName.ToLower().StartsWith(nameListen))
                                    && (string.IsNullOrEmpty(levelListen) || emp.Level == int.Parse(levelListen))
                                    && (string.IsNullOrEmpty(partListen) || emp.Part == int.Parse(partListen)))
                        .ToListAsync();
           
            // Pagination
            int totalRecords = listens.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            listens = listens.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            listenData.Listenings = listens;
            listenData.CurrentPage = currentPage;
            listenData.TotalPages = totalPages;
            listenData.PageSize = pageSize;
            listenData.NameListen = nameListen;
            
            return View(listenData);
        }

        [HttpGet]
        [Route("/WebToeicF/Listens/DoListen/{id}")]
        public async Task<IActionResult> DoListen(int id)
        {
            // Lay danh sach cau hoi
            var questions = await _dbcontext.ListeningQuestions.Where(lq => lq.ListeningId == id).ToListAsync();

            // Logic Comment 
            var comments = await _dbcontext.CommentListens
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.Replies)
            .Where(c => c.ListeningId == id)
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

            var listen = await _dbcontext.Listenings
                        .FirstOrDefaultAsync(g => g.Id == id);
            

            var qVM = new GetDoListenVM
            {    // phan 1   
                idListen = id,
                listenQuestions = questions,
                userAnswers = new List<UserAnswersVM> { },

                // phan data cho cmt
                Listen = listen,       
                CommentListens = rootComments,
                
            };
            return View(qVM);
        }


        [HttpPost]
        [Route("/WebToeicF/Listens/DoListen/{id}")]
        public async Task<IActionResult> DoListenPost(int id, [FromForm] Dictionary<int,string> userAnswers)
        {            
            var correctAnswers = _dbcontext.ListeningQuestions
                                .Where(tq => tq.ListeningId == id)
                                .Select(tq => new { tq.Id, tq.CorrectAnswer })
                                .ToDictionary(tq => tq.Id, tq => tq.CorrectAnswer);

            int correctCount = 0;
            List<GetDoListenVM> result = new List<GetDoListenVM>();

            foreach (var userAnswer in userAnswers)
            {
                var questionId = userAnswer.Key;
                var selectedAnswer = userAnswer.Value;

                /*var correctAnswerId = _dbcontext.ListeningQuestions
                                    .Where(tq => tq.Id == questionId)
                                    .Select(tq => tq.Id)
                                    .FirstOrDefault();*/
                var correctAns = _dbcontext.ListeningQuestions
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
                result.Add(new GetDoListenVM 
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
        [Route("/WebToeicF/Listens/CmtDoListen/{id}")]
        public async Task<IActionResult> CreateCommentListen(int id, GetDoListenVM model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại từ HttpContext
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userName = user.UserName;
                // Lấy Id của người dùng
                var userId = user.Id;
                // Lay ten bai listen
                var listen = await _dbcontext.Listenings.FirstOrDefaultAsync(x => x.Id == id);
                var listenName = listen.ListeningName;

                var newCmtListen = new CommentListening
                {
                    ParentCommentId = model.ParentCommentIdL,
                    UserId = userId,
                    ListeningId = id,
                    Content = model.ContentL,
                    Time = DateTime.Now,
                    UserNameCmtL = userName,
                    ListenNameCmtL = listenName

                };

                await _dbcontext.CommentListens.AddAsync(newCmtListen);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("DoListen", new { id = id });
            }

            // TODO: Throw some error
            return RedirectToAction("DoListen", new { id = id });
        }


    }
}
