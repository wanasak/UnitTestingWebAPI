using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data.Repository
{
    public class BlogRepository : EntityBaseRepository<Blog>, IBlogRepository
    {
        public BlogRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public Blog GetBlogByName(string name)
        {
            var _blog = DbContext.Blogs.Where(b => b.Name == name).First();
            return _blog;
        }
    }

    public interface IBlogRepository : IEntityBaseRepository<Blog>
    {
        Blog GetBlogByName(string name);
    }
}
