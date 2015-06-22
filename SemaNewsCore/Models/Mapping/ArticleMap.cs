using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired().HasMaxLength(256);

            this.Property(t => t.Url)
                .IsRequired().HasMaxLength(256);

            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Article");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.ReleasedDate).HasColumnName("ReleasedDate");
            this.Property(t => t.CollectedDate).HasColumnName("CollectedDate");
            this.Property(t => t.Abstract).HasColumnName("Abstract");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.IsIndexed).HasColumnName("IsIndexed");
            this.Property(t => t.IsMark).HasColumnName("IsMark");
            this.Property(t => t.IsRelevantToLocal).HasColumnName("IsRelevantToLocal");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.GFieldId).HasColumnName("GFieldId");
            this.Property(t => t.NewspaperId).HasColumnName("NewspaperId");

            // Relationships
            this.HasOptional(t => t.Field)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.FieldId);
            this.HasOptional(t => t.GField)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.GFieldId);
            this.HasOptional(t => t.Newspaper)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.NewspaperId);

        }
    }
}
