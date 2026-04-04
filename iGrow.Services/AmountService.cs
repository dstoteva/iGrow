namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class AmountService : IAmountService
    {
        private readonly IAmountRepository _amountRepository;
        public AmountService(IAmountRepository amountRepository)
        {
            this._amountRepository = amountRepository;
        }

        public async Task<IEnumerable<SelectAmountId>> GetAllAmountsAsync()
        {
            IEnumerable<Amount> amounts = await this._amountRepository
                .GetAllAmountsNoTrackingAsync();

            IEnumerable<SelectAmountId> projected = amounts
                .Select(c => new SelectAmountId
                {
                    Id = c.Id,
                    Name = c.Name
                });

            return projected;
        }
    }
}
