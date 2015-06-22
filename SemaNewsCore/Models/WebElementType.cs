using System;
using System.Collections.Generic;

namespace SemaNewsCore.Models
{
    public partial class WebElementType
    {
        public WebElementType()
        {
            this.ArticleWebElements = new List<ArticleWebElement>();
            this.FieldWebElements = new List<FieldWebElement>();
        }

        public int Id { get; set; }
        public string WENotation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ArticleWebElement> ArticleWebElements { get; set; }
        public virtual ICollection<FieldWebElement> FieldWebElements { get; set; }
    }
}
