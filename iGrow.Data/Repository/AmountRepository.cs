namespace iGrow.Data.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using iGrow.Data.Models;
    using iGrow.Data.Repository.Contracts;

    public class AmountRepository : BaseRepository, IAmountRepository
    {
        public AmountRepository(ApplicationDbContext dbContext)
            :base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Amount>> GetAllAmountsNoTrackingAsync()
        {
            return await DbContext.Amounts.AsNoTracking().ToArrayAsync();
        }

        public async Task<Amount?> GetAmountByIdAsync(int id)
        {
            return await DbContext.Amounts.FindAsync(id);
        }

        public async Task<bool> AddAmountAsync(Amount amount)
        {
            await DbContext.Amounts.AddAsync(amount);

            int r = await SaveChangesAsync();

            return r == 1;
        }

        public async Task<bool> DeleteAmountAsync(Amount amount)
        {
            DbContext.Amounts.Remove(amount);

            int r = await SaveChangesAsync();

            return r == 1;
        }
        public async Task<bool> ItemExistsByNameAsync(string name)
        {
            return await DbContext.Amounts.AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }
    }
}
