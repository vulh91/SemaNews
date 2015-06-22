using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models.Mapping
{
    public class VisitedLinkMap : EntityTypeConfiguration<VisitedLink>
    {
        public VisitedLinkMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("VisitedLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.URL).HasColumnName("URL").HasMaxLength(250);
            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(250);
            this.Property(t => t.VisitCount).HasColumnName("VisitCount");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}
