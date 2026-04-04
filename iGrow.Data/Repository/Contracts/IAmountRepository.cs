namespace iGrow.Data.Repository.Contracts
{
    using iGrow.Data.Models;
    public interface IAmountRepository
    {
        Task<IEnumerable<Amount>> GetAllAmountsNoTrackingAsync();
    }
}
