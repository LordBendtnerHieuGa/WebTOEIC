using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
