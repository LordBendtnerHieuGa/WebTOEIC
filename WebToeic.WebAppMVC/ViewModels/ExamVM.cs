using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class ExamVM
    {
        public List<Test> Tests { get; set; }
        public string OrderByName { get; set; }
        public string OrderByID { get; set; }
        public string NameExam { get; set; }
        public string OrderBy { get; set; }
        public int TotalRecord { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
