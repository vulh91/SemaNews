using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class GGRelationInstanceMap : EntityTypeConfiguration<GGRelationInstance>
    {
        public GGRelationInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.GFieldId1, t.GFieldId2, t.GGRelationId });

            // Properties
            this.Property(t => t.GFieldId1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.GFieldId2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.GGRelationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("GGRelationInstance");
            this.Property(t => t.GFieldId1).HasColumnName("GFieldId1");
            this.Property(t => t.GFieldId2).HasColumnName("GFieldId2");
            this.Property(t => t.GGRelationId).HasColumnName("GGRelationId");

            // Relationships
            this.HasRequired(t => t.GField1)
                .WithMany(t => t.GGRelationInstancesOut)
                .HasForeignKey(d => d.GFieldId1);
            this.HasRequired(t => t.GField2)
                .WithMany(t => t.GGRelationInstancesIn)
                .HasForeignKey(d => d.GFieldId2);
            this.HasRequired(t => t.GGRelation)
                .WithMany(t => t.GGRelationInstances)
                .HasForeignKey(d => d.GGRelationId);

        }
    }
}
