namespace iGrow.Services
{
    using iGrow.Data.Models;
    using iGrow.Data.Repository;
    using iGrow.Data.Repository.Contracts;
    using iGrow.GCommon.Exceptions;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

    public class RecurringTypeService : IRecurringTypeService
    {
        private readonly IRecurringTypeRepository _recurringTypeRepository;

        public RecurringTypeService(IRecurringTypeRepository recurringTypeRepository)
        {
            this._recurringTypeRepository = recurringTypeRepository;
        }
        public async Task<IEnumerable<SelectRecurringTypeId>> GetAllRecurringTypesAsync()
        {
            IEnumerable<RecurringType> recurringTypes = await this._recurringTypeRepository
                .GetAllRecurringTypesNoTrackingAsync();

            IEnumerable<SelectRecurringTypeId> projected = recurringTypes
                .Select(rt => new SelectRecurringTypeId
                {
                    Id = rt.Id,
                    Name = rt.Name
                });

            return projected;
        }

        public async Task AddRecurringTypeAsync(string name)
        {
            RecurringType rt = new RecurringType { Name = name };

            bool success = await _recurringTypeRepository.AddRecurringTypeAsync(rt);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<SelectRecurringTypeId?> GetRecurringTypeByIdAsync(int id)
        {
            RecurringType? rt = await _recurringTypeRepository.GetRecurringTypeByIdAsync(id);

            if (rt != null)
            {
                return new SelectRecurringTypeId { Id = rt.Id, Name = rt.Name };
            }
            else
            {
                throw new EntityNotFoundException();
            }
        }

        public async Task DeleteRecurringTypeAsync(int id)
        {
            RecurringType? rt = await _recurringTypeRepository.GetRecurringTypeByIdAsync(id);

            if (rt == null)
            {
                throw new EntityNotFoundException();
            }

            bool success = await _recurringTypeRepository.DeleteRecurringTypeAsync(rt);

            if (!success)
            {
                throw new EntityPersistFailureException();
            }
        }

        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await _recurringTypeRepository.ItemExistsByNameAsync(name);
        }
    }
}
