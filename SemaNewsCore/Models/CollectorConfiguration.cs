using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class CollectorConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
