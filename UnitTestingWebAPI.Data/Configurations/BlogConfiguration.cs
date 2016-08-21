using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestingWebAPI.Entity;

namespace UnitTestingWebAPI.Data.Configurations
{
    public class BlogConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogConfiguration()
        {
            Property(b => b.DateCreated)
                .HasColumnType("datetime2");
            Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            Property(b => b.URL)
                .IsRequired()
                .HasMaxLength(200);
            Property(b => b.Owner)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
