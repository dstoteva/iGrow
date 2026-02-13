namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;
    public interface IRecurringTypeService
    {
        Task<IEnumerable<SelectRecurringTypeId>> GetAllRecurringTypesAsync();
    }
}
