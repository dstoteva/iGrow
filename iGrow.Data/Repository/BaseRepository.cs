namespace iGrow.Data.Repository
{
    public abstract class BaseRepository : IDisposable
    {
        private bool isDisposed = false;
        private readonly ApplicationDbContext _dbContext;

        protected BaseRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        protected ApplicationDbContext DbContext
            => _dbContext;

        protected async Task<int> SaveChangesAsync()
        {
            return await DbContext!.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _dbContext?.Dispose();
                }
            }
            isDisposed = true;
        }
    }
}
