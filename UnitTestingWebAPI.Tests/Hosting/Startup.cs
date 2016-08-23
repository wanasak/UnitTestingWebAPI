using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using UnitTestingWebAPI.Core;
using UnitTestingWebAPI.Core.Controllers;
using UnitTestingWebAPI.Core.Filters;
using UnitTestingWebAPI.Core.MessageHandlers;
using UnitTestingWebAPI.Data;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Data.Repository;
using UnitTestingWebAPI.Entity;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.Tests.Hosting
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new HeaderAppenderHandler());
            config.MessageHandlers.Add(new EndRequestHandler());
            config.Filters.Add(new ArticlesReversedFilter());
            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MapHttpAttributeRoutes();

            // Autofac configuration
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(ArticlesController).Assembly);

            // Unit Of Work
            var _unitOfWork = new Mock<IUnitOfWork>();
            builder.RegisterInstance(_unitOfWork.Object).As<IUnitOfWork>();

            // Repositories
            var articlesRepository = new Mock<IArticleRepository>();
            articlesRepository.Setup(x => x.GetAll()).Returns(
                    AppInitializer.GetAllArticles()
                );
            builder.RegisterInstance(articlesRepository.Object).As<IArticleRepository>();

            var blogRepository = new Mock<IBlogRepository>();
            blogRepository.Setup(x => x.GetAll())
                .Returns(AppInitializer.GetBlogs());
            builder.RegisterInstance(blogRepository.Object).As<IBlogRepository>();

            // Services
            builder.RegisterAssemblyTypes(typeof(ArticleService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterInstance(new ArticleService(articlesRepository.Object, _unitOfWork.Object));
            builder.RegisterInstance(new BlogService(blogRepository.Object, _unitOfWork.Object));

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}
