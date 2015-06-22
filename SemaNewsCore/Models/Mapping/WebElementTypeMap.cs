using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class WebElementTypeMap : EntityTypeConfiguration<WebElementType>
    {
        public WebElementTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("WebElementType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WENotation).HasColumnName("WENotation");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
