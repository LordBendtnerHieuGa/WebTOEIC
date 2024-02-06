using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class CommentAndVocabularyVM
    {
        public Vocabulary? Vocabulary { get; set; }
        
        public List<CommentVocabulary>? CommentVocabularies { get; set; }

        public List<VocabularyContent>? VocaContents { get; set; }

        public string ContentV { get; set; }
        public int? ParentCommentIdV { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int? VocaId { get; set; }
    }
}
