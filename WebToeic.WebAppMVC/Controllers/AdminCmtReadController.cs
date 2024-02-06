using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminCmtReadController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;

        public AdminCmtReadController(WebToeicDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [Route("AdminCmtRead/{page?}")]
        public async Task<IActionResult> Index(string nameRead = "", string nameUser = "", string orderBy = "", int currentPage = 1)
        {
            nameRead = string.IsNullOrEmpty(nameRead) ? "" : nameRead.ToLower();
            nameUser = string.IsNullOrEmpty(nameUser) ? "" : nameUser.ToLower();

            var cmtreadData = new AdminCmtReadVM();
            cmtreadData.OrderByUserName = string.IsNullOrEmpty(orderBy) ? "username_desc" : "username";
            cmtreadData.OrderByReadName = string.IsNullOrEmpty(orderBy) ? "readname_desc" : "";

            var cmtreads = await _dbcontext.CommentReadings
                                .Where(emp => (string.IsNullOrEmpty(nameRead) || emp.ReadingNameCmtR.ToLower().StartsWith(nameRead))
                                    && (string.IsNullOrEmpty(nameUser) || emp.UserNameCmtR.ToLower().StartsWith(nameUser)))
                                .ToListAsync();

            switch (orderBy)
            {
                case "readname_desc":
                    cmtreads = cmtreads.OrderByDescending(emp => emp.ReadingNameCmtR).ToList();
                    break;
                case "username_desc":
                    cmtreads = cmtreads.OrderByDescending(emp => emp.UserNameCmtR).ToList();
                    break;
                case "username":
                    cmtreads = cmtreads.OrderBy(emp => emp.UserNameCmtR).ToList();
                    break;
                default:
                    cmtreads = cmtreads.OrderBy(emp => emp.ReadingNameCmtR).ToList();
                    break;
            }
            int totalRecords = cmtreads.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            cmtreads = cmtreads.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            cmtreadData.CommentReads = cmtreads;
            cmtreadData.CurrentPage = currentPage;
            cmtreadData.TotalPages = totalPages;
            cmtreadData.PageSize = pageSize;

            cmtreadData.OrderBy = orderBy;
            return View(cmtreadData);
        }



        [HttpPost]
        [Route("AdminCmtRead/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int cmtReadId = id;

            var cmtRead = await _dbcontext.CommentReadings
                            .FirstOrDefaultAsync(v => v.Id == id);


            if (cmtRead != null)
            {
                _dbcontext.CommentReadings.Remove(cmtRead);

                // Xoá các bản ghi con có cột ParentId bằng commentId
                var replies = _dbcontext.CommentReadings.Where(c => c.ParentCommentId == id);
                _dbcontext.CommentReadings.RemoveRange(replies);

                await _dbcontext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
