using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data.Repository
{
    public class ArticleRepository : EntityBaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        public Article GetArticleByTitle(string title)
        {
            var _article = DbContext.Articles.Where(a => a.Title == title).FirstOrDefault();
            return _article;
        }
    }

    public interface IArticleRepository : IEntityBaseRepository<Article>
    {
        Article GetArticleByTitle(string title);
    }
}
