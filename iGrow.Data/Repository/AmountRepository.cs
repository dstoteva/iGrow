namespace iGrow.Data.Repository
{
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
            return await DbContext!.Amounts
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}
