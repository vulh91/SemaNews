using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class NewspaperMap : EntityTypeConfiguration<Newspaper>
    {
        public NewspaperMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Url)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Newspaper");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.IsLocal).HasColumnName("IsLocal");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DefinedTime).HasColumnName("DefinedTime");
            this.Property(t => t.IsActivated).HasColumnName("IsActivated");
        }
    }
}
