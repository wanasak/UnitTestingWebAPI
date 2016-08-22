using Autofac;
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
            //builder.RegisterApiControllers(typeof(BlogController).Assembly);
        }
    }
}
