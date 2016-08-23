using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.Core.Controllers
{
    public class ArticlesController : ApiController
    {
        private IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // GET: api/articles
        public IEnumerable<Article> GetArticles()
        {
            return _articleService.GetArticles();
        }

        // GET: api/articles/5
        [ResponseType(typeof(Article))]
        public IHttpActionResult GetArticle(int id)
        {
            Article article = _articleService.GetArticle(id);
            if (article == null)
                return NotFound();
            return Ok(article);
        }

        // PUT: api/articles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutArticle(Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (article.ID < 0)
                return BadRequest();

            _articleService.UpdateArticle(article);

            try
            {
                _articleService.SaveArticle();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExist(article.ID))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/articles
        [ResponseType(typeof(Article))]
        public IHttpActionResult PostArticle(Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _articleService.CreateArticle(article);

            return CreatedAtRoute("DefaultApi", new { id = article.ID }, article);
        }

        // DELETE: api/articles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteArticle(int id)
        {
            Article article = _articleService.GetArticle(id);
            if (article == null)
                return NotFound();
            _articleService.DeleteArticle(article);
            return Ok(article);
        }

        private bool ArticleExist(int id)
        {
            return _articleService.GetArticle(id) != null;
        }
    }
}
