using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using PagedList;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminListenController : Controller
    {

        private readonly ListenService _listenService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminListenController(ListenService listenService, WebToeicDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _listenService = listenService;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("AdminListen/{page?}")]
        public async Task<IActionResult> Index(string nameListen = "", string levelListen = "", string orderBy = "", string partListen = "", int currentPage = 1)
        {

            // Search
            nameListen = string.IsNullOrEmpty(nameListen) ? "" : nameListen.ToLower();
            var listenData = new ListenVM();

            listenData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "name";
            listenData.OrderByID = orderBy == "id" ? "id_desc" : "id";
            listenData.OrderByLevel = orderBy == "level" ? "level_desc" : "level";
            listenData.OrderByPart = orderBy == "part" ? "part_desc" : "part";


            var listens = await _dbcontext.Listenings
                        .Where(emp => (string.IsNullOrEmpty(nameListen) || emp.ListeningName.ToLower().StartsWith(nameListen))
                                    && (string.IsNullOrEmpty(levelListen) || emp.Level == int.Parse(levelListen))
                                    && (string.IsNullOrEmpty(partListen) || emp.Part == int.Parse(partListen)))
                        .ToListAsync();
            // OrderBy
            switch (orderBy)
            {
                case "name_desc":
                    listens = listens.OrderByDescending(emp => emp.ListeningName).ToList();
                    break;

                case "name":
                    listens = listens.OrderBy(emp => emp.ListeningName).ToList();
                    break;

                case "id_desc":
                    listens = listens.OrderByDescending(emp => emp.Id).ToList();
                    break;

                case "id":
                    listens = listens.OrderBy(emp => emp.Id).ToList();
                    break;

                case "level_desc":
                    listens = listens.OrderByDescending(emp => emp.Level).ToList();
                    break;

                case "level":
                    listens = listens.OrderBy(emp => emp.Level).ToList();
                    break;

                case "part_desc":
                    listens = listens.OrderByDescending(emp => emp.Part).ToList();
                    break;

                case "part":
                    listens = listens.OrderBy(emp => emp.Part).ToList();
                    break;
            }

            // Pagination
            int totalRecords = listens.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            listens = listens.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            listenData.Listenings = listens;
            listenData.CurrentPage = currentPage;
            listenData.TotalPages = totalPages;
            listenData.PageSize = pageSize;
            listenData.NameListen = nameListen;
            listenData.OrderBy = orderBy;
            listenData.TotalRecord = totalRecords;

            return View(listenData);
        }

        [HttpGet]
        [Route("AdminListen/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminListen/Create")]
        public async Task<IActionResult> Create(IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, List<IFormFile> audiosFile , string listenName, string selectedLevel, string selectedPart)
        {
            try
            {
                //var vocas = await _dbcontext.Vocabularies.ToListAsync();


                if (imageFile != null && imageFile.Length > 0 && listenName != null && selectedLevel != null && selectedPart != null)
                {

                    int level = int.Parse(selectedLevel);
                    int part = int.Parse(selectedPart);
             
                    // Lưu tệp ảnh vào thư mục trên máy chủ                   
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen");
                    string filePath = Path.Combine(uploadsFolder, fileName);
                 
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    var listen = new Listening
                    {
                        Level = level,
                        Part = part,
                        Photo = fileName,
                        ListeningName = listenName
                    };

                    _dbcontext.Listenings.Add(listen);
                    await _dbcontext.SaveChangesAsync();

                    var listenId = listen.Id;

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
                                        var question = worksheet.Cells[row, 1].Value?.ToString(); ;
                                        var correctanswer = worksheet.Cells[row, 2].Value?.ToString();
                                        var answer1 = worksheet.Cells[row, 3].Value?.ToString();
                                        var answer2 = worksheet.Cells[row, 4].Value?.ToString();
                                        var answer3 = worksheet.Cells[row, 5].Value?.ToString();
                                        var answer4 = worksheet.Cells[row, 6].Value?.ToString();
                                        var explain = worksheet.Cells[row, 7].Value?.ToString();
                                        var photo = worksheet.Cells[row, 8].Value?.ToString();
                                        var order = worksheet.Cells[row, 9].Value;
                                        var audioL = worksheet.Cells[row, 10].Value?.ToString();

                                        int numberValue = 0;
                                        if (order is double || order is float)
                                        {
                                            numberValue = Convert.ToInt32(order);
                                        }
                                        else if (order is string)
                                        {
                                            if (int.TryParse((string)order, out int parsedNumber))
                                            {
                                                numberValue = parsedNumber;
                                            }

                                        }

                                        var listeningQuestion = new ListeningQuestion()
                                        {

                                            Question = question,
                                            CorrectAnswer = correctanswer,
                                            Answer1 = answer1,
                                            Answer2 = answer2,
                                            Answer3 = answer3,
                                            Answer4 = answer4,
                                            Explain = explain,
                                            Photo = photo,
                                            Order = numberValue,
                                            AudioL = audioL,
                                            ListeningId = listenId
                                        };
                                        _dbcontext.ListeningQuestions.Add(listeningQuestion);
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
                            string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen");
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
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioListen");
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


        [HttpGet]
        [Route("AdminListen/Edit/{id}")]
        public async Task<IActionResult> Edit()
        {
            return View(); 
        }

        [HttpPost]
        [Route("AdminListen/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, List<IFormFile> audiosFile, string listenName, string selectedLevel, string selectedPart)
        {
            try
            {
                var listen = await _dbcontext.Listenings.FindAsync(id);

                int level = int.Parse(selectedLevel);
                int part = int.Parse(selectedPart);
                listen.Level = level;
                listen.Part = part;            
                listen.ListeningName = listenName;
                _dbcontext.Listenings.Update(listen);
                await _dbcontext.SaveChangesAsync();

                if (listen == null)
                {
                    return View("NotFound");
                }

                if (imageFile != null && imageFile.Length > 0)
                {

                  /*  // Xóa ảnh cũ
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen", listen.Photo);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }*/

                    // Lưu tệp ảnh mới vào thư mục trên máy chủ
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Cập nhật thông tin bài nghe
                    
                    listen.Photo = fileName;
                   

                    _dbcontext.Listenings.Update(listen);
                    await _dbcontext.SaveChangesAsync();
                }

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Xóa câu hỏi cũ
                    var oldQuestions = _dbcontext.ListeningQuestions.Where(q => q.ListeningId == id);
                    _dbcontext.ListeningQuestions.RemoveRange(oldQuestions);
                    await _dbcontext.SaveChangesAsync();

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
                                    var question = worksheet.Cells[row, 1].Value?.ToString(); ;
                                    var correctanswer = worksheet.Cells[row, 2].Value?.ToString();
                                    var answer1 = worksheet.Cells[row, 3].Value?.ToString();
                                    var answer2 = worksheet.Cells[row, 4].Value?.ToString();
                                    var answer3 = worksheet.Cells[row, 5].Value?.ToString();
                                    var answer4 = worksheet.Cells[row, 6].Value?.ToString();
                                    var explain = worksheet.Cells[row, 7].Value?.ToString();
                                    var photo = worksheet.Cells[row, 8].Value?.ToString();
                                    var order = worksheet.Cells[row, 9].Value;
                                    var audioL = worksheet.Cells[row, 10].Value?.ToString();

                                    int numberValue = 0;
                                    if (order is double || order is float)
                                    {
                                        numberValue = Convert.ToInt32(order);
                                    }
                                    else if (order is string)
                                    {
                                        if (int.TryParse((string)order, out int parsedNumber))
                                        {
                                            numberValue = parsedNumber;
                                        }

                                    }

                                    var listeningQuestion = new ListeningQuestion()
                                    {
                                        Question = question,
                                        CorrectAnswer = correctanswer,
                                        Answer1 = answer1,
                                        Answer2 = answer2,
                                        Answer3 = answer3,
                                        Answer4 = answer4,
                                        Explain = explain,
                                        Photo = photo,
                                        Order = numberValue,
                                        AudioL = audioL,
                                        ListeningId = id
                                    };
                                    _dbcontext.ListeningQuestions.Add(listeningQuestion);
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

                // Xóa ảnh cũ trong danh sách ảnh
                /*var oldImages = _dbcontext.ListeningImages.Where(i => i.ListeningId == listenId);
                foreach (var oldImage in oldImages)
                {
                    string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen", oldImage.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                _dbcontext.ListeningImages.RemoveRange(oldImages);*/

                if (imagesFile.Count > 0)
                {
                    try
                    {
                        foreach (var image in imagesFile)
                        {
                            var fileNames = Path.GetFileName(image.FileName);
                            string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageListen");
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
                /*// Xóa file audio cũ trong danh sách audio
                var oldAudios = _dbcontext.ListeningAudios.Where(a => a.ListeningId == listenId);
                foreach (var oldAudio in oldAudios)
                {
                    string oldAudioPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioListen", oldAudio.Audio);
                    if (File.Exists(oldAudioPath))
                    {
                        File.Delete(oldAudioPath);
                    }
                }
                _dbcontext.ListeningAudios.RemoveRange(oldAudios);*/

                if (audiosFile.Count > 0)
                {
                    try
                    {
                        foreach (var audio in audiosFile)
                        {
                            var audioName = Path.GetFileName(audio.FileName);
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioListen");
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
                TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                return View("Edit");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện hành động !";
                return View("Edit");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("AdminListen/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int listenId = id;

            var listen = await _dbcontext.Listenings
                            .Include(v => v.ListeningQuestions)
                            .FirstOrDefaultAsync(v => v.Id == listenId);

            if (listen != null)
            {
                _dbcontext.Listenings.Remove(listen);

                // Xoá các câu hỏi có cùng Id với bản ghi Vocabulary trong bảng VocabularyContents
                var questions = _dbcontext.ListeningQuestions
                    .Where(vc => vc.ListeningId == listenId);

                _dbcontext.ListeningQuestions.RemoveRange(questions);

                await _dbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }

}
