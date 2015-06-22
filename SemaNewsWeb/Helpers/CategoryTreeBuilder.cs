using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace SemaNewsWeb.Helpers
{
    public static class CategoryTreeBuilder
    {
        public static string BuildGFTree()
        {
            using (SemaNewsDBContext context = new SemaNewsDBContext())
            {
                string source = string.Empty;
                var rootGFs = context.GFields.Where(m => m.GGRelationInstancesIn.Where(r => r.GGRelation.Notation == SemaNewsCore.NRelationNotation.PARENT.ToString()).Count() == 0).OrderBy(m => m.Name).ToList();

                foreach (var item in rootGFs)
                {
                    source += BuildGFTree(item, context);
                }
                return source;
            }
        }

        public static string BuildGFTree(GField gf, SemaNewsDBContext context)
        {
            string htmlString = string.Empty;
            var childGFs = gf.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == SemaNewsCore.NRelationNotation.PARENT.ToString()).Select(r => r.GField2).ToList();

            if (childGFs == null || childGFs.Count() == 0)
                return string.Format("<li><a href=\"/News/Category/{0}\">{1}</a></li>", gf.Id, gf.Name);
            else
            {
                htmlString += string.Format("<li class=\"dropdown-submenu\"><a href=\"/News/Category/{0}\">{1}</a>", gf.Id, gf.Name);
                htmlString += string.Format("<ul class=\"dropdown-menu\">");
                foreach (var child in childGFs)
                {
                    htmlString += BuildGFTree(child, context);
                }
                htmlString += "</ul></li>";
                return htmlString;
            }
        }

        public static TreeView BuildTreeViewForCategory()
        {
            TreeView tree = new TreeView();
            using (SemaNewsDBContext context = new SemaNewsDBContext())
            {
                var rootGFs = context.GFields.Where(m => m.GGRelationInstancesIn.Where(r => r.GGRelation.Notation == SemaNewsCore.NRelationNotation.PARENT.ToString()).Count() == 0).OrderBy(m => m.Name).ToList();

                foreach (var item in rootGFs)
                {
                    tree.nodes.Add(BuildTreeViewForCategory(item, context));
                }
                return tree;
            }
        }

        public static TreeNode BuildTreeViewForCategory(GField gf, SemaNewsDBContext context)
        {
            if (gf == null || context == null)
                throw new ArgumentNullException();

            TreeNode node = new TreeNode();
            node.text = gf.Name;
            node.id = gf.Id;
            node.href = "/Category/Edit/" + gf.Id;
            var childGFs = gf.GGRelationInstancesOut.Where(m => m.GGRelation.Notation == SemaNewsCore.NRelationNotation.PARENT.ToString()).Select(r => r.GField2).ToList();
            if (childGFs == null || childGFs.Count == 0)
            {
                node.nodes = null;
                return node;
            }

            foreach (var item in childGFs)
            {
                node.nodes.Add(BuildTreeViewForCategory(item, context));
            }

            return node;
        }
    }
}