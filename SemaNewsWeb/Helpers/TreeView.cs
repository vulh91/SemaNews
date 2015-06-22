using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.Helpers
{
    public class TreeView
    {
        public ICollection<TreeNode> nodes { get; set; }
        public TreeView()
        {
            nodes = new List<TreeNode>();
        }
    }
}