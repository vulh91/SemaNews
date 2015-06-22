using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Keyphrases { get; set; }
        public string KeyphraseGraphs { get; set; }
    }
}
