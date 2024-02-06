using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class GrammarVM
    {
        public List<Grammar> Grammars { get; set; }
        public string OrderByName { get; set; }
        public string OrderByID { get; set; }
        public string NameGrammar { get; set; }
        public string OrderBy { get; set; }
        public int TotalRecord { get; set; } // xem lai neu loi

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
