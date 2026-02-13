namespace iGrow.Services
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IEnumerable<SelectCategoryId>> GetAllCategoriesAsync()
        {
            return await this._dbContext.Categories
                .Select(c => new SelectCategoryId
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
