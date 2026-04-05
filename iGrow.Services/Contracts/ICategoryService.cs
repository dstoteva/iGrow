namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;
    using Microsoft.AspNetCore.Http;

    public interface ICategoryService
    {
        Task<IEnumerable<SelectCategoryId>> GetAllCategoriesAsync();
        Task AddCategoryAsync(string name, IFormFile? file);
        Task<SelectCategoryId?> GetCategoryByIdAsync(int id);
        Task DeleteCategoryAsync(int id);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}