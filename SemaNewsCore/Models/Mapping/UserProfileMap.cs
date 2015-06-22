using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.Avatar).HasColumnName("Avatar");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.Signature).HasColumnName("Signature");

            // Relationships
            this.HasRequired(t => t.User)
                .WithOptional(t => t.UserProfile);

        }
    }
}
