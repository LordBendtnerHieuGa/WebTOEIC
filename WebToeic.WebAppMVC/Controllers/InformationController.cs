using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    public class InformationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;
        //private readonly User user;

        /*private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;*/

        public InformationController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            //this.user = user;
        }

        [HttpGet]
        public async Task<IActionResult> EditInfor(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = await userManager.FindByIdAsync(id);
                if (user != null) 
                {
                    InforVM model = new InforVM
                    {
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        DOB = user.DOB,
                        Address = user.Address,
                    };

                    return View(model);
                }
            }
            return RedirectToAction("Index", "AdminHome");
        }

        [HttpPost]
        public async Task<IActionResult> EditInfor(InforVM infor)
        {

            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(infor.Id);
                if (user != null)
                {
                    // Kiem tra mat khau hien tai

                    bool isOldPasswordCorrect = await userManager.CheckPasswordAsync(user, infor.OldPassword);
                    if (!isOldPasswordCorrect)
                    {
                        ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng");
                        return View(infor);
                    }

                    // Thay doi mat khau
                    var changePasswordResult = await userManager.ChangePasswordAsync(user, infor.OldPassword, infor.Password);
                    if (changePasswordResult.Succeeded)
                    {
                        await signInManager.RefreshSignInAsync(user);
                    }
                    user.UserName = infor.UserName;
                    user.PhoneNumber = infor.PhoneNumber;
                    user.Email = infor.Email;
                    user.DOB = infor.DOB;
                    user.Address = infor.Address;

                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Thay đổi thông tin cá nhân thành công !";
                        return View(infor);
                    };

                    return View(infor);
                    //return View(infor);
                }
            }
            return View(infor);
        }

       

    }
}
