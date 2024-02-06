using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.ViewModels;


namespace WebToeic.WebAppMVC.Services
{
    public class GrammarService
    {
        private readonly GrammarRepository _grammarRepository;
        public GrammarService(GrammarRepository grammarRepository)
        {
            _grammarRepository = grammarRepository;
        }


        public async Task<Grammar> GetSingleById(int id)
        {
            var grammar = await _grammarRepository.GetSingleById(id);
            return grammar;
        }

        public async Task<PaginatedList<Grammar>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            static bool IsNumeric(string input, out int search)
            {
                return int.TryParse(input, out search);
            }

            var query = await _grammarRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {

                if (IsNumeric(searchString, out int search) == true)
                {
                    query = query.Where(g => g.Id == search);
                }
                else
                {
                    query = query.Where(g => g.GrammarName.Contains(searchString));
                }
            }

            return await PaginatedList<Grammar>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Grammar> Add(Grammar grammar)
        {
            return await _grammarRepository.Add(grammar);
        }

        public async Task<Grammar> Update(Grammar grammar)
        {
            return await _grammarRepository.Update(grammar);
        }

        public async Task Delete(int id)
        {
            await _grammarRepository.Delete(id);
        }

    }
}
