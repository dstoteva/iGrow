namespace iGrow.Services
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;
    using iGrow.Data.Repository.Contracts;
    using iGrow.Data.Models;

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
    }
}
