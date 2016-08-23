using Microsoft.Owin.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTestingWebAPI.Core.MessageHandlers;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Tests.Hosting;

namespace UnitTestingWebAPI.Tests
{
    [TestFixture]
    public class MessageHandlerTests
    {
        private EndRequestHandler _endRequestHandler;
        private HeaderAppenderHandler _headerAppenderHandler;

        [SetUp]
        public void Setup()
        {
            // Direct MessageHandler test
            _endRequestHandler = new EndRequestHandler();
            _headerAppenderHandler = new HeaderAppenderHandler()
            {
                InnerHandler = _endRequestHandler
            };
        }

        [Test]
        public async void ShouldAppendCustomHeader()
        {
            var invoker = new HttpMessageInvoker(_headerAppenderHandler);
            var result = await invoker.SendAsync(
                new HttpRequestMessage(
                    HttpMethod.Get,
                    new Uri("http://localhost/api/test/")),
                CancellationToken.None
            );

            Assert.That(result.Headers.Contains("X-WebAPI-Header"), Is.True);
            Assert.That(result.Content.ReadAsStringAsync().Result, Is.EqualTo("Unit testing message handlers!"));
        }
        [Test]
        public void ShouldCallToControllerActionAppendCustomHeader()
        {
            var address = "http://localhost:9000/";

            using (WebApp.Start<Startup>(address))
            {
                HttpClient _client = new HttpClient();
                var response = _client.GetAsync(address + "api/articles").Result;

                Assert.That(response.Headers.Contains("X-WebAPI-Header"), Is.True);

                var _returnArticles = response.Content.ReadAsAsync<List<Article>>().Result;
                Assert.That(_returnArticles.Count, Is.EqualTo(AppInitializer.GetAllArticles().Count));
            }
        }
    }
}
