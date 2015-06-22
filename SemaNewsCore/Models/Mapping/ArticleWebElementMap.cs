using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class ArticleWebElementMap : EntityTypeConfiguration<ArticleWebElement>
    {
        public ArticleWebElementMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ArticleWebElement");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.WebElementTypeId).HasColumnName("WebElementTypeId");
            this.Property(t => t.NewspaperId).HasColumnName("NewspaperId");

            // Relationships
            this.HasRequired(t => t.Newspaper)
                .WithMany(t => t.ArticleWebElements)
                .HasForeignKey(d => d.NewspaperId);
            this.HasRequired(t => t.WebElementType)
                .WithMany(t => t.ArticleWebElements)
                .HasForeignKey(d => d.WebElementTypeId);

        }
    }
}
