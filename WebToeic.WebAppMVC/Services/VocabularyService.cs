using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;
using WebToeic.WebAppMVC.Repositories;
using WebToeic.WebAppMVC.ViewModels;

namespace WebToeic.WebAppMVC.Services
{
    public class VocabularyService
    {
        private readonly VocabularyRepository _vocabularyRepository;
        private readonly WebToeicDbContext _dbcontext;
        public VocabularyService(VocabularyRepository vocabularyRepository,WebToeicDbContext dbContext)
        {
            _vocabularyRepository = vocabularyRepository;
            _dbcontext = dbContext;
        }

        public async Task<Vocabulary> GetSingleById(int id)
        {
            var vocabulary = await _vocabularyRepository.GetSingleById(id);
            return vocabulary;
        }

        public async Task<PaginatedList<Vocabulary>> GetPaginatedList(string? searchString = null, int pageIndex = 1, int pageSize = 5)
        {
            static bool IsNumeric(string input, out int search)
            {
                return int.TryParse(input, out search);
            }

            var query = await _vocabularyRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {

                if (IsNumeric(searchString, out int search) == true)
                {
                    query = query.Where(g => g.Id == search);
                }
                else
                {
                    query = query.Where(g => g.VocabularyName.Contains(searchString));
                }
            }

            return await PaginatedList<Vocabulary>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Vocabulary> Add(Vocabulary read)
        {
            return await _vocabularyRepository.Add(read);
        }

        public async Task<Vocabulary> Update(Vocabulary read)
        {
            return await _vocabularyRepository.Update(read);
        }

        public async Task Delete(int id)
        {
            await _vocabularyRepository.Delete(id);
        }

        /*public List<Vocabulary> GetVoacbularies(string nameVC, string? sortOrder)
        {
            var vocabularies = _dbcontext.Vocabularies.ToListAsync();
               

            if (!string.IsNullOrEmpty(nameVC))
            {
                vocabularies = vocabularies.Where(vc => vc.VocabularyName.ToLower().StartsWith(nameVC));
                
            }

            if (categoryId != null)
            {
                articles = articles.Where(a => a.CategoryId == categoryId);
            }

            if (authorId != null)
            {
                articles = articles.Where(a => a.AuthorId == authorId);
            }

            switch (sortOrder)
            {
                case "date":
                    articles = articles.OrderByDescending(a => a.ArticleDate);
                    break;
                case "title":
                    articles = articles.OrderBy(a => a.ArticleTitle);
                    break;
                case "date_desc":
                    articles = articles.OrderBy(a => a.ArticleDate);
                    break;
                case "title_desc":
                    articles = articles.OrderByDescending(a => a.ArticleTitle);
                    break;
                default:
                    articles = articles.OrderByDescending(a => a.ArticleDate);
                    break;
            }

            var result = articles.ToList();

            return result;
        }*/


    }


}

