using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.Extensions
{
    public class SelectableItem<T>
    {
        public T Model { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }

        public SelectableItem(T data, bool isSelected = true)
        {
            this.Model = data;
            this.Selected = isSelected;
        }

        public SelectableItem(T data, string text, bool isSelected = true)
        {
            this.Model = data;
            this.Text = text;
            this.Selected = isSelected;
        }
    }
}