namespace Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ElectronicStoreDbContext dbContext;

        public ElectronicStoreDbContext Init()
        {
            return dbContext ?? (dbContext = new ElectronicStoreDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}