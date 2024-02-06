using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Repositories
{
    public class VocabularyRepository : RepositoryBase<Vocabulary>
    {
        public VocabularyRepository(WebToeicDbContext context) : base(context)
        {
        }
        public async Task<Vocabulary> GetSingleById(int id)
        {
            return await _context.Set<Vocabulary>()
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
