using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNews.Utilities.Models
{
    public class NodeAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {

            return "[name = " + Name +" , Value = " + Value + "]";
        }

        public static bool MatchInList(NodeAttribute node, List<NodeAttribute> list)
        {
            foreach (NodeAttribute item in list)
            {
                if (item.Name == "fromHead" || item.Name == "fromTail")
                    continue;
                if (item.Name == node.Name)
                    return true;
            }
            return false;
        }

        public static bool ContainInList(NodeAttribute node, List<NodeAttribute> list)
        {
            foreach (NodeAttribute item in list)
            {
                if (item.Name == "fromHead" || item.Name == "fromTail")
                    continue;
                if ((item.Name == "action" || item.Name == "href" || item.Name == "id") && item.Name == node.Name)
                    return true;
                if (item.Name == node.Name && item.Value == node.Value)
                    return true;
            }
            return false;
        }
    }
}
