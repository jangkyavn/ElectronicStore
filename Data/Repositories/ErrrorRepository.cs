using Data.Infrastructure;
using Model.Models;

namespace Data.Repositories
{
    public interface IErrrorRepository : IRepository<Error>
    {

    }

    public class ErrrorRepository : RepositoryBase<Error>, IErrrorRepository
    {
        public ErrrorRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
