namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<SelectCategoryId>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await this._categoryRepository
                .GetAllCategoriesNoTrackingAsync();

            IEnumerable<SelectCategoryId> projected = categories
                .Select(c => new SelectCategoryId
                {
                    Id = c.Id,
                    Name = c.Name
                });

            return projected;
        }
    }
}
