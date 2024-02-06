using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class ListenVM
    {
        public List<Listening> Listenings { get; set; }
        public string OrderByName { get; set; }
        public string OrderByID { get; set; }
        public string OrderByPart { get; set; }
        public string OrderByLevel { get; set; }

        public string NameListen { get; set; }
        public string OrderBy { get; set; }
        public int TotalRecord { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
