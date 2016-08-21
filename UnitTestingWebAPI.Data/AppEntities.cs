using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Data.Configurations;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data
{
    public class AppEntities : DbContext
    {
        public AppEntities()
            : base("AppEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Blog> Blogs { get; set; }


        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new BlogConfiguration());
        }
    }
}
