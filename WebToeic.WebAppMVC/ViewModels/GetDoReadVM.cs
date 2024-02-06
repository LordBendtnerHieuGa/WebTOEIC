using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class GetDoReadVM
    {
        public int? QuestionId { get; set; } // Neu loi can check lai      
        public string? CorrectAnswer { get; set; }
        public string? SelectedAnswer { get; set; } // Neu loi can check lai

        public int? idRead { get; set; }
        public List<ReadingQuestion>? readQuestions { get; set; } // Cai nay dung chung cho Cmt va Check answer
        public List<UserAnswersVM>? userAnswers { get; set; }

        public GetDoReadVM()
        {
            readQuestions = new List<ReadingQuestion>();
        }


        public Reading? Reading { get; set; }

        public List<CommentReading>? CommentReads { get; set; }
        public string ContentR { get; set; }
        public int? ParentCommentIdR { get; set; }
        public string? UserId { get; set; }
    }
}
