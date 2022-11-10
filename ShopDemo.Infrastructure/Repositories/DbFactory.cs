namespace ShopDemo.Infrastructure.Repositories
{
    public interface IDbFactory : IDisposable
    {
        ApplicationDbContext Init();
    }

    public class DbFactoryData : IDbFactory
    {
        public ApplicationDbContext dbContext;

        public DbFactoryData(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public ApplicationDbContext Init()
        {
            return dbContext;
        }

        #region IDisposable Support

        private bool isDisposed;

        ~DbFactoryData()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                DisposeCore();
            }
        }

        protected virtual void DisposeCore()
        {
        }

        #endregion IDisposable Support
    }
}
