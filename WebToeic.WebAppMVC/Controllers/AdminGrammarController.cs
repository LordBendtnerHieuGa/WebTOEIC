using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PagedList.Mvc;
using WebToeic.WebAppMVC.ViewModels;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using PagedList;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using Utilities.DataTypes.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminGrammarController : Controller
    {
        private readonly GrammarService _grammarService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AdminGrammarController(GrammarService grammarService, WebToeicDbContext dbcontext, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _grammarService = grammarService;
            _dbcontext = dbcontext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("AdminGrammar/{page?}")]
        public async Task<IActionResult> Index(string nameGrammar = "", string orderBy = "", int currentPage = 1)
        {
            nameGrammar = string.IsNullOrEmpty(nameGrammar) ? "" : nameGrammar.ToLower();
            var grammarData = new GrammarVM();
            grammarData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            grammarData.OrderByID = orderBy == "id" ? "id_desc" : "id";

            var grammars = await _dbcontext.Grammars
                                .Where(emp => nameGrammar == "" || emp.GrammarName.ToLower().StartsWith(nameGrammar))
                                .ToListAsync();

            switch (orderBy)
            {
                case "name_desc":
                    grammars = grammars.OrderByDescending(emp => emp.GrammarName).ToList();
                    break;
                case "id_desc":
                    grammars = grammars.OrderByDescending(emp => emp.Id).ToList();
                    break;
                case "id":
                    grammars = grammars.OrderBy(emp => emp.Id).ToList();
                    break;
                default:
                    grammars = grammars.OrderBy(emp => emp.GrammarName).ToList();
                    break;
            }
            int totalRecords = grammars.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            grammars = grammars.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            grammarData.TotalRecord = totalRecords;  
            grammarData.Grammars = grammars;
            grammarData.CurrentPage = currentPage;
            grammarData.TotalPages = totalPages;
            grammarData.PageSize = pageSize;
            grammarData.NameGrammar = nameGrammar;
            grammarData.OrderBy = orderBy;
            return View(grammarData);
        }


        [HttpGet]
        [Route("AdminGrammar/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminGrammar/Create")]
        public IActionResult Create(IFormFile formFile, List<IFormFile> imagesFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile.Length > 0)
                {
                    var stream = formFile.OpenReadStream();

                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;


                            try
                            {
                                var imageG = worksheet.Cells[2, 1].Value?.ToString(); ;
                                var htmlContent = worksheet.Cells[2, 2].Value?.ToString();
                                var markDownContent = worksheet.Cells[2, 3].Value?.ToString();
                                var grammarName = worksheet.Cells[2, 4].Value?.ToString();

                                var grammar = new Grammar()
                                {

                                    ImageG = imageG,
                                    HtmlContent = htmlContent,
                                    MarkDownContent = markDownContent,
                                    GrammarName = grammarName
                                };

                                _dbcontext.Grammars.Add(grammar);

                            }
                            catch (Exception ex)
                            {
                                return View("NotFound");
                            }

                        }
                        _dbcontext.SaveChanges();

                        if (imagesFile.Count > 0)
                        {
                            foreach (var image in imagesFile)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageGrammar");
                                string filePath = Path.Combine(uploadsFolder, fileName);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    image.CopyToAsync(fileStream);
                                }

                            }
                        }
                        TempData["SuccessMessage"] = "Tạo mới thành công !";
                        return View("Create");

                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện hành động !";
                        return View("Create");
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("AdminGrammar/Edit/{id}")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminGrammar/Edit/{id}")]
        public IActionResult Edit(int id, IFormFile formFile, List<IFormFile> imagesFile)
        {
            if (ModelState.IsValid)
            {
                var grammar = _dbcontext.Grammars.FirstOrDefault(g => g.Id == id);
                if (grammar == null)
                {
                    return View("NotFound");
                }

                if (formFile.Length > 0)
                {
                    var stream = formFile.OpenReadStream();

                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            try
                            {
                                var imageG = worksheet.Cells[2, 1].Value?.ToString(); ;
                                var htmlContent = worksheet.Cells[2, 2].Value?.ToString();
                                var markDownContent = worksheet.Cells[2, 3].Value?.ToString();
                                var grammarName = worksheet.Cells[2, 4].Value?.ToString();

                                grammar.ImageG = imageG;
                                grammar.HtmlContent = htmlContent;
                                grammar.MarkDownContent = markDownContent;
                                grammar.GrammarName = grammarName;

                                _dbcontext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                return View("NotFound");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return View("NotFound");
                    }
                }

                if (imagesFile.Count > 0)
                {
                    foreach (var image in imagesFile)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageGrammar");
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyToAsync(fileStream);
                        }
                    }
                }

                TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                return View("Edit");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện hành động !";
                return View("Edit");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("AdminGrammar/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int grammarId = id;

            var grammar = await _dbcontext.Grammars
                            .FirstOrDefaultAsync(v => v.Id == grammarId);

            if (grammar != null)
            {
                _dbcontext.Grammars.Remove(grammar);

                
                await _dbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
