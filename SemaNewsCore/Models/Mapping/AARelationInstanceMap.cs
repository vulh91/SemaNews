using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class AARelationInstanceMap : EntityTypeConfiguration<AARelationInstance>
    {
        public AARelationInstanceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ArticleId1, t.ArticleId2 });

            // Properties
            this.Property(t => t.ArticleId1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ArticleId2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AARelationInstance");
            this.Property(t => t.ArticleId1).HasColumnName("ArticleId1");
            this.Property(t => t.ArticleId2).HasColumnName("ArticleId2");
            this.Property(t => t.NRelationId).HasColumnName("NRelationId");
            this.Property(t => t.RelationWeight).HasColumnName("RelationWeight");

            // Relationships
            this.HasRequired(t => t.Article)
                .WithMany(t => t.AARelationInstances)
                .HasForeignKey(d => d.ArticleId1);
            this.HasRequired(t => t.Article1)
                .WithMany(t => t.AARelationInstances1)
                .HasForeignKey(d => d.ArticleId2);
            this.HasRequired(t => t.NRelation)
                .WithMany(t => t.AARelationInstances)
                .HasForeignKey(d => d.NRelationId);

        }
    }
}
