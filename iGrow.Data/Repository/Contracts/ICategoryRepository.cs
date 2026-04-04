namespace iGrow.Data.Repository.Contracts
{
    using iGrow.Data.Models;
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesNoTrackingAsync();
    }
}
