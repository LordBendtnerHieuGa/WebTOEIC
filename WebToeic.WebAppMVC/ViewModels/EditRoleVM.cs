using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class EditRoleVM
    {
        public EditRoleVM() 
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Tên Role là bắt buộc")]
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public List<string> Users { get; set; }
    }
}
