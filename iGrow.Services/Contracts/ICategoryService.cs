namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;

    public interface ICategoryService
    {
        Task<IEnumerable<SelectCategoryId>> GetAllCategoriesAsync();
        Task AddCategoryAsync(string name);
        Task<SelectCategoryId?> GetCategoryByIdAsync(int id);
        Task DeleteCategoryAsync(int id);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}