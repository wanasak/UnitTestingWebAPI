using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UnitTestingWebAPI.Core.Filters;
using UnitTestingWebAPI.Core.MessageHandlers;

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
        }
    }
}
