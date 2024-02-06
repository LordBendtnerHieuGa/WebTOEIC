using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Services
{
    public class ReadService
    {
        private readonly ReadRepository _readRepository;
        public ReadService(ReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<Reading> GetSingleById(int id)
        {
            var read = await _readRepository.GetSingleById(id);
            return read;
        }

        public async Task<PaginatedList<Reading>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            static bool IsNumeric(string input, out int search)
            {
                return int.TryParse(input, out search);
            }

            var query = await _readRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {

                if (IsNumeric(searchString, out int search) == true)
                {
                    query = query.Where(g => g.Id == search);
                }
                else
                {
                    query = query.Where(g => g.ReadingsName.Contains(searchString));
                }
            }

            return await PaginatedList<Reading>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Reading> Add(Reading read)
        {
            return await _readRepository.Add(read);
        }

        public async Task<Reading> Update(Reading read)
        {
            return await _readRepository.Update(read);
        }

        public async Task Delete(int id)
        {
            await _readRepository.Delete(id);
        }

    }
}

