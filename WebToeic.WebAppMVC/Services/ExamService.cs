using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Services
{
    public class ExamService
    {
        private readonly ExamRepository _examRepository;
        public ExamService(ExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<Test> GetSingleById(int id)
        {
            var exam = await _examRepository.GetSingleById(id);
            return exam;
        }

        public async Task<PaginatedList<Test>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            static bool IsNumeric(string input, out int search)
            {
                return int.TryParse(input, out search);
            }

            var query = await _examRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {

                if (IsNumeric(searchString, out int search) == true)
                {
                    query = query.Where(g => g.Id == search);
                }
                else
                {
                    query = query.Where(g => g.TestName.Contains(searchString));
                }
            }

            return await PaginatedList<Test>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Test> Add(Test exam)
        {
            return await _examRepository.Add(exam);
        }

        public async Task<Test> Update(Test exam)
        {
            return await _examRepository.Update(exam);
        }

        public async Task Delete(int id)
        {
            await _examRepository.Delete(id);
        }

    }
}

