using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class GetDoListenVM
    {
        public int? QuestionId { get; set; } // Neu loi can check lai      
        public string? CorrectAnswer { get; set; }
        public string? SelectedAnswer { get; set; } // Neu loi can check lai

        public int? idListen { get; set; }
        public List<ListeningQuestion>? listenQuestions { get; set; } // Cai nay dung chung cho Cmt va Check answer
        public List<UserAnswersVM>? userAnswers { get; set; }

        public GetDoListenVM()
        {
            listenQuestions = new List<ListeningQuestion>();
        }


        public Listening? Listen { get; set; }

        public List<CommentListening>? CommentListens { get; set; }
        public string ContentL { get; set; }
        public int? ParentCommentIdL { get; set; }
        public string? UserId { get; set; }       
        //public int? VocaId { get; set; }
    }
}
