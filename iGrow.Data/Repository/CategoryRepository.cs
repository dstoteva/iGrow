namespace iGrow.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using Microsoft.AspNetCore.Http;

    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesNoTrackingAsync()
        {
            return await DbContext.Categories.AsNoTracking().ToArrayAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await DbContext.Categories.FindAsync(id);
        }

        public async Task<bool> AddCategoryAsync(Category category, CancellationToken? cancellationToken)
        {
            if(cancellationToken != null)
            {
                await DbContext.Categories.AddAsync(category, (CancellationToken)cancellationToken);
            }
            else
            {
                await DbContext.Categories.AddAsync(category);
            }


            int r = await SaveChangesAsync();

            return r == 1;
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            DbContext.Categories.Remove(category);

            int r = await SaveChangesAsync();

            return r == 1;
        }

        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await DbContext.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}
