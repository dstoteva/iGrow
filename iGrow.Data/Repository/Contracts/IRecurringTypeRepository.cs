namespace iGrow.Data.Repository.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using iGrow.Data.Models;

    public interface IRecurringTypeRepository
    {
        Task<IEnumerable<RecurringType>> GetAllRecurringTypesNoTrackingAsync();
        Task<RecurringType?> GetRecurringTypeByIdAsync(int id);
        Task<bool> AddRecurringTypeAsync(RecurringType rt);
        Task<bool> DeleteRecurringTypeAsync(RecurringType rt);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}