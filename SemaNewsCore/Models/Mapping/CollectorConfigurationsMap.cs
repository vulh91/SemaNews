using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class CollectorConfigurationsMap : EntityTypeConfiguration<CollectorConfigurations>
    {
        public CollectorConfigurationsMap()
        {
            //Primary key
            this.HasKey(t => t.Id);

            //Properties
            this.Property(t => t.Name);
            this.Property(t => t.Value);
            this.Property(t => t.Description);

            //Table & Column Mappings
            this.ToTable("CollectorConfigurations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
