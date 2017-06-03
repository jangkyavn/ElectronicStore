using Data.Infrastructure;
using Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totoRow);
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Post> GetAllByTagPaging(string tag, int page, int pageSize, out int totoRow)
        {
            var query = from p in DbContext.Posts
                        join pt in DbContext.PostTags on p.ID equals pt.PostID
                        where pt.TagID == tag && p.Status
                        orderby p.CreatedDate descending
                        select p;

            totoRow = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return query;
        }
    }
}
