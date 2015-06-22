using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class FieldMap : EntityTypeConfiguration<Field>
    {
        public FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Url)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Field");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsActivated).HasColumnName("IsActivated");
            this.Property(t => t.Group).HasColumnName("Group");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.DefinedTime).HasColumnName("DefinedTime");
            this.Property(t => t.NewspaperId).HasColumnName("NewspaperId");
            this.Property(t => t.GFieldId).HasColumnName("GFieldId");

            // Relationships
            this.HasOptional(t => t.GField)
                .WithMany(t => t.Fields)
                .HasForeignKey(d => d.GFieldId);
            this.HasOptional(t => t.Newspaper)
                .WithMany(t => t.Fields)
                .HasForeignKey(d => d.NewspaperId);

        }
    }
}
