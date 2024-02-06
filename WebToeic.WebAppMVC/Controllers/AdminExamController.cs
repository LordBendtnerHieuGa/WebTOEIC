using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PagedList;
using System.IO;
using Utilities.DataTypes.ExtensionMethods;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Services;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminExamController : Controller
    {
        private readonly ExamService _examService;
        private readonly WebToeicDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminExamController(ExamService examService, WebToeicDbContext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _examService = examService;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("AdminExam/{page?}")]
        public async Task<IActionResult> Index(string nameExam = "", string orderBy = "", int currentPage = 1)
        {

            nameExam = string.IsNullOrEmpty(nameExam) ? "" : nameExam.ToLower();
            var examData = new ExamVM();
            examData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            examData.OrderByID = orderBy == "id" ? "id_desc" : "id";

            var tests = await _dbcontext.Tests
                                .Where(emp => nameExam == "" || emp.TestName.ToLower().StartsWith(nameExam))
                                .ToListAsync();

            switch (orderBy)
            {
                case "name_desc":
                    tests = tests.OrderByDescending(emp => emp.TestName).ToList();
                    break;
                case "id_desc":
                    tests = tests.OrderByDescending(emp => emp.Id).ToList();
                    break;
                case "id":
                    tests = tests.OrderBy(emp => emp.Id).ToList();
                    break;
                default:
                    tests = tests.OrderBy(emp => emp.TestName).ToList();
                    break;
            }
            int totalRecords = tests.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            tests = tests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            examData.TotalRecord = totalRecords;
            examData.Tests = tests;
            examData.CurrentPage = currentPage;
            examData.TotalPages = totalPages;
            examData.PageSize = pageSize;
            examData.NameExam = nameExam;
            examData.OrderBy = orderBy;
            return View(examData);
        }


        [HttpGet]
        [Route("AdminExam/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminExam/Create")]
        public async Task<IActionResult> Create(IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, List<IFormFile> audiosFile, string examName)
        {
            try
            {              

                if (imageFile != null && imageFile.Length > 0)
                {

                    // Lưu tệp ảnh vào thư mục trên máy chủ                   
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageExam");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    var exam = new Test
                    {                      
                        ImageT = fileName,
                        TestName = examName
                    };

                    _dbcontext.Tests.Add(exam);
                    await _dbcontext.SaveChangesAsync();

                    var testId = exam.Id;

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
                                        var correctanswer = worksheet.Cells[row, 2].Value?.ToString();
                                        var imageTQ = worksheet.Cells[row, 3].Value?.ToString();
                                        var number = worksheet.Cells[row, 4].Value;
                                        var option1 = worksheet.Cells[row, 5].Value?.ToString();
                                        var option2 = worksheet.Cells[row, 6].Value?.ToString();
                                        var option3 = worksheet.Cells[row, 7].Value?.ToString();
                                        var option4 = worksheet.Cells[row, 8].Value?.ToString();
                                        var paragraph = worksheet.Cells[row, 9].Value?.ToString();
                                        var question = worksheet.Cells[row, 10].Value?.ToString();

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

                                        var testQuestion = new TestQuestion()
                                        {

                                            AudioMp3 = audioMp3,
                                            CorrectAnswer = correctanswer,
                                            ImageTQ = imageTQ,
                                            Number = numberValue,
                                            Option1 = option1,
                                            Option2 = option2,
                                            Option3 = option3,
                                            Option4 = option4,
                                            Paragraph = paragraph,
                                            Question = question,
                                            TestId = testId,
                                            UserAnswer = null
                                        };
                                        _dbcontext.TestQuestions.Add(testQuestion);
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
                            string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageExam");
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
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioExam");
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
                TempData["SuccessMessage"] = "Tạo mới thành công!";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện !";
                return View("Create");
            }

        }

        [HttpGet]
        [Route("AdminExam/Edit/{id}")]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("AdminExam/Edit/{id}")]
        public async Task<IActionResult> Edit(int id, IFormFile imageFile, IFormFile excelFile, List<IFormFile> imagesFile, List<IFormFile> audiosFile, string examName)
        {
            try
            {
                var exam = await _dbcontext.Tests.FindAsync(id);
                if (exam == null)
                {
                    return RedirectToAction("Index");
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Xóa tệp ảnh hiện tại nếu có
                    if (!string.IsNullOrEmpty(exam.ImageT))
                    {
                        string currentImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageExam", exam.ImageT);
                        if (System.IO.File.Exists(currentImagePath))
                        {
                            System.IO.File.Delete(currentImagePath);
                        }
                    }

                    // Lưu tệp ảnh mới vào thư mục trên máy chủ
                    var fileName = Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageExam");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Cập nhật thông tin bài thi với ảnh mới
                    exam.ImageT = fileName;
                    exam.TestName = examName;
                }
                else
                {
                    // Nếu không có tệp ảnh mới, chỉ cập nhật tên bài thi
                    exam.TestName = examName;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _dbcontext.Tests.Update(exam);
                await _dbcontext.SaveChangesAsync();

                if (excelFile != null && excelFile.Length > 0)
                {
                    // Xóa các câu hỏi hiện tại của bài thi
                    var existingQuestions = _dbcontext.TestQuestions.Where(q => q.TestId == id);
                    _dbcontext.TestQuestions.RemoveRange(existingQuestions);
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
                                    var audioMp3 = worksheet.Cells[row, 1].Value?.ToString(); ;
                                    var correctanswer = worksheet.Cells[row, 2].Value?.ToString();
                                    var imageTQ = worksheet.Cells[row, 3].Value?.ToString();
                                    var number = worksheet.Cells[row, 4].Value;
                                    var option1 = worksheet.Cells[row, 5].Value?.ToString();
                                    var option2 = worksheet.Cells[row, 6].Value?.ToString();
                                    var option3 = worksheet.Cells[row, 7].Value?.ToString();
                                    var option4 = worksheet.Cells[row, 8].Value?.ToString();
                                    var paragraph = worksheet.Cells[row, 9].Value?.ToString();
                                    var question = worksheet.Cells[row, 10].Value?.ToString();

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

                                    var testQuestion = new TestQuestion()
                                    {
                                        AudioMp3 = audioMp3,
                                        CorrectAnswer = correctanswer,
                                        ImageTQ = imageTQ,
                                        Number = numberValue,
                                        Option1 = option1,
                                        Option2 = option2,
                                        Option3 = option3,
                                        Option4 = option4,
                                        Paragraph = paragraph,
                                        Question = question,
                                        TestId = id,
                                        UserAnswer = null
                                    };
                                    _dbcontext.TestQuestions.Add(testQuestion);
                                }

                                await _dbcontext.SaveChangesAsync();
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

                // Xử lý tệp ảnh mới
                if (imagesFile.Count > 0)
                {
                    try
                    {
                        foreach (var image in imagesFile)
                        {
                            var fileNames = Path.GetFileName(image.FileName);
                            string uploadsFolders = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsImageExam");
                            string filePaths = Path.Combine(uploadsFolders, fileNames);
                            using (var fileStreams = new FileStream(filePaths, FileMode.Create))
                            {
                                await image.CopyToAsync(fileStreams);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return View("NotFound");
                    }
                }

                // Xử lý tệp âm thanh mới
                if (audiosFile.Count > 0)
                {
                    try
                    {
                        foreach (var audio in audiosFile)
                        {
                            var audioName = Path.GetFileName(audio.FileName);
                            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploadsAudioExam");
                            string filePaths = Path.Combine(uploadsFolder, audioName);
                            using (var fileStream = new FileStream(filePaths, FileMode.Create))
                            {
                                await audio.CopyToAsync(fileStream);
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
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi thực hiện !";
                return View("Edit");
            }
        }

        [HttpPost]
        [Route("AdminExam/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int examId = id;

            var exam = await _dbcontext.Tests
                            .Include(v => v.TestQuestions)
                            .FirstOrDefaultAsync(v => v.Id == examId);

            if (exam != null)
            {
                _dbcontext.Tests.Remove(exam);

                // Xoá các câu hỏi có cùng Id với bản ghi Vocabulary trong bảng VocabularyContents
                var questions = _dbcontext.TestQuestions
                    .Where(vc => vc.TestId == examId);

                _dbcontext.TestQuestions.RemoveRange(questions);

                await _dbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
