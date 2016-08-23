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
    public interface IArticleService
    {
        IEnumerable<Article> GetArticles(string title = null);
        Article GetArticle(int id);
        Article GetArticle(string title);
        void CreateArticle(Article article);
        void UpdateArticle(Article article);
        void DeleteArticle(Article article);
        void SaveArticle();
    }

    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository,
            IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWOrk = unitOfWork;
        }

        public IEnumerable<Article> GetArticles(string title = null)
        {
            if (string.IsNullOrEmpty(title))
                return _articleRepository.GetAll();
            else
                return _articleRepository.GetMany(a => a.Title.ToLower().Contains(title.ToLower()));
        }

        public Article GetArticle(int id)
        {
            return _articleRepository.GetSingle(id);
        }

        public Article GetArticle(string title)
        {
            return _articleRepository.GetArticleByTitle(title);
        }

        public void CreateArticle(Article article)
        {
            _articleRepository.Add(article);
        }

        public void UpdateArticle(Article article)
        {
            _articleRepository.Edit(article);
        }

        public void DeleteArticle(Article article)
        {
            _articleRepository.Delete(article);
        }

        public void SaveArticle()
        {
            _unitOfWOrk.Commit();
        }
    }
}
