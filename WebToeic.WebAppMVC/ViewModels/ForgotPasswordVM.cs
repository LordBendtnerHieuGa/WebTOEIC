using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class ForgotPasswordVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Hãy nhập email của bạn")]
        public string Email { get; set; } = default!;
    }
}
