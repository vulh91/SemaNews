using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class ArticleKGMap : EntityTypeConfiguration<ArticleKG>
    {
        public ArticleKGMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ArticleKG");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LDVL_Graph).HasColumnName("LDVL_Graph");
            this.Property(t => t.DT_Graph).HasColumnName("DT_Graph");

            // Relationships
            this.HasRequired(t => t.Article)
                .WithOptional(t => t.ArticleKG);

        }
    }
}
