using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Repositories
{
    public class ListenRepository : RepositoryBase<Listening>
    {
        public ListenRepository(WebToeicDbContext context) : base(context)
        {

        }

        public async Task<Listening> GetSingleById(int id)
        {
            return await _context.Set<Listening>()
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
