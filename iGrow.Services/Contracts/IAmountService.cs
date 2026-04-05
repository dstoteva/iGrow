namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;
    public interface IAmountService
    {
        Task<IEnumerable<SelectAmountId>> GetAllAmountsAsync();
        Task AddAmountAsync(string name);
        Task<SelectAmountId?> GetAmountByIdAsync(int id);
        Task DeleteAmountAsync(int id);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}