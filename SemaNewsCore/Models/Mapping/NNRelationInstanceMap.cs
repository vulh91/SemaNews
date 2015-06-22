using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class NNRelationInstanceMap : EntityTypeConfiguration<NNRelationInstance>
    {
        public NNRelationInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.NewspaperId1, t.NewspaperId2 });

            // Properties
            this.Property(t => t.NewspaperId1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NewspaperId2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("NNRelationInstance");
            this.Property(t => t.NewspaperId1).HasColumnName("NewspaperId1");
            this.Property(t => t.NewspaperId2).HasColumnName("NewspaperId2");
            this.Property(t => t.NRelationId).HasColumnName("NRelationId");

            // Relationships
            this.HasRequired(t => t.Newspaper1)
                .WithMany(t => t.NNRelationInstancesOut)
                .HasForeignKey(d => d.NewspaperId1);
            this.HasRequired(t => t.Newspaper2)
                .WithMany(t => t.NNRelationInstancesIn)
                .HasForeignKey(d => d.NewspaperId2);
            this.HasRequired(t => t.NRelation)
                .WithMany(t => t.NNRelationInstances)
                .HasForeignKey(d => d.NRelationId);

        }
    }
}
