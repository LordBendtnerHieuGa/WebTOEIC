using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Repositories
{
    public class GrammarRepository : RepositoryBase<Grammar>
    {
        public GrammarRepository(WebToeicDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Grammar> GetSingleById(int id)
        {
            return await _context.Set<Grammar>()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

       /* public async Task<IQueryable<Grammar>> GetAll()
        {
            return _context.Set<Grammar>().AsQueryable();
        }*/
    }
}
