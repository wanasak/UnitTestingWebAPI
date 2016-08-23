using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Core.MediaTypeFormatter;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Tests
{
    [TestFixture]
    public class MediaTypeFormatterTest
    {
        Blog _blog;
        Article _article;
        ArticleFormatter _formatter;

        [SetUp]
        public void Setup()
        {
            _blog = AppInitializer.GetBlogs().First();
            _article = AppInitializer.GetTechFundaArticles().First();
            _formatter = new ArticleFormatter();
        }


        [Test]
        public void FormatterShouldThrowExceptionWhenUnsupportedType()
        {
            Assert.Throws<InvalidOperationException>(() => new ObjectContent<Blog>(_blog, _formatter));
        }
        [Test]
        public void FormatterShouldNotThrowExceptionWhenUnsupportedType()
        {
            Assert.DoesNotThrow(() => new ObjectContent<Article>(_article, _formatter));
        }
        [Test]
        public void FormatterShouldHeaderBeSetCorrectly()
        {
            var content = new ObjectContent<Article>(_article, new ArticleFormatter());

            Assert.That(content.Headers.ContentType.MediaType, Is.EqualTo("application/article"));
        }
        [Test]
        public void FormatterShouldBeAbleToDeserializeArticle()
        {
            var content = new ObjectContent<Article>(_article, _formatter);
            Task<Article> deserializedArticle = content.ReadAsAsync<Article>(
                new[] { _formatter } 
                );
            deserializedArticle.Wait();

            Assert.That(_article, Is.SameAs(deserializedArticle.Result));
        }
        [Test]
        public void FormatterShouldNotBeAbleToWriteUnsupportedType()
        {
            var canWriteBlog = _formatter.CanWriteType(typeof(Blog));

            Assert.That(canWriteBlog, Is.False);
        }
        [Test]
        public void FormatterShouldBeAbleToWriteArticle()
        {
            var canWriteArticle = _formatter.CanWriteType(typeof(Article));

            Assert.That(canWriteArticle, Is.True);
        }
    }
}
