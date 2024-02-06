using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class LoginVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Hãy nhập email của bạn")]
        public string Email { get; set; } = default!;

        /*[Required(ErrorMessage = "Hãy nhập username của bạn")]
        public string UserName { get; set; } = default!;*/

        [Required(ErrorMessage = "Hãy nhập mật khẩu của bạn")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
