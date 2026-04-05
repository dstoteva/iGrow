namespace iGrow.Data.Repository.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using iGrow.Data.Models;
    using Microsoft.AspNetCore.Http;

    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesNoTrackingAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<bool> AddCategoryAsync(Category category, CancellationToken? cancellationToken);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}