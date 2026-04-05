namespace iGrow.Data.Repository.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using iGrow.Data.Models;

    public interface IAmountRepository
    {
        Task<IEnumerable<Amount>> GetAllAmountsNoTrackingAsync();
        Task<Amount?> GetAmountByIdAsync(int id);
        Task<bool> AddAmountAsync(Amount amount);
        Task<bool> DeleteAmountAsync(Amount amount);
        Task<bool> ItemExistsByNameAsync(string name);
    }
}
