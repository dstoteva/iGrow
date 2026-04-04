namespace iGrow.Data.Repository
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;

    public class RecurringTypeRepository : BaseRepository, IRecurringTypeRepository
    {
        public RecurringTypeRepository(ApplicationDbContext dbContext)
            :base(dbContext)
        {
        }

        public async Task<IEnumerable<RecurringType>> GetAllRecurringTypesNoTrackingAsync()
        {
            return await DbContext!.RecurringTypes
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
