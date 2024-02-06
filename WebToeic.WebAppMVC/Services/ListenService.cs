using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Services
{
    public class ListenService
    {
        private readonly ListenRepository _listenRepository;
        public ListenService(ListenRepository listenRepository)
        {
            _listenRepository = listenRepository;
        }


        public async Task<Listening> GetSingleById(int id)
        {
            var listen = await _listenRepository.GetSingleById(id);
            return listen;
        }

        public async Task<PaginatedList<Listening>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            static bool IsNumeric(string input, out int search)
            {
                return int.TryParse(input, out search);
            }

            var query = await _listenRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {

                if (IsNumeric(searchString, out int search) == true)
                {
                    query = query.Where(g => g.Id == search);
                }
                else
                {
                    query = query.Where(g => g.ListeningName.Contains(searchString));
                }
            }

            return await PaginatedList<Listening>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Listening> Add(Listening listen)
        {
            return await _listenRepository.Add(listen);
        }

        public async Task<Listening> Update(Listening listen)
        {
            return await _listenRepository.Update(listen);
        }

        public async Task Delete(int id)
        {
            await _listenRepository.Delete(id);
        }

    }

}
    

