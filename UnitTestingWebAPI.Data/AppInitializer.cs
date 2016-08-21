using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data
{
    public class AppInitializer : DropCreateDatabaseIfModelChanges<AppEntities>
    {
        protected override void Seed(AppEntities context)
        {

            context.Commit();
        }

        public static List<Blog> GetBlogs()
        {
            List<Blog> _blogs = new List<Blog>()
            {
                new Blog()
                {
                    Name = "TechFunda",
                    URL = "http://techfunda.com/",
                    Owner = "Robert Edward",
                    Articles = GetTechFundaArticles()
                },
                new Blog()
                {
                    Name = "Dotnet awesome",
                    URL = "http://www.dotnetawesome.com/",
                    Owner = "Alan Smith",
                    Articles = GetDotnetAwesomeArticles()
                }
            };
            return _blogs;
        }

        public static List<Article> GetTechFundaArticles()
        {
            List<Article> _articles = new List<Article>()
            {
                new Article()
                {
                    Author = "TechFunda.com",
                    Title = "ASP.NET MVC > Basics",
                    Contents = "ASP.NET MVC stands for ASP.NET Model View Controller design pattern. MVC was first invented by Trygve Reenskaug and was named as “Thing Model View Editor” pattern originally. Slowly it became popular and renamed as “Model View Controller”.",
                    URL = "http://techfunda.com/howto/asp-net-mvc/basics"
                },
                new Article()
                {
                    Author = "TechFunda.com",
                    Title = "SQL Server > Table ",
                    Contents = "Table is an object in the database that actually holds the data into the database. Data is stored in the form of rows and columns into the database.",
                    URL = "http://techfunda.com/howto/sql-server/table"
                }
            };
            return _articles;
        }

        public static List<Article> GetDotnetAwesomeArticles()
        {
            List<Article> _articles = new List<Article>()
            {
                new Article()
                {
                    Author = "Sourav Mondal",
                    Title = "Advance master details entry form in asp.net MVC ",
                    Contents = "In the one of my previous article, I have explained how to create a simple master details entry form in asp.net MVC application, where I explained very simple one, without edit, delete functionality and all with text input field for details record for easy to understand.",
                    URL = "http://www.dotnetawesome.com/2016/07/advance-master-details-entry-form-in-mvc.html"
                }
            };
            return _articles;
        }

        public static List<Article> GetAllArticles()
        {
            List<Article> _articles = new List<Article>();
            _articles.AddRange(GetTechFundaArticles());
            _articles.AddRange(GetDotnetAwesomeArticles());
            return _articles;
        }
    }
}
