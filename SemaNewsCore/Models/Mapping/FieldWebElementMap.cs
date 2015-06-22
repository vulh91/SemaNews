using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class FieldWebElementMap : EntityTypeConfiguration<FieldWebElement>
    {
        public FieldWebElementMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Address)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("FieldWebElement");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.WebElementTypeId).HasColumnName("WebElementTypeId");
            this.Property(t => t.NewspaperId).HasColumnName("NewspaperId");

            // Relationships
            this.HasRequired(t => t.Newspaper)
                .WithMany(t => t.FieldWebElements)
                .HasForeignKey(d => d.NewspaperId);
            this.HasRequired(t => t.WebElementType)
                .WithMany(t => t.FieldWebElements)
                .HasForeignKey(d => d.WebElementTypeId);

        }
    }
}
