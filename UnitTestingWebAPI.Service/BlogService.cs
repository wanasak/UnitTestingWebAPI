using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Data.Repository;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Service
{
    public interface IBlogService
    {
        IEnumerable<Blog> GetBlogs(string name = null);
        Blog GetBlog(int id);
        Blog GetBlog(string name);
        void CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(Blog blog);
        void SaveBlog();
    }

    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
        }

        public IEnumerable<Blog> GetBlogs(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return _blogRepository.GetAll();
            else
                return _blogRepository.GetMany(b => b.Name.ToLower().Contains(name.ToLower()));
        }
        public Blog GetBlog(int id)
        {
            return _blogRepository.GetSingle(id);
        }
        public Blog GetBlog(string name)
        {
            return _blogRepository.GetBlogByName(name);
        }
        public void CreateBlog(Blog blog)
        {
            _blogRepository.Add(blog);
        }
        public void UpdateBlog(Blog blog)
        {
            _blogRepository.Edit(blog);
        }
        public void DeleteBlog(Blog blog)
        {
            _blogRepository.Delete(blog);
        }
        public void SaveBlog()
        {
            _unitOfWork.Commit();
        }
    }
}
