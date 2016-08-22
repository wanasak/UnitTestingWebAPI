﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;
using UnitTestingWebAPI.Core.Controllers;

namespace UnitTestingWebAPI.Core
{
    // used for controller registration 
    public class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies().ToList();
            var assemblies = new List<Assembly>(baseAssemblies) { typeof(BlogsController).Assembly };
            baseAssemblies.AddRange(assemblies);
            return baseAssemblies.Distinct().ToList();
        }
    }
}
