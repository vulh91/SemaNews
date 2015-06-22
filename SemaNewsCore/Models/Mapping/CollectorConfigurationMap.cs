using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class CollectorConfigurationMap : EntityTypeConfiguration<CollectorConfiguration>
    {
        public CollectorConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("CollectorConfigurations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
