using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize]
    public class UserExamController : Controller
    {
        private readonly ExamService _examService;
        private readonly WebToeicDbContext _dbcontext;


        public UserExamController(ExamService examService, WebToeicDbContext dbcontext)
        {
            _examService = examService;
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("/WebToeicF/Exams/{page?}")]
        public async Task<IActionResult> Index(string nameExam = "", int currentPage = 1)
        {
            nameExam = string.IsNullOrEmpty(nameExam) ? "" : nameExam.ToLower();
            var examData = new ExamVM();
            

            var tests = await _dbcontext.Tests
                                .Where(emp => nameExam == "" || emp.TestName.ToLower().StartsWith(nameExam))
                                .ToListAsync();

           
            int totalRecords = tests.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            tests = tests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            examData.Tests = tests;
            examData.CurrentPage = currentPage;
            examData.TotalPages = totalPages;
            examData.PageSize = pageSize;
            examData.NameExam = nameExam;
           
            return View(examData);

        }


        [HttpGet]
        [Route("/WebToeicF/Exams/DoExam/{id}")]
        public async Task<IActionResult> DoExam([FromRoute] int id)
        {

            var questions = await _dbcontext.TestQuestions.Where(tq => tq.TestId == id).ToListAsync();
            var quesVM = new GetDoExamVM
            {
                idExam = id,
                testQuestions = questions,
                userAnswers = new List<UserAnswersVM> { },
            };

            return View(quesVM);
        }

        [HttpPost]
        [Route("/WebToeicF/Exams/DoExam/{id}")]
        public async Task<IActionResult> DoExamPost(int id, [FromForm] Dictionary<int, string> userAnswers)
        {
            
            var correctAnswers = _dbcontext.TestQuestions
                                  .Where(tq => tq.TestId == id)
                                  .Select(tq => new { tq.Id, tq.CorrectAnswer })
                                  .ToDictionary(tq => tq.Id, tq => tq.CorrectAnswer);

            int correctCount = 0;
            List<GetDoExamVM> result = new List<GetDoExamVM>();

            foreach (var userAnswer in userAnswers)
            {
                var questionId = userAnswer.Key;
                var selectedAnswer = userAnswer.Value;

                var correctAns = _dbcontext.TestQuestions
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
                result.Add(new GetDoExamVM
                {
                    QuestionId = questionId,
                    CorrectAnswer = correctAns,
                    SelectedAnswer = selectedAnswer
                });

            }


            return Json(new { correctCount, answers = result });
            

        }

    }
}

