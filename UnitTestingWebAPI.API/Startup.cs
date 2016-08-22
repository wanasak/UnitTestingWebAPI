using Autofac;
using Autofac.Integration.WebApi;
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
using UnitTestingWebAPI.Core.MediaTypeFormatter;
using UnitTestingWebAPI.Data.Infrastructure;
using UnitTestingWebAPI.Data.Repository;
using UnitTestingWebAPI.Service;

namespace UnitTestingWebAPI.API
{
    // This code will ensure to use WebApi controllers from the UnitTestingWebAPI.Core project(CustomAssembliesResolver)
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());
            config.Formatters.Add(new ArticleFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Autofac configuration
            var builder = new ContainerBuilder();
            // Register API Controller in different project
            builder.RegisterApiControllers(typeof(BlogsController).Assembly);
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(BlogRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Serrvices
            builder.RegisterAssemblyTypes(typeof(ArticleService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            IContainer container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseWebApi(config);
        }
    }
}
