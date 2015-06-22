using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public class TopicMapping : EntityTypeConfiguration<Topic>
    {
        public TopicMapping()
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
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Keyphrases).HasColumnName("Keyphrases");
            this.Property(t => t.KeyphraseGraphs).HasColumnName("KeyphraseGraphs");
        }
    }
}
