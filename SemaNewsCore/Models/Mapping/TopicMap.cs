using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SemaNewsCore.Models.Mapping
{
    public class TopicMap : EntityTypeConfiguration<Topic>
    {
        public TopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Topic");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Keyphrases).HasColumnName("Keyphrases");
            this.Property(t => t.KeyphraseGraphs).HasColumnName("KeyphraseGraphs");
        }
    }
}
