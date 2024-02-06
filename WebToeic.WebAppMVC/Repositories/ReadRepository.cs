using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Repositories
{
    public class ReadRepository : RepositoryBase<Reading>
    {
        public ReadRepository(WebToeicDbContext context) : base(context)
        {
        }

        public async Task<Reading> GetSingleById(int id)
        {
            return await _context.Set<Reading>()
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
