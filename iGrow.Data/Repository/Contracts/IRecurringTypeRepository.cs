namespace iGrow.Data.Repository.Contracts
{
    using iGrow.Data.Models;
    public interface IRecurringTypeRepository
    {
        Task<IEnumerable<RecurringType>> GetAllRecurringTypesNoTrackingAsync();
    }
}
