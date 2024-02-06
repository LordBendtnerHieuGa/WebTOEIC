using Microsoft.EntityFrameworkCore;
using WebToeic.WebAppMVC.Data.EFCore;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Repositories
{
    public class ExamRepository : RepositoryBase<Test>
    {
        public ExamRepository(WebToeicDbContext context) : base(context) 
        {

        }
        public async Task<Test> GetSingleById(int id)
        {
            return await _context.Set<Test>()
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
