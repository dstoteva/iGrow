namespace iGrow.Services.Contracts
{
    using iGrow.Web.ViewModels;
    public interface IAmountService
    {
        Task<IEnumerable<SelectAmountId>> GetAllAmountsAsync();
    }
}
