using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminReadController : Controller
    {
        private readonly ReadService _readService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AdminReadController(ReadService readService, WebToeicDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _readService = readService;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("AdminRead/{page?}")]
        public async Task<IActionResult> Index(string nameRead = "", string levelRead = "", string orderBy = "", string partRead = "", int currentPage = 1)
        {
            // Search
            nameRead = string.IsNullOrEmpty(nameRead) ? "" : nameRead.ToLower();
            var readData = new ReadVM();

            readData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "name";
            readData.OrderByID = orderBy == "id" ? "id_desc" : "id";
            readData.OrderByLevel = orderBy == "level" ? "level_desc" : "level";
            readData.OrderByPart = orderBy == "part" ? "part_desc" : "part";


            var reads = await _dbcontext.Readings
                        .Where(emp => (string.IsNullOrEmpty(nameRead) || emp.ReadingsName.ToLower().StartsWith(nameRead))
                                    && (string.IsNullOrEmpty(levelRead) || emp.Level == int.Parse(levelRead))
                                    && (string.IsNullOrEmpty(partRead) || emp.Part == int.Parse(partRead)))
                        .ToListAsync();
            // OrderBy
            switch (orderBy)
            {
                case "name_desc":
                    reads = reads.OrderByDescending(emp => emp.ReadingsName).ToList();
                    break;

                case "name":
                    reads = reads.OrderBy(emp => emp.ReadingsName).ToList();
                    break;

                case "id_desc":
                    reads = reads.OrderByDescending(emp => emp.Id).ToList();
                    break;

                case "id":
                    reads = reads.OrderBy(emp => emp.Id).ToList();
                    break;

                case "level_desc":
                    reads = reads.OrderByDescending(emp => emp.Level).ToList();
                    break;

                case "level":
                    reads = reads.OrderBy(emp => emp.Level).ToList();
                    break;

                case "part_desc":
                    reads = reads.OrderByDescending(emp => emp.Part).ToList();
                    break;

                case "part":
                    reads = reads.OrderBy(emp => emp.Part).ToList();
                    break;
            }

            // Pagination
            int totalRecords = reads.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            reads = reads.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            readData.Readings = reads;
            readData.CurrentPage = currentPage;
            readData.TotalPages = totalPages;
            readData.PageSize = pageSize;
            readData.NameRead = nameRead;
            readData.OrderBy = orderBy;
            readData.TotalRecord = totalRecords;

            return View(readData);
        }

        [HttpGet]
        [Route("AdminRead/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminRead/Create")]
        public async Task<IActionResult> Create(IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, string readName, string selectedLevel, string selectedPart)
        {
            try
            {

                if (imageFile != null && imageFile.Length > 0 && readName != null && selectedLevel != null && selectedPart != null)
                {

                    int level = int.Parse(selectedLevel);
                    int part = int.Parse(selectedPart);

                    // Lưu tệp ảnh vào thư mục trên máy chủ                   
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead");
                    string filePath = Path.Combine(uploadsFolder, fileName);
         
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    var reading = new Reading
                    {
                        Level = level,
                        Part = part,
                        Photo = fileName,
                        ReadingsName = readName
                    };

                    _dbcontext.Readings.Add(reading);
                    await _dbcontext.SaveChangesAsync();

                    var readId = reading.Id;

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

                                        var readingQuestion = new ReadingQuestion()
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
                                            ReadingId = readId
                                        };
                                        _dbcontext.ReadingQuestions.Add(readingQuestion);
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
                    if (imagesFile.Count > 0)
                    {
                        try
                        {
                            foreach (var image in imagesFile)
                            {
                                var fileNames = Path.GetFileName(image.FileName);
                                string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead");
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
        [Route("AdminRead/Edit/{id}")]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminRead/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, string readName, string selectedLevel, string selectedPart)
        {
            try
            {
                // Tìm bài đọc hiểu theo id
                var reading = await _dbcontext.Readings.FindAsync(id);
                int level = int.Parse(selectedLevel);
                int part = int.Parse(selectedPart);
                reading.Level = level;
                reading.Part = part;
                
                reading.ReadingsName = readName;

                _dbcontext.Readings.Update(reading);
                await _dbcontext.SaveChangesAsync();

                if (reading != null)
                {
                    if (imageFile != null && imageFile.Length > 0 && readName != null && selectedLevel != null && selectedPart != null)
                    {


                        // Xóa tệp ảnh cũ
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead", reading.Photo);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                        // Lưu tệp ảnh mới vào thư mục trên máy chủ
                        var fileName = Path.GetFileName(imageFile.FileName);
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead");
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        reading.Photo = fileName;
                        _dbcontext.Readings.Update(reading);
                        await _dbcontext.SaveChangesAsync();
                    }

                    if (excelFile != null && excelFile.Length > 0)
                    {
                        // Xóa các câu hỏi cũ của bài đọc hiểu
                        var oldQuestions = _dbcontext.ReadingQuestions.Where(q => q.ReadingId == reading.Id);
                        _dbcontext.ReadingQuestions.RemoveRange(oldQuestions);
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
                                        // Lấy thông tin câu hỏi từ tệp Excel
                                        var question = worksheet.Cells[row, 1].Value?.ToString(); ;
                                        var correctanswer = worksheet.Cells[row, 2].Value?.ToString();
                                        var answer1 = worksheet.Cells[row, 3].Value?.ToString();
                                        var answer2 = worksheet.Cells[row, 4].Value?.ToString();
                                        var answer3 = worksheet.Cells[row, 5].Value?.ToString();
                                        var answer4 = worksheet.Cells[row, 6].Value?.ToString();
                                        var explain = worksheet.Cells[row, 7].Value?.ToString();
                                        var photo = worksheet.Cells[row, 8].Value?.ToString();
                                        var order = worksheet.Cells[row, 9].Value;

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

                                        var readingQuestion = new ReadingQuestion()
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
                                            ReadingId = reading.Id
                                        };
                                        _dbcontext.ReadingQuestions.Add(readingQuestion);
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

                    if (imagesFile.Count > 0)
                    {
                        try
                        {
                            // Xóa tệp ảnh cũ
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead", reading.Photo);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                            // Lưu các tệp ảnh mới vào thư mục trên máy chủ
                            foreach (var image in imagesFile)
                            {
                                var fileNames = Path.GetFileName(image.FileName);
                                string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageRead");
                                string filePaths = Path.Combine(uploadsFolders, fileNames);
                                using (var fileStreams = new FileStream(filePaths, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStreams);
                                }
                                // Cập nhật thông tin ảnh trong cơ sở dữ liệu
                              
                            }
                        }
                        catch (Exception ex)
                        {
                            return View("NotFound");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Index");
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
        [Route("AdminRead/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int readId = id;

            var read = await _dbcontext.Readings
                            .Include(v => v.ReadingQuestions)
                            .FirstOrDefaultAsync(v => v.Id == readId);

            if (read != null)
            {
                _dbcontext.Readings.Remove(read);

                // Xoá các câu hỏi có cùng Id với bản ghi Vocabulary trong bảng VocabularyContents
                var questions = _dbcontext.ReadingQuestions
                    .Where(vc => vc.ReadingId == readId);

                _dbcontext.ReadingQuestions.RemoveRange(questions);

                await _dbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
