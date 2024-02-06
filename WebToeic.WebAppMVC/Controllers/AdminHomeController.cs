using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Models;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminHomeController : Controller
    {
        private readonly ILogger<AdminHomeController> _logger;
        private readonly WebToeicDbContext _dbcontext;

        public AdminHomeController(ILogger<AdminHomeController> logger, WebToeicDbContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        [HttpGet]
        [Route("AdminHome")]
        public async Task<IActionResult> Index()
        {
            int grammars = await _dbcontext.Grammars.CountAsync();
            int vocas = await _dbcontext.Vocabularies.CountAsync();
            int listens = await _dbcontext.Listenings.CountAsync();
            int reads = await _dbcontext.Readings.CountAsync();
            int exams = await _dbcontext.Tests.CountAsync();
            int users = await _dbcontext.Users.CountAsync();
            int roles = await _dbcontext.Roles.CountAsync();
            int cmtGrammar = await _dbcontext.Comments.CountAsync();
            int cmtVoca = await _dbcontext.CommentVocabularies.CountAsync();
            int cmtListen = await _dbcontext.CommentListens.CountAsync(); 
            int cmtRead = await _dbcontext.Readings.CountAsync();

            var totalDasboard = new AdminHomeDasboardVM()
            {
                TotalGrammar = grammars,
                TotalVoca = vocas,
                TotalListen = listens,
                TotalRead = reads,
                TotalExam = exams,
                TotalUser = users,
                TotalRole = roles,
                TotalCmtGramar = cmtGrammar,
                TotalCmtVoca = cmtVoca,
                TotalCmtListen = cmtListen,
                TotalCmtRead = cmtRead
            };

            return View(totalDasboard);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

