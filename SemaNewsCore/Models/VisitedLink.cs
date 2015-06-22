using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Models
{
    public class VisitedLink
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public int VisitCount { get; set; }
    }
}
