using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.Helpers
{
    public class TreeNode
    {
        public int id { get; set; }
        public string text { get; set; }
        public ICollection<TreeNode> nodes { get; set; }
        public string icon
        {
            get
            {
                return "glyphicon";
            }
        }
        public string href { get; set; }

        public string[] tags
        {
            get
            {
                if (nodes != null && nodes.Count != 0)
                    return new string[] {nodes.Count.ToString()};
                return null;
            }
        }

        public TreeNode()
        {
            text = "";
            nodes = new List<TreeNode>();
        }
    }
}