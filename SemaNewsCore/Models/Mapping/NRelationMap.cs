using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class NRelationMap : EntityTypeConfiguration<NRelation>
    {
        public NRelationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("NRelation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Notation).HasColumnName("Notation");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
