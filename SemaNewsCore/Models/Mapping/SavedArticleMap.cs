using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class SavedArticleMap : EntityTypeConfiguration<SavedArticle>
    {
        public SavedArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired();

            this.Property(t => t.Url)
                .IsRequired();

            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SavedArticle");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserQueryId).HasColumnName("UserQueryId");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.SavedTime).HasColumnName("SavedTime");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.ReleasedDate).HasColumnName("ReleasedDate");
            this.Property(t => t.CollectedDate).HasColumnName("CollectedDate");
            this.Property(t => t.Abstract).HasColumnName("Abstract");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.GFieldId).HasColumnName("GFieldId");
            this.Property(t => t.NewspaperId).HasColumnName("NewspaperId");

            // Relationships
            this.HasOptional(t => t.Article)
                .WithMany(t => t.SavedArticles)
                .HasForeignKey(d => d.ArticleId);
            this.HasOptional(t => t.Field)
                .WithMany(t => t.SavedArticles)
                .HasForeignKey(d => d.FieldId);
            this.HasOptional(t => t.GField)
                .WithMany(t => t.SavedArticles)
                .HasForeignKey(d => d.GFieldId);
            this.HasOptional(t => t.Newspaper)
                .WithMany(t => t.SavedArticles)
                .HasForeignKey(d => d.NewspaperId);
            this.HasRequired(t => t.UserQuery)
                .WithMany(t => t.SavedArticles)
                .HasForeignKey(d => d.UserQueryId);

        }
    }
}
