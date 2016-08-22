using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Core.MediaTypeFormatter
{
    public class ArticleFormatter : BufferedMediaTypeFormatter
    {
        public ArticleFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/article"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            // for single article 
            if (type == typeof(Article))
                return true;
            else
            {
                // for multiple article
                Type _type = typeof(IEnumerable<Article>);
                return _type.IsAssignableFrom(type);
            }
        }

        public override void WriteToStream(Type type, 
            object value, 
            System.IO.Stream writeStream, 
            System.Net.Http.HttpContent content)
        {
            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                var articles = value as IEnumerable<Article>;
                if (articles != null)
                {
                    foreach (var article in articles)
                    {
                        writer.Write(string.Format("[{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                            article.ID,
                            article.Title,
                            article.Author,
                            article.URL,
                            article.Contents));
                    }
                }
                else
                {
                    var article = value as Article;
                    if (article == null)
                        throw new InvalidOperationException("Cannot serialize type");
                    writer.Write(string.Format("[{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                            article.ID,
                            article.Title,
                            article.Author,
                            article.URL,
                            article.Contents));
                }
            }
        }
    }
}
