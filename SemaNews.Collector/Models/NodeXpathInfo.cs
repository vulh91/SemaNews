using SemaNews.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemaNews.Collector.Models
{
    public class NodeXpathInfo
    {
        public string Name { set; get; }
        public string Id { set; get; }
        public string Class { set; get; }
        public NodeAttribute FromHeadPos { set; get; }
        public NodeAttribute FromTailPos { set; get; }
        public int Position { set; get; }
        public List<NodeAttribute> Attr { set; get; }

        public NodeXpathInfo()
        {
            Attr = new List<NodeAttribute>();
        }

        public NodeXpathInfo(string detailXpath)
        {
            Attr = new List<NodeAttribute>();
            string splitForm = @"/\w+(\[\d+\])*(\{([^\}^\{])+?\})*";
            MatchCollection matches = Regex.Matches(detailXpath, splitForm);
            List<string> xpathlist = new List<string>();
            foreach (Match m in matches)
                xpathlist.Add(m.Value);
            xpathlist.RemoveAll(x => x == string.Empty);
            AnalyzeXpath(xpathlist[xpathlist.Count - 1]);
        }

        public void AnalyzeXpath(string xpathForNode)
        {
            int attrPos = xpathForNode.IndexOf('{');
            if (attrPos > 0)
            {
                string originPart = xpathForNode.Substring(0, attrPos);
                AnalyzeOriginalPart(originPart);
                string attrPart = xpathForNode.Substring(attrPos);
                AnalyzeAttrPart(attrPart);

            }
            else
                AnalyzeOriginalPart(xpathForNode);
        }

        private void AnalyzeAttrPart(string attrPart)
        {

            List<string> xpathSplit = Regex.Split(attrPart, @"\{|\}").ToList();
            xpathSplit.RemoveAll(x => x == string.Empty);
            foreach (string s in xpathSplit)
            {
                string[] attrSplit = s.Split('=');
                if (attrSplit.Length >= 2)
                {
                    if (attrSplit[0] == "fromHead")
                    {
                        FromHeadPos = new NodeAttribute { Name = attrSplit[0], Value = attrSplit[1] };
                        continue;
                    }
                    if (attrSplit[0] == "fromTail")
                    {
                        FromTailPos = new NodeAttribute { Name = attrSplit[0], Value = attrSplit[1] };
                        continue;
                    }
                    Attr.Add(new NodeAttribute { Name = attrSplit[0], Value = attrSplit[1] });
                }

            }

        }


        private void AnalyzeOriginalPart(string originalPart)
        {
            originalPart = originalPart.TrimStart('/');
            List<string> xpathSplit = Regex.Split(originalPart, @"\[|\]|and").ToList();
            xpathSplit.RemoveAll(x => x == string.Empty);
            Name = xpathSplit[0];

            if (xpathSplit.Count == 1)
                Position = 1;
            else if (xpathSplit.Count == 3)
            {
                int temp;
                if (int.TryParse(xpathSplit[2], out temp))
                    Position = temp;
            }


            for (int i = 1; i < xpathSplit.Count; i++)
            {
                string s = xpathSplit[i].Trim();

                int temp;
                if (int.TryParse(s, out temp))
                {
                    Position = temp;
                }
                else if (s.StartsWith("@class"))
                {
                    string[] classValSplit = s.Split('=');
                    if (classValSplit.Length == 2)
                        Class = classValSplit[1].Trim('\'', ' ');
                }
                else if (s.StartsWith("@id"))
                {
                    string[] idValSplit = s.Split('=');
                    if (idValSplit.Length == 2)
                    {
                        Id = idValSplit[1].Trim('\'', ' ');
                    }


                }

            }

            if (xpathSplit.Count == 2 && Id == null && Class == null)
            {
                int temp;
                if (int.TryParse(xpathSplit[1], out temp))
                    Position = temp;
            }
            else if (xpathSplit.Count == 2)
                Position = 1;
        }
    }
}
