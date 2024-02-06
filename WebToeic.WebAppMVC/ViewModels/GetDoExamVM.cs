using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class GetDoExamVM
    {
        public int? QuestionId { get; set; } // Neu loi can check lai      
        public string? CorrectAnswer { get; set; }
        public string? SelectedAnswer { get; set; } // Neu loi can check lai

        public int idExam { get; set; }
        public List<TestQuestion> testQuestions { get; set; }
        public List<UserAnswersVM>? userAnswers { get; set; }

        public GetDoExamVM()
        {
            testQuestions = new List<TestQuestion>();
        }
    }
}
