using System.ComponentModel.DataAnnotations;

namespace WebToeic.WebAppMVC.ViewModels
{
    public class ResetPasswordVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Hãy nhập email của bạn")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Hãy nhập mật khẩu của bạn")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Hãy xác nhận mật khẩu của bạn")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = default!;

        public string Code { get; set; }
    }
}
