using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Reflection.Emit.Commands;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    public static class AppRole
    {
        public const string Admin = "admin";
        public const string Employee = "employee";
        public const string User = "user";


    }

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration configuration;

        /*private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;*/

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

 

        public IActionResult Register()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model) 
        {
            try 
            { 
                if (ModelState.IsValid) 
                {
                    var chkEmail = await userManager.FindByEmailAsync(model.Email);
                    if (chkEmail != null) 
                    {
                        ModelState.AddModelError(string.Empty, "Email đã tồn tại");
                        return View(model);
                    }
                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                    };
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (!await roleManager.RoleExistsAsync(AppRole.User))
                        {
                            await roleManager.CreateAsync(new Role(AppRole.User));
                        }
                        await signInManager.SignInAsync(user, isPersistent: false);


                        
                        return RedirectToAction("Index", "Home");
                    }
                    if(result.Errors.Count() > 0)
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                
            } 
            catch (Exception) 
            {
                throw;
            }
            return View(model);

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var chkEmail = await userManager.FindByEmailAsync(model.Email) ;
                    if (chkEmail == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email not found");
                        return View(model);
                    }
                    if(await userManager.CheckPasswordAsync(chkEmail, model.Password) == false)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                         return View(model);
                    }
                    var result = await signInManager.PasswordSignInAsync(chkEmail, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Kiểm tra xem người dùng có trong Role đã định sẵn hay không
                        //var isInRole = await userManager.IsInRoleAsync(chkEmail, "Admin");

                        // Nếu không có, thêm người dùng vào Role
                        /*if (!isInRole)
                        {
                           await userManager.AddToRoleAsync(chkEmail, "Admin");
                        }*/

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if(ModelState.IsValid)
            {
                //int userId = int.Parse(user.Id);
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var linkhref = "<a href= '" + Url.Action("ResetPassword", "Account", new { email = model.Email, code = token }, "https") + "'> Reset Password </a>";
                    string subject = "Password change";
                    string body = "<b> Password link reset password! </b><br/>" + linkhref;
                    SendMail(model.Email, subject, body);
                }
            }

            return View();
        }

        public bool SendMail (string toEmail, string subject, string emailBody)
        {
            try
            {

                string senderEmail = configuration["EmailSettings:SenderEmail"];
                string senderPassword = configuration["EmailSettings:SenderPassword"];

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                //MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
                //var email = new MimeMessage();
                //email.From = 
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string code, string? email)
        {

            return code == null ? View("NotFound") : View();
        }

        [HttpPost]
        
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            /* if(!ModelState.IsValid)
             {
                 return View(model);
             }*/

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}
