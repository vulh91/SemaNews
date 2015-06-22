using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class FFRelationInstanceMap : EntityTypeConfiguration<FFRelationInstance>
    {
        public FFRelationInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FieldId1, t.FieldId2 });

            // Properties
            this.Property(t => t.FieldId1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FieldId2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("FFRelationInstance");
            this.Property(t => t.FieldId1).HasColumnName("FieldId1");
            this.Property(t => t.FieldId2).HasColumnName("FieldId2");
            this.Property(t => t.NRelationId).HasColumnName("NRelationId");

            // Relationships
            this.HasRequired(t => t.Field1)
                .WithMany(t => t.FFRelationInstances)
                .HasForeignKey(d => d.FieldId1);
            this.HasRequired(t => t.Field2)
                .WithMany(t => t.FFRelationInstances1)
                .HasForeignKey(d => d.FieldId2);
            this.HasRequired(t => t.NRelation)
                .WithMany(t => t.FFRelationInstances)
                .HasForeignKey(d => d.NRelationId);

        }
    }
}
