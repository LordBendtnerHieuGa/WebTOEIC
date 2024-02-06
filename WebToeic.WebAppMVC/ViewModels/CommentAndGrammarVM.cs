using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class CommentAndGrammarVM
    {
        public Grammar? Grammar { get; set; }
        //public IEnumerable<CommentGrammar>? CommentGrammars { get; set; }
        public List<CommentGrammar>? CommentGrammars { get; set; }

        public string ContentG { get; set; }     
        public int? ParentCommentIdG { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public int? GrammarId { get; set; }
        
        
    }
}
