using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data.Configurations
{
    public class ArticleConfiguration : EntityTypeConfiguration<Article>
    {
        public ArticleConfiguration()
        {
            //ToTable("Article");
            Property(a => a.Author)
                .IsRequired()
                .HasMaxLength(50);
            Property(a => a.Contents)
                .IsRequired();
            Property(a => a.DateCreated)
                .HasColumnType("datetime2");
            Property(a => a.DateEdited)
                .HasColumnType("datetime2");
            Property(a => a.URL)
                .IsRequired()
                .HasMaxLength(1000);
            Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
