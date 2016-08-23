using Microsoft.Owin.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UnitTestingWebAPI.Core.Filters;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Tests.Hosting;

namespace UnitTestingWebAPI.Tests
{
    [TestFixture]
    public class ActionFilterTest 
    {
        private List<Article> _articles;

        [SetUp]
        public void Setup()
        {
            _articles = AppInitializer.GetAllArticles();
        }

        [Test]
        public void ShouldSortArticlesByTitle()
        {
            var filter = new ArticlesReversedFilter();
            var executedContext = new HttpActionExecutedContext(
                new HttpActionContext
                {
                    Response = new System.Net.Http.HttpResponseMessage()
                },
                null
            );
            executedContext.Response.Content = new ObjectContent<List<Article>>(new List<Article>(_articles), new JsonMediaTypeFormatter());
            filter.OnActionExecuted(executedContext);
            var _returnArticles = executedContext.Response.Content.ReadAsAsync<List<Article>>().Result;

            Assert.That(_returnArticles.First(), Is.EqualTo(_articles.Last()));
        }
        [Test]
        public void ShouldCallToControllerActionRevereseArticles()
        {
            var address = "http://localhost:9000/";

            using (WebApp.Start<Startup>(address))
            {
                HttpClient client = new HttpClient();
                var response = client.GetAsync(address + "api/articles").Result;
                var returnArticles = response.Content.ReadAsAsync<List<Article>>().Result;

                Assert.That(returnArticles.First().Title, Is.EqualTo(AppInitializer.GetAllArticles().Last().Title));
            }
        }
    }
}
