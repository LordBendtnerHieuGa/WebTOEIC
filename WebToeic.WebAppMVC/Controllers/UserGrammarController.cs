using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    public class UserGrammarController : Controller
    {
        private readonly GrammarService _grammarService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly UserManager<User> _userManager;


        public UserGrammarController(GrammarService grammarService, WebToeicDbContext dbcontext, UserManager<User> userManager)
        {
            _grammarService = grammarService;
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/WebToeicF/Grammars/{page?}")]
        public async Task<IActionResult> Index(string nameGrammar = "", int currentPage = 1)
        {

            nameGrammar = string.IsNullOrEmpty(nameGrammar) ? "" : nameGrammar.ToLower();
            var grammarData = new GrammarVM();
           

            var grammars = await _dbcontext.Grammars
                                .Where(emp => nameGrammar == "" || emp.GrammarName.ToLower().StartsWith(nameGrammar))
                                .ToListAsync();

            
            int totalRecords = grammars.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            grammars = grammars.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            grammarData.Grammars = grammars;
            grammarData.CurrentPage = currentPage;
            grammarData.TotalPages = totalPages;
            grammarData.PageSize = pageSize;
            grammarData.NameGrammar = nameGrammar;
            
            return View(grammarData);
        }

        [HttpGet]
        [Route("/WebToeicF/Grammars/Detail/{id?}")]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var comments = await _dbcontext.Comments
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.Replies)
            .Where(c => c.GrammarId == id)
            .ToListAsync();

            // Structure comments into a tree
            var rootComments = comments
                .Where(c => c.ParentCommentId == null)
                .AsParallel()
                .ToList();

            // Lấy Id của người dùng
            var user = await _userManager.GetUserAsync(HttpContext.User);
            //var userId = user.Id;
            //var userName = user.UserName;

            var grammar = await _grammarService.GetSingleById(id);
            var grammarData = new CommentAndGrammarVM()
            {
                Grammar = grammar,
                CommentGrammars = rootComments,
                //UserName = userName,
                
            };
            return View(grammarData);
        }

        [Authorize]
        [HttpPost]
        [Route("/WebToeicF/Grammars/Detail/{id?}")]
        public async Task<IActionResult> CreateCommentGrammar(int id,CommentAndGrammarVM cmtGrammar)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại từ HttpContext
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userName = user.UserName;
                // Lấy Id của người dùng
                var userId = user.Id;
                // Lay ten bai grammar
                var grammar = await _dbcontext.Grammars.FirstOrDefaultAsync(x => x.Id == id);
                var grammarName = grammar.GrammarName;

                var newCmtGrammar = new CommentGrammar
                {
                    ParentCommentId = cmtGrammar.ParentCommentIdG,
                    UserId = userId,
                    GrammarId= id,
                    Content = cmtGrammar.ContentG,
                    Time = DateTime.Now,
                    UserNameCmtG = userName,
                    GrammarNameCmtG = grammarName
                };

                await _dbcontext.Comments.AddAsync(newCmtGrammar);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("Detail", new {id = id});
            }

            // TODO: Throw some error
            return RedirectToAction("Detail", new { id = id });
        }

    }
}
