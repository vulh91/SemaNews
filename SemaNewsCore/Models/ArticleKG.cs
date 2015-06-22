using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class ArticleKG
    {
        public int Id { get; set; }
        public string LDVL_Graph { get; set; }
        public string DT_Graph { get; set; }
        public virtual Article Article { get; set; }
    }
}
