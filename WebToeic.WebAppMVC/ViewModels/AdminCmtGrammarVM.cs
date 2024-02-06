using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class AdminCmtGrammarVM
    {
        
        public List<CommentGrammar> CommentGrammars { get; set; }

        public string OrderByUserName { get; set; }
        public string OrderByGrammarName { get; set; }
     
        public string OrderBy { get; set; }


        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
