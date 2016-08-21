using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingWebAPI.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        AppEntities dbContext;

        public AppEntities Init()
        {
            return dbContext ?? (dbContext = new AppEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
