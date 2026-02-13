namespace iGrow.Services
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

    public class RecurringTypeService : IRecurringTypeService
    {
        private readonly ApplicationDbContext _dbContext;

        public RecurringTypeService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IEnumerable<SelectRecurringTypeId>> GetAllRecurringTypesAsync()
        {
            return await this._dbContext.RecurringTypes
                .Select(rt => new SelectRecurringTypeId
                {
                    Id = rt.Id,
                    Name = rt.Name
                })
                .ToListAsync();
        }
    }
}
