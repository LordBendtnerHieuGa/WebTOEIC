using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PagedList;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminAccController : Controller
    {

        //private UserManager<AccVM> userManager;
        private readonly UserManager<User> _userManager;
        private readonly WebToeicDbContext _dbcontext;
        private readonly RoleManager<Role> _roleManager;
        public AdminAccController(UserManager<User> userManager, WebToeicDbContext dbcontex, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _dbcontext = dbcontex;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("AdminAcc/{page?}")]
        public async Task<IActionResult> Index(string nameUser = "", string email = "", string orderBy = "", string address = "",string phoneUser = "", int currentPage = 1)
        {
            // Search
            nameUser = string.IsNullOrEmpty(nameUser) ? "" : nameUser.ToLower();
            var userData = new AccAdminVM();

            userData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "name";
            userData.OrderByEmail = orderBy == "email" ? "email_desc" : "email";
            userData.OrderByAddress = orderBy == "address" ? "address_desc" : "address";
            


            var users = await _dbcontext.Users
                        .Where(emp => (string.IsNullOrEmpty(nameUser) || emp.UserName.ToLower().StartsWith(nameUser))
                                    && (string.IsNullOrEmpty(email) || emp.Email.ToLower().StartsWith(email))

                                    && (string.IsNullOrEmpty(phoneUser) || emp.PhoneNumber.StartsWith(phoneUser))
                                    && (string.IsNullOrEmpty(address) || emp.Address.ToLower().StartsWith(address)))
                        .ToListAsync();
            // OrderBy
            switch (orderBy)
            {
                case "name_desc":
                    users = users.OrderByDescending(emp => emp.UserName).ToList();
                    break;

                case "name":
                    users = users.OrderBy(emp => emp.UserName).ToList();
                    break;

                case "email_desc":
                    users = users.OrderByDescending(emp => emp.Email).ToList();
                    break;

                case "email":
                    users = users.OrderBy(emp => emp.Email).ToList();
                    break;

                case "address_desc":
                    users = users.OrderByDescending(emp => emp.Address).ToList();
                    break;

                case "address":
                    users = users.OrderBy(emp => emp.Address).ToList();
                    break;

               
            }

            // Pagination
            int totalRecords = users.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            users = users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            userData.Users = users;
            userData.CurrentPage = currentPage;
            userData.TotalPages = totalPages;
            userData.PageSize = pageSize;
            userData.NameUser = nameUser;
            userData.OrderBy = orderBy;
            userData.TotalRecord = totalRecords;

            return View(userData);

        }


        [HttpGet]
        [Route("AdminAcc/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                User user = await _userManager.FindByIdAsync(id);
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("AdminAcc/Edit/{id}")]
        public async Task<IActionResult> Edit(InforVM model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.DOB = model.DOB;
                user.Address = model.Address;

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công !";
                    return View("Edit");
                };

                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện hành động !";   
                return View(model);
                //return View(infor);
            }
            return View(model);
        }

        [HttpPost]
        [Route("AdminAcc/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {


            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                // Xử lý khi không tìm thấy người dùng
                return View("Not Found");
            }

            // Xoá người dùng
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                // Xử lý khi không xoá được người dùng
                //return BadRequest();
                return View("Not Found");
            }

            // Xoá bản ghi trong bảng AspNetUserRoles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                var roleObj = await _roleManager.FindByNameAsync(role);
                if (roleObj != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }

            // Xử lý thành công
           
            return RedirectToAction("Index");
        }


    }
   
}
