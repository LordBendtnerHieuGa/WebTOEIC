using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminCmtVocaController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;

        public AdminCmtVocaController(WebToeicDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [Route("AdminCmtVoca/{page?}")]
        public async Task<IActionResult> Index(string nameVoca = "", string nameUser = "", string orderBy = "", int currentPage = 1)
        {
            nameVoca = string.IsNullOrEmpty(nameVoca) ? "" : nameVoca.ToLower();
            nameUser = string.IsNullOrEmpty(nameUser) ? "" : nameUser.ToLower();

            var cmtvocaData = new AdminCmtVocaVM();
            cmtvocaData.OrderByUserName = string.IsNullOrEmpty(orderBy) ? "username_desc" : "username";
            cmtvocaData.OrderByVocaName = string.IsNullOrEmpty(orderBy) ? "vocaname_desc" : "";

            var cmtvocas = await _dbcontext.CommentVocabularies
                                .Where(emp => (string.IsNullOrEmpty(nameVoca) || emp.VocaNameCmtV.ToLower().StartsWith(nameVoca))
                                    && (string.IsNullOrEmpty(nameUser) || emp.UserNameCmtV.ToLower().StartsWith(nameUser)))
                                .ToListAsync();

            switch (orderBy)
            {
                case "vocaname_desc":
                    cmtvocas = cmtvocas.OrderByDescending(emp => emp.VocaNameCmtV).ToList();
                    break;
                case "username_desc":
                    cmtvocas = cmtvocas.OrderByDescending(emp => emp.UserNameCmtV).ToList();
                    break;
                case "username":
                    cmtvocas = cmtvocas.OrderBy(emp => emp.UserNameCmtV).ToList();
                    break;
                default:
                    cmtvocas = cmtvocas.OrderBy(emp => emp.VocaNameCmtV).ToList();
                    break;
            }
            int totalRecords = cmtvocas.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            cmtvocas = cmtvocas.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            cmtvocaData.CommentVocas = cmtvocas;
            cmtvocaData.CurrentPage = currentPage;
            cmtvocaData.TotalPages = totalPages;
            cmtvocaData.PageSize = pageSize;

            cmtvocaData.OrderBy = orderBy;
            return View(cmtvocaData);
        }



        [HttpPost]
        [Route("AdminCmtVoca/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int cmtVocaId = id;

            var cmtVoca = await _dbcontext.CommentVocabularies
                            .FirstOrDefaultAsync(v => v.Id == id);


            if (cmtVoca != null)
            {
                _dbcontext.CommentVocabularies.Remove(cmtVoca);

                // Xoá các bản ghi con có cột ParentId bằng commentId
                var replies = _dbcontext.CommentVocabularies.Where(c => c.ParentCommentId == id);
                _dbcontext.CommentVocabularies.RemoveRange(replies);

                await _dbcontext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}

