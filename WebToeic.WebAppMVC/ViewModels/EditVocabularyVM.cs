using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class EditVocabularyVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc")]
        public string VocabularyName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IFormFile? ExcelFile { get; set; }
        public List<IFormFile>? ImagesFile { get; set; }
        public List<IFormFile>? AudiosFile { get; set; }
        
    }
}
