using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminCmtListenController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;

        public AdminCmtListenController(WebToeicDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [Route("AdminCmtListen/{page?}")]
        public async Task<IActionResult> Index(string nameListen = "", string nameUser = "", string orderBy = "", int currentPage = 1)
        {
            nameListen = string.IsNullOrEmpty(nameListen) ? "" : nameListen.ToLower();
            nameUser = string.IsNullOrEmpty(nameUser) ? "" : nameUser.ToLower();

            var cmtlistenData = new AdminCmtListenVM();
            cmtlistenData.OrderByUserName = string.IsNullOrEmpty(orderBy) ? "username_desc" : "username";
            cmtlistenData.OrderByListenName = string.IsNullOrEmpty(orderBy) ? "listenname_desc" : "";

            var cmtlistens = await _dbcontext.CommentListens
                                .Where(emp => (string.IsNullOrEmpty(nameListen) || emp.ListenNameCmtL.ToLower().StartsWith(nameListen))
                                    && (string.IsNullOrEmpty(nameUser) || emp.UserNameCmtL.ToLower().StartsWith(nameUser)))
                                .ToListAsync();

            switch (orderBy)
            {
                case "listenname_desc":
                    cmtlistens = cmtlistens.OrderByDescending(emp => emp.ListenNameCmtL).ToList();
                    break;
                case "username_desc":
                    cmtlistens = cmtlistens.OrderByDescending(emp => emp.UserNameCmtL).ToList();
                    break;
                case "username":
                    cmtlistens = cmtlistens.OrderBy(emp => emp.UserNameCmtL).ToList();
                    break;
                default:
                    cmtlistens = cmtlistens.OrderBy(emp => emp.ListenNameCmtL).ToList();
                    break;
            }
            int totalRecords = cmtlistens.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            cmtlistens = cmtlistens.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            cmtlistenData.CommentListens = cmtlistens;
            cmtlistenData.CurrentPage = currentPage;
            cmtlistenData.TotalPages = totalPages;
            cmtlistenData.PageSize = pageSize;
            
            cmtlistenData.OrderBy = orderBy;
            return View(cmtlistenData);
        }



        [HttpPost]
        [Route("AdminCmtListen/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int cmtListenId = id;

            var cmtListen = await _dbcontext.CommentListens
                            .FirstOrDefaultAsync(v => v.Id == id);


            if (cmtListen != null)
            {
                _dbcontext.CommentListens.Remove(cmtListen);

                // Xoá các bản ghi con có cột ParentId bằng commentId
                var replies = _dbcontext.CommentListens.Where(c => c.ParentCommentId == id);
                _dbcontext.CommentListens.RemoveRange(replies);

                await _dbcontext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}

