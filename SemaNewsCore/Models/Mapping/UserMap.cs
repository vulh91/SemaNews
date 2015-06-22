using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired();

            this.Property(t => t.Password)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasOptional(t => t.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
