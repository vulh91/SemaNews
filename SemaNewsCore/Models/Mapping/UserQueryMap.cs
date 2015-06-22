using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class UserQueryMap : EntityTypeConfiguration<UserQuery>
    {
        public UserQueryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserQuery");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SearchQuery).HasColumnName("SearchQuery");
            this.Property(t => t.SavedTime).HasColumnName("SavedTime");
            this.Property(t => t.IsSaved).HasColumnName("IsSaved");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserQueries)
                .HasForeignKey(d => d.UserId);

        }
    }
}
