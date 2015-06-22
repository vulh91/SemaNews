using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class ArticleWebElement
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public Nullable<int> Group { get; set; }
        public int WebElementTypeId { get; set; }
        public int NewspaperId { get; set; }
        public virtual Newspaper Newspaper { get; set; }
        public virtual WebElementType WebElementType { get; set; }
    }
}
