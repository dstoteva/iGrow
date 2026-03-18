namespace iGrow.Services
{
    using Microsoft.EntityFrameworkCore;

    using iGrow.Data;
    using iGrow.Services.Contracts;
    using iGrow.Web.ViewModels;

    public class AmountService : IAmountService
    {
        private readonly ApplicationDbContext _dbContext;

        public AmountService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SelectAmountId>> GetAllAmountsAsync()
        {
            return await this._dbContext.Amounts
                .Select(a => new SelectAmountId
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToListAsync();
        }
    }
}
