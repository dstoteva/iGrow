namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;
    public interface IRecurringTypeService
    {
        Task<IEnumerable<SelectRecurringTypeId>> GetAllRecurringTypesAsync();
        Task AddRecurringTypeAsync(string name);
        Task<SelectRecurringTypeId?> GetRecurringTypeByIdAsync(int id);
        Task DeleteRecurringTypeAsync(int id);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}
