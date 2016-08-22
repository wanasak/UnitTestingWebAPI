using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.Core.Controllers
{
    public class BlogController : ApiController
    {
        private IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: api/blogs
        public IEnumerable<Blog> GetBlogs()
        {
            return _blogService.GetBlogs();
        }

        // GET: api/blogs/5
        public IHttpActionResult GetBlog(int id)
        {
            Blog blog = _blogService.GetBlog(id);
            if (blog == null)
                return NotFound();
            return Ok(blog);
        }

        // PUT: api/blogs/5
        public IHttpActionResult PutBlog(Blog blog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _blogService.UpdateBlog(blog);

            try
            {
                _blogService.SaveBlog();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExist(blog.BlogID))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/blogs
        public IHttpActionResult PostBlog(Blog blog)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _blogService.CreateBlog(blog);

            return CreatedAtRoute("DefaultApi", new { id = blog.BlogID }, blog);
        }

        // DELETE: api/blogs/5
        public IHttpActionResult DeleteBlog(int id)
        {
            Blog blog = _blogService.GetBlog(id);
            if (blog == null)
                return NotFound();
            _blogService.DeleteBlog(blog);
            return Ok(blog);
        }

        private bool BlogExist(int id)
        {
            return _blogService.GetBlog(id) != null;
        }
    }
}
