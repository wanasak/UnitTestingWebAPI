using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Data.Repository;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.Tests
{
    [TestFixture]
    public class ServiceTest
    {
        #region Variables
        IArticleService _articleService;
        IArticleRepository _articleRepository;
        IUnitOfWork _unitOfWork;
        List<Article> _randomArticles;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            _randomArticles = SetupArticles();

            _articleRepository = SetupArticleRepository();
            _unitOfWork = new Mock<IUnitOfWork>().Object;
            _articleService = new ArticleService(_articleRepository, _unitOfWork);
        }
        public List<Article> SetupArticles()
        {
            int counter = new int();
            List<Article> articles = AppInitializer.GetAllArticles();

            foreach (Article article in articles)
                article.ID = ++counter;

            return articles;
        }
        public IArticleRepository SetupArticleRepository()
        {
            // Init repository
            var repo = new Mock<IArticleRepository>();

            // Setup mocking behavior
            repo.Setup(r => r.GetAll()).Returns(_randomArticles);
            repo.Setup(r => r.GetSingle(It.IsAny<int>()))
                .Returns(new Func<int, Article>(
                    id => _randomArticles.Find(a => a.ID.Equals(id))
                        ));
            repo.Setup(r => r.Add(It.IsAny<Article>()))
                .Callback(new Action<Article>(newArticle =>
                    {
                        dynamic maxArticleID = _randomArticles.Last().ID;
                        dynamic nextArticleID = maxArticleID + 1;
                        newArticle.ID = nextArticleID;
                        newArticle.DateCreated = DateTime.Now;
                        _randomArticles.Add(newArticle);
                    }));
            repo.Setup(r => r.Edit(It.IsAny<Article>()))
                .Callback(new Action<Article>(updateArticle =>
                    {
                        var oldArticle = _randomArticles.Find(a => a.ID == updateArticle.ID);
                        oldArticle.DateEdited = DateTime.Now;
                        oldArticle = updateArticle;
                    }));
            repo.Setup(r => r.Delete(It.IsAny<Article>()))
                .Callback(new Action<Article>(deleteArticle =>
                    {
                        var article = _randomArticles.Find(a => a.ID == deleteArticle.ID);
                        if (article != null)
                            _randomArticles.Remove(article);
                    }));

            return repo.Object;
        }
        #endregion

        #region Tests
        [Test]
        public void ServiceShouldReturnAllArticles()
        {
            var articles = _articleService.GetArticles().ToList();

            Assert.That(articles, Is.EqualTo(_randomArticles));
        }
        [Test]
        public void ServiceShouldReturnRightArticle()
        {
            var articles = _articleService.GetArticle(2);

            Assert.That(articles, Is.EqualTo(_randomArticles.Find(a => a.Title.Contains("SQL Server > Table"))));
        }
        [Test]
        public void ServiceShouldAddNewArticle()
        {
            var newArticle = new Article()
            {
                Author = "Wanasak Suraintaranggoon",
                Contents = "Test Contents.",
                Title = "Test Title",
                URL = "https://wordpress.com/stats/insights/smudger194.wordpress.com"
            };
            int maxArticleID = _randomArticles.Last().ID;
            _articleService.CreateArticle(newArticle);

            Assert.That(newArticle, Is.EqualTo(_randomArticles.Last()));
            Assert.That(maxArticleID + 1, Is.EqualTo(_randomArticles.Last().ID));
        }
        [Test]
        public void ServiceShouldUpdateArticle()
        {
            var firstArticle = _randomArticles.First();
            firstArticle.Title = "Update Title";
            _articleService.UpdateArticle(firstArticle);

            Assert.That(firstArticle.DateEdited, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(firstArticle.ID, Is.EqualTo(1));
        }
        [Test]
        public void ServiceShouldDeleteArticle()
        {
            int maxID = _randomArticles.Max(a => a.ID);
            var lastArticle = _randomArticles.Last();

            _articleService.DeleteArticle(lastArticle);

            Assert.That(maxID, Is.GreaterThan(_randomArticles.Max(a => a.ID)));
        }
        #endregion

    }
}
