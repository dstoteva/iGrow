namespace iGrow.Services
{
    using System.IO;

    using Microsoft.AspNetCore.Http;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private static readonly string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private static readonly string[] AllowedExt = { ".png", ".jpg", ".jpeg", ".gif" };
        private const long MaxFileBytes = 2 * 1024 * 1024;

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
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                });

            return projected;
        }

        public async Task AddCategoryAsync(string name, IFormFile? file)
        {
            Category category = new Category { Name = name };
            bool success = false;

            if (file != null && file.Length > 0)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExt.Contains(ext))
                {
                    throw new InvalidOperationException("Invalid image extension.");
                }

                if (file.Length > MaxFileBytes)
                {
                    throw new InvalidOperationException("Image too large.");
                }

                var uploads = Path.Combine(wwwRootPath, "images", "categories");
                Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, fileName);

                await using (var fs = File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                }

                category.ImageUrl = $"/images/categories/{fileName}";

                success = await _categoryRepository.AddCategoryAsync(category);
            }
            else
            {
                success = await _categoryRepository.AddCategoryAsync(category);
            }

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
                return new SelectCategoryId { Id = c.Id, Name = c.Name, ImageUrl = c.ImageUrl};
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
