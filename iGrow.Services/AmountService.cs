namespace iGrow.Services
{
    using iGrow.Data;
    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
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

        public async Task AddAmountAsync(string name)
        {
            Amount amount = new Amount { Name = name };

            bool success = await _amountRepository.AddAmountAsync(amount);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<SelectAmountId?> GetAmountByIdAsync(int id)
        {
            Amount? a = await _amountRepository.GetAmountByIdAsync(id);

            if (a != null) 
            {
                return new SelectAmountId { Id = a.Id, Name = a.Name };
            }
            else
            {
                throw new EntityNotFoundException();
            }
            
        }

        public async Task DeleteAmountAsync(int id)
        {
            Amount? a = await _amountRepository.GetAmountByIdAsync(id);

            if (a == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await _amountRepository.DeleteAmountAsync(a);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await _amountRepository.ItemExistsByNameAsync(name);
        }
    }
}
