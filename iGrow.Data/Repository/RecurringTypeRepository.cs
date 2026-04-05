namespace iGrow.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;

    public class RecurringTypeRepository : BaseRepository, IRecurringTypeRepository
    {
        public RecurringTypeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<RecurringType>> GetAllRecurringTypesNoTrackingAsync()
        {
            return await DbContext.RecurringTypes.AsNoTracking().ToArrayAsync();
        }

        public async Task<RecurringType?> GetRecurringTypeByIdAsync(int id)
        {
            return await DbContext.RecurringTypes.FindAsync(id);
        }

        public async Task<bool> AddRecurringTypeAsync(RecurringType rt)
        {
            await DbContext.RecurringTypes.AddAsync(rt);

            int r = await SaveChangesAsync();

            return r == 1;
        }

        public async Task<bool> DeleteRecurringTypeAsync(RecurringType rt)
        {
            DbContext.RecurringTypes.Remove(rt);

            int r = await SaveChangesAsync();

            return r == 1;
        }

        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await DbContext.RecurringTypes.AnyAsync(rt => rt.Name.ToLower() == name.ToLower());
        }
    }
}