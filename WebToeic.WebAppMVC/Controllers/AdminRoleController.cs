using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminRoleController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;

        public AdminRoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [Route("/AdminRole/CreateRole")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Route("/AdminRole/CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleVM vm)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role
                {
                    Name = vm.RoleName,
                    Description = vm.RoleDescription
                };
                IdentityResult result = await roleManager.CreateAsync(role);

                if(result.Succeeded) 
                {
                    TempData["SuccessMessage"] = "Tạo mới thành công !";
                    return View("CreateRole");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(vm);
        }

        [HttpGet]
        [Route("/AdminRole/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if(role == null)
            {
                return View("Not Found");
            }

            var model = new EditRoleVM
            {
                Id = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description
            };

            foreach(var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }


        [HttpPost]
        [Route("/AdminRole/Edit/{id}")]
        public async Task<IActionResult> Edit(EditRoleVM model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                return View("Not Found");
            }
            else
            {
                role.Name = model.RoleName;
                role.Description = model.RoleDescription;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                    return View("Edit");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                return View("Not Found");
            }

            var model = new List<UserRoleVM>();
            foreach(var user in userManager.Users)
            {
                var userRoleVM = new UserRoleVM() 
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleVM.IsSelected = true;
                }
                else
                {
                    userRoleVM.IsSelected = false;
                }
                model.Add(userRoleVM);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleVM> model, string roleId)
        {

            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View("Not Found");
            }
            for(int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else 
                {
                    continue;
                }

                if(result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("Edit", new { Id = roleId });
                }
            }

            return RedirectToAction("Edit", new { Id = roleId });

        }


        [HttpPost]
        [Route("AdminRole/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {


            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return View("Not Found");
            }

            // Xoá người dùng
            var result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                // Xử lý khi không xoá được người dùng
                //return BadRequest();
                return View("Not Found");
            }

            // Xử lý thành công

            return RedirectToAction("Index");
        }

    }
}
