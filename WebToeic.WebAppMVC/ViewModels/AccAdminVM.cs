using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class AccAdminVM
    {
        public List<User> Users { get; set; }
        public string OrderByName { get; set; }
        public string OrderByEmail { get; set; }
        public string OrderByAddress { get; set; }
        public string NameUser { get; set; }
        public string OrderBy { get; set; }
        public int TotalRecord { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
