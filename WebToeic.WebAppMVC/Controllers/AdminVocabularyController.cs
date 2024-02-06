using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminVocabularyController : Controller
    {
        private readonly VocabularyService _vocabularyService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminVocabularyController(VocabularyService vocabularyService, WebToeicDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _vocabularyService = vocabularyService;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
 
        [HttpGet]
        [Route("AdminVocabulary/{page?}")]
        public async Task<IActionResult> Index(string nameVC = "", string orderBy = "", int currentPage = 1)
        {
            nameVC = string.IsNullOrEmpty(nameVC)?"" : nameVC.ToLower();
            var vocaData = new VocabularyVM();
            vocaData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            vocaData.OrderByID = orderBy == "id" ? "id_desc" : "id";

            var vocabularies = await _dbcontext.Vocabularies
                                .Where(emp => nameVC == "" || emp.VocabularyName.ToLower().StartsWith(nameVC))
                                .ToListAsync();    
          
            switch(orderBy)
            {
                case "name_desc":
                    vocabularies = vocabularies.OrderByDescending(emp => emp.VocabularyName).ToList();
                    break;
                case "id_desc":
                    vocabularies = vocabularies.OrderByDescending(emp => emp.Id).ToList();
                    break;
                case "id":
                    vocabularies = vocabularies.OrderBy(emp => emp.Id).ToList();
                    break;
                default:
                    vocabularies = vocabularies.OrderBy(emp => emp.VocabularyName).ToList();
                    break;
            }
            int totalRecords = vocabularies.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            vocabularies = vocabularies.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            vocaData.Vocabularies = vocabularies;
            vocaData.CurrentPage = currentPage;
            vocaData.TotalPages = totalPages;
            vocaData.PageSize = pageSize;
            vocaData.NameVoca = nameVC;
            vocaData.OrderBy = orderBy;
            return View(vocaData);
           
        }

        [HttpGet]
        [Route("AdminVocabulary/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminVocabulary/Create")]
        public async Task<IActionResult> Create(IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, List<IFormFile> audiosFile, string vocabularyName)
        {
            if (ModelState.IsValid)
            {
                try 
                {               

                    if (imageFile != null && imageFile.Length > 0)
                    {                   

                        var fileName = Path.GetFileName(imageFile.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageVoca");
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                  
                        var voca = new Vocabulary
                        {
                            ImageV = fileName,
                            VocabularyName = vocabularyName
                        };
              
                        _dbcontext.Vocabularies.Add(voca);
                        await _dbcontext.SaveChangesAsync();

                        var vocaId = voca.Id;

                        if (excelFile != null && excelFile.Length > 0)
                        {
                            var stream = excelFile.OpenReadStream();

                            try
                            {
                                using (var package = new ExcelPackage(stream))
                                {
                                    var worksheet = package.Workbook.Worksheets.First();
                                    var rowCount = worksheet.Dimension.Rows;

                                    try
                                    {

                                        for (var row = 2; row <= rowCount; row++)
                                        {
                                            var audioMp3 = worksheet.Cells[row, 1].Value?.ToString(); ;
                                            var content = worksheet.Cells[row, 2].Value?.ToString();
                                            var imageVC = worksheet.Cells[row, 3].Value?.ToString();
                                            var meaning = worksheet.Cells[row, 4].Value?.ToString();
                                            var number = worksheet.Cells[row, 5].Value;
                                            var sentence = worksheet.Cells[row, 6].Value?.ToString();
                                            var transcribed = worksheet.Cells[row, 7].Value?.ToString();

                                            int numberValue = 0;
                                            if (number is double || number is float)
                                            {
                                                numberValue = Convert.ToInt32(number);
                                            }
                                            else if (number is string)
                                            {
                                                if (int.TryParse((string)number, out int parsedNumber))
                                                {
                                                    numberValue = parsedNumber;
                                                }

                                            }

                                            var vocacontent = new VocabularyContent()
                                            {

                                                AudioMp3 = audioMp3,
                                                Content = content,
                                                ImageVC = imageVC,
                                                Meaning = meaning,
                                                Number = numberValue,
                                                Sentence = sentence,
                                                Transcribed = transcribed,
                                                VocabularyContentId = vocaId
                                            };
                                            _dbcontext.VocabularyContents.Add(vocacontent);
                                            _dbcontext.SaveChanges();
                                        }

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

                    }

                    if (imagesFile.Count > 0)
                    {
                        try
                        {
                            foreach (var image in imagesFile)
                            {
                                var fileNames = Path.GetFileName(image.FileName);
                                string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageVoca");
                                string filePaths = Path.Combine(uploadsFolders, fileNames);
                                using (var fileStreams = new FileStream(filePaths, FileMode.Create))
                                {
                                    image.CopyToAsync(fileStreams);
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            return View("NotFound");
                        }

                    }

                    if (audiosFile.Count > 0)
                    {
                        try
                        {
                            foreach (var audio in audiosFile)
                            {
                                var audioName = Path.GetFileName(audio.FileName);
                                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioVoca");
                                string filePaths = Path.Combine(uploadsFolder, audioName);
                                using (var fileStream = new FileStream(filePaths, FileMode.Create))
                                {
                                    audio.CopyToAsync(fileStream);
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            return View("NotFound");
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
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện hành động !";
                return View("Create");
            }
            
        }


        [HttpGet]
        [Route("AdminVocabulary/Edit/{id}")]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminVocabulary/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, IFormFile? imageFile, IFormFile? excelFile, List<IFormFile>? imagesFile, List<IFormFile>? audiosFile, string vocabularyName)
        {

            try
            {
                var voca = await _dbcontext.Vocabularies.FindAsync(id);
                if (voca == null)
                {
                    return View("NotFound");
                }

                if (vocabularyName != null)
                {
                    voca.VocabularyName = vocabularyName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Trường tên là bắt buộc !";
                    return View("Edit");
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageVoca");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    voca.ImageV = fileName;

                }

                if (excelFile != null && excelFile.Length > 0)
                {
                    var stream = excelFile.OpenReadStream();

                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            try
                            {
                                var existingContents = _dbcontext.VocabularyContents.Where(vc => vc.VocabularyContentId == id).ToList();

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    var audioMp3 = worksheet.Cells[row, 1].Value?.ToString(); ;
                                    var content = worksheet.Cells[row, 2].Value?.ToString();
                                    var imageVC = worksheet.Cells[row, 3].Value?.ToString();
                                    var meaning = worksheet.Cells[row, 4].Value?.ToString();
                                    var number = worksheet.Cells[row, 5].Value;
                                    var sentence = worksheet.Cells[row, 6].Value?.ToString();
                                    var transcribed = worksheet.Cells[row, 7].Value?.ToString();

                                    int numberValue = 0;
                                    if (number is double || number is float)
                                    {
                                        numberValue = Convert.ToInt32(number);
                                    }
                                    else if (number is string)
                                    {
                                        if (int.TryParse((string)number, out int parsedNumber))
                                        {
                                            numberValue = parsedNumber;
                                        }

                                    }

                                    var vocacontent = existingContents.FirstOrDefault(vc => vc.Id == row - 1);

                                    if (vocacontent == null)
                                    {
                                        vocacontent = new VocabularyContent()
                                        {
                                            VocabularyContentId = id
                                        };
                                        _dbcontext.VocabularyContents.Add(vocacontent);
                                    }

                                    vocacontent.AudioMp3 = audioMp3;
                                    vocacontent.Content = content;
                                    vocacontent.ImageVC = imageVC;
                                    vocacontent.Meaning = meaning;
                                    vocacontent.Number = numberValue;
                                    vocacontent.Sentence = sentence;
                                    vocacontent.Transcribed = transcribed;
                                }

                                // Remove any existing contents that were not updated
                                var contentIdsToRemove = existingContents.Where(ec => !worksheet.Cells[ec.Id + 1, 1].Merge).Select(ec => ec.Id).ToList();
                                var contentsToRemove = _dbcontext.VocabularyContents.Where(vc => contentIdsToRemove.Contains(vc.Id) && vc.VocabularyContentId == id).ToList();
                                _dbcontext.VocabularyContents.RemoveRange(contentsToRemove);
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

                await _dbcontext.SaveChangesAsync();

                if (imagesFile.Count > 0)
                {
                    try
                    {
                        foreach (var image in imagesFile)
                        {
                            var fileNames = Path.GetFileName(image.FileName);
                            string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageVoca");
                            string filePaths = Path.Combine(uploadsFolders, fileNames);
                            using (var fileStreams = new FileStream(filePaths, FileMode.Create))
                            {
                                image.CopyToAsync(fileStreams);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return View("NotFound");
                    }
                }

                if (audiosFile.Count > 0)
                {
                    try
                    {
                        foreach (var audio in audiosFile)
                        {
                            var audioName = Path.GetFileName(audio.FileName);
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioVoca");
                            string filePaths = Path.Combine(uploadsFolder, audioName);
                            using (var fileStream = new FileStream(filePaths, FileMode.Create))
                            {
                                audio.CopyToAsync(fileStream);
                            }
                        }
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

            TempData["SuccessMessage"] = "Chỉnh sửa đã được thực hiện thành công.";
            return RedirectToAction("Edit");

        }

        [HttpPost]
        [Route("AdminVocabulary/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int vocabularyId = id;

            var vocabulary = await _dbcontext.Vocabularies
                            .Include(v => v.VocabularyContents)
                            .FirstOrDefaultAsync(v => v.Id == vocabularyId);

            if (vocabulary != null)
            {
                _dbcontext.Vocabularies.Remove(vocabulary);

                // Xoá các câu hỏi có cùng Id với bản ghi Vocabulary trong bảng VocabularyContents
                var questions = _dbcontext.VocabularyContents
                    .Where(vc => vc.VocabularyContentId == vocabularyId);

                _dbcontext.VocabularyContents.RemoveRange(questions);

                await _dbcontext.SaveChangesAsync();
               
            }
            return RedirectToAction("Index");
        }
    }
}
