namespace iGrow.Data.Repository
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;

    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
                
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesNoTrackingAsync()
        {
            return await DbContext!.Categories
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
