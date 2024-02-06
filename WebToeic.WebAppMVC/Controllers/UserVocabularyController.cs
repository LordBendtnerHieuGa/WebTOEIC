using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PagedList;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    public class UserVocabularyController : Controller
    {
        private readonly VocabularyService _vocabularyService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly UserManager<User> _userManager;

        public UserVocabularyController(VocabularyService vocabularyService, WebToeicDbContext dbcontext, UserManager<User> userManager)
        {
            _vocabularyService = vocabularyService;
            _dbcontext = dbcontext;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Route("/WebToeicF/Vocabularies/{page?}")]
        public async Task<IActionResult> Index(string nameVC = "", int currentPage = 1)
        {
            nameVC = string.IsNullOrEmpty(nameVC) ? "" : nameVC.ToLower();
            var vocaData = new VocabularyVM();
            

            var vocabularies = await _dbcontext.Vocabularies
                                .Where(emp => nameVC == "" || emp.VocabularyName.ToLower().StartsWith(nameVC))
                                .ToListAsync();

            
            int totalRecords = vocabularies.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            vocabularies = vocabularies.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            vocaData.Vocabularies = vocabularies;
            vocaData.CurrentPage = currentPage;
            vocaData.TotalPages = totalPages;
            vocaData.PageSize = pageSize;
            vocaData.NameVoca = nameVC;
           
            return View(vocaData);

        }

        [HttpGet]
        [Route("/WebToeicF/Vocabularies/Detail/{id?}")]
        public async Task<IActionResult> Detail(int id)
        {
            var comments = await _dbcontext.CommentVocabularies
            .AsNoTrackingWithIdentityResolution()
            .Include(c => c.Replies)
            .Where(c => c.VocabularyId == id)
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

            var voca = await _dbcontext.Vocabularies
                        .FirstOrDefaultAsync(g => g.Id == id);
            var vocaContents = await _dbcontext.VocabularyContents.Where(vc => vc.VocabularyContentId == id).ToListAsync();
           

            var vocaData = new CommentAndVocabularyVM()
            {
                Vocabulary = voca,
                VocaContents = vocaContents,
                CommentVocabularies = rootComments,
                //UserName = userName,
               
            };
            return View(vocaData);
        }

        [Authorize]
        [HttpPost]
        [Route("/WebToeicF/Vocabularies/Detail/{id?}")]     
        public async Task<IActionResult> CreateCommentVoca(int id, CommentAndVocabularyVM cmtVoca)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng hiện tại từ HttpContext
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userName = user.UserName;
                // Lấy Id của người dùng
                var userId = user.Id;
                //Lay ten bai voca 
                var voca = await _dbcontext.Vocabularies.FirstOrDefaultAsync(x => x.Id == id);
                var vocaName = voca.VocabularyName;

                var newCmtVoca = new CommentVocabulary
                {
                    ParentCommentId = cmtVoca.ParentCommentIdV,
                    UserId = userId,
                    VocabularyId = id,
                    Content = cmtVoca.ContentV,
                    Time = DateTime.Now,
                    UserNameCmtV = userName,
                    VocaNameCmtV = vocaName,
                };

                await _dbcontext.CommentVocabularies.AddAsync(newCmtVoca);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("Detail", new { id = id });
            }

            // TODO: Throw some error
            return RedirectToAction("Detail", new { id = id });
        }


    }

}

        



