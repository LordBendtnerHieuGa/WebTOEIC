using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminCmtGrammarController : Controller
    {
        private readonly WebToeicDbContext _dbcontext;

        public AdminCmtGrammarController(WebToeicDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [Route("AdminCmtGrammar/{page?}")]
        public async Task<IActionResult> Index(string nameGrammar = "", string nameUser = "", string orderBy = "", int currentPage = 1)
        {
            nameGrammar = string.IsNullOrEmpty(nameGrammar) ? "" : nameGrammar.ToLower();
            nameUser = string.IsNullOrEmpty(nameUser) ? "" : nameUser.ToLower();

            var cmtgrammarData = new AdminCmtGrammarVM();
            cmtgrammarData.OrderByUserName = string.IsNullOrEmpty(orderBy) ? "username_desc" : "username";
            cmtgrammarData.OrderByGrammarName = string.IsNullOrEmpty(orderBy) ? "grammarname_desc" : "";

            var cmtgrammars = await _dbcontext.Comments
                                .Where(emp => (string.IsNullOrEmpty(nameGrammar) || emp.GrammarNameCmtG.ToLower().StartsWith(nameGrammar))
                                    && (string.IsNullOrEmpty(nameUser) || emp.UserNameCmtG.ToLower().StartsWith(nameUser)))
                                .ToListAsync();

            switch (orderBy)
            {
                case "grammarname_desc":
                    cmtgrammars = cmtgrammars.OrderByDescending(emp => emp.GrammarNameCmtG).ToList();
                    break;
                case "username_desc":
                    cmtgrammars = cmtgrammars.OrderByDescending(emp => emp.UserNameCmtG).ToList();
                    break;
                case "username":
                    cmtgrammars = cmtgrammars.OrderBy(emp => emp.UserNameCmtG).ToList();
                    break;
                default:
                    cmtgrammars = cmtgrammars.OrderBy(emp => emp.GrammarNameCmtG).ToList();
                    break;
            }
            int totalRecords = cmtgrammars.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            cmtgrammars = cmtgrammars.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            cmtgrammarData.CommentGrammars = cmtgrammars;
            cmtgrammarData.CurrentPage = currentPage;
            cmtgrammarData.TotalPages = totalPages;
            cmtgrammarData.PageSize = pageSize;
            //cmtgrammarData.NameGrammar = nameGrammar;
            cmtgrammarData.OrderBy = orderBy;
            return View(cmtgrammarData);
        }

     
            
        [HttpPost]
        [Route("AdminCmtGrammar/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int cmtGrammarId = id;

            var cmtGrammar = await _dbcontext.Comments
                            .FirstOrDefaultAsync(v => v.Id == id);


            if (cmtGrammar != null)
            {
                _dbcontext.Comments.Remove(cmtGrammar);

                // Xoá các bản ghi con có cột ParentId bằng commentId
                var replies = _dbcontext.Comments.Where(c => c.ParentCommentId == id);
                _dbcontext.Comments.RemoveRange(replies);

                await _dbcontext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
