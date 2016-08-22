using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using UnitTestingWebAPI.Core.Controllers;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Data.Repository;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.Tests
{
    [TestFixture]
    public class ControllerTest
    {
        #region Variables
        IArticleService _articleService;
        IArticleRepository _articleRepository;
        IUnitOfWork _unitOfWork;
        List<Article> _randomArticles;
        #endregion

        #region Setup
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
            repo.Setup(r => r.GetAll().ToList()).Returns(_randomArticles);
            repo.Setup(r => r.GetSingle(It.IsAny<int>()))
                .Returns(new Func<int, Article>(id =>
                    _randomArticles.Find(a => a.ID.Equals(id))));
            repo.Setup(r => r.Add(It.IsAny<Article>()))
                .Callback(new Action<Article>(addArticle =>
                    {
                        dynamic maxArticleID = _randomArticles.Max(a => a.ID);
                        dynamic nextArticleID = ++maxArticleID;
                        addArticle.ID = nextArticleID;
                        addArticle.DateCreated = DateTime.Now;
                        _randomArticles.Add(addArticle);
                    }));
            repo.Setup(r => r.Edit(It.IsAny<Article>()))
                .Callback(new Action<Article>(updateArticle =>
                    {
                        var oldArticle = _randomArticles.Find(a => a.ID == updateArticle.ID);
                        oldArticle.DateEdited = DateTime.Now;
                        oldArticle.URL = updateArticle.URL;
                        oldArticle.Title = updateArticle.Title;
                        oldArticle.Contents = updateArticle.Contents;
                        oldArticle.BlogID = updateArticle.BlogID;
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

        #region Test
        [Test]
        public void ControllerShouldReturnAllArticles()
        {
            var articleController = new ArticlesController(_articleService);
            var result = articleController.GetArticles();

            CollectionAssert.AreEqual(result, _randomArticles);
        }
        [Test]
        public void ControllerShouldReturnLastArticle()
        {
            var articleController = new ArticlesController(_articleService);
            var result = articleController.GetArticle(3) as OkNegotiatedContentResult<Article>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Title, _randomArticles.Last().Title);
        }
        [Test]
        public void ControllerShouldPutReturnBadRequestResult()
        {
            var articleController = new ArticlesController(_articleService)
            {
                Configuration = new System.Web.Http.HttpConfiguration(),
                Request = new System.Net.Http.HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri("http://localhost/api/articles/-1")
                }
            };
            var badResult = articleController.PutArticle(new Article() { ID = -1, Title = "Unknow Article" });
            Assert.That(badResult, Is.TypeOf<BadRequestResult>());
        }
        #endregion
    }
}
