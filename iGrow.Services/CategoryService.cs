namespace iGrow.Services
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

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

        public async Task AddCategoryAsync(string name)
        {
            Category category = new Category { Name = name };

            bool success = await _categoryRepository.AddCategoryAsync(category);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<SelectCategoryId?> GetCategoryByIdAsync(int id)
        {
            Category? c = await _categoryRepository.GetCategoryByIdAsync(id);

            if (c != null)
            {
                return new SelectCategoryId { Id = c.Id, Name = c.Name };
            }
            else
            {
                throw new EntityNotFoundException();
            }            
        }

        public async Task DeleteCategoryAsync(int id)
        {
            Category? c = await _categoryRepository.GetCategoryByIdAsync(id);

            if (c == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await _categoryRepository.DeleteCategoryAsync(c);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await _categoryRepository.ItemExistsByNameAsync(name);
        }
    }
}
