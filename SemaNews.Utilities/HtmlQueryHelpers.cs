using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using SemaNews.Utilities.Models;
namespace SemaNews.Utilities
{
    public class HtmlQueryHelpers
    {
        /// <summary>
        /// Get HtmlElement from simple xpath uses for field
        /// </summary>
        /// <param name="source"></param>
        /// <param name="xpath"></param>
        /// <param name="isDetail"></param>
        /// <returns></returns>
        public static HtmlNode GetSingleNodeByXpath(HtmlDocument source, string xpath, bool isDetail)
        {
            //xpath = xpath.Substring(1);
            string splitForm = @"/\w+(\[\d+\])*(\{([^\}^\{])+?\})*";
            MatchCollection matches = Regex.Matches(xpath, splitForm);
            List<string> xpathlist = new List<string>();
            foreach (Match m in matches)
                xpathlist.Add(m.Value);
            xpathlist.RemoveAll(x => x == string.Empty);
            HtmlNode currentNode = source.DocumentNode.FirstChild;
            int stopIndex = -1;

            for (int index = 0; index < xpathlist.Count; index++)
            {
                string path = xpathlist[index];
                NodeXpathInfo nodeInfo = new NodeXpathInfo();
                nodeInfo.AnalyzeXpath(path);
                if (nodeInfo.Name == "html")
                {
                    foreach (var node in source.DocumentNode.ChildNodes)
                    {
                        if (node.Name == "html")
                        {
                            currentNode = node;
                            break;
                        }
                    }
                }
                else
                {
                    int nodePos = 0;
                    bool foundNode = false;
                    List<HtmlNode> matchEle = new List<HtmlNode>();

                    foreach (var node in currentNode.ChildNodes)
                    {

                        if (node.Name == nodeInfo.Name)
                        {
                            nodePos++;
                            if (isDetail)
                            {
                                string nodeClass = node.GetAttributeValue("class", null);
                                string nodeId = node.GetAttributeValue("id", null);
                                if (nodeInfo.Id != null && nodeId == nodeInfo.Id && nodeInfo.Position == matchEle.Count + 1)
                                {
                                    currentNode = node;
                                    foundNode = true;
                                    break;
                                }
                                else if (nodeClass != null && nodeClass == nodeInfo.Class
                                            && nodeInfo.Position == matchEle.Count + 1)
                                {
                                    currentNode = node;
                                    foundNode = true;
                                    break;
                                }
                                else if (nodeClass == null && nodeInfo.Class == null
                                        && nodeInfo.Position == nodePos && nodeId == null && nodeInfo.Id == null)
                                {
                                    currentNode = node;
                                    foundNode = true;
                                    break;
                                }
                                else if ((nodeClass != null || nodeId != null) && nodeClass == nodeInfo.Class && nodeId == nodeInfo.Id)
                                {
                                    matchEle.Add(node);
                                    nodePos++;
                                }
                            }
                            else
                            {
                                if (nodeInfo.Position == nodePos)
                                {
                                    currentNode = node;
                                    foundNode = true;
                                    break;
                                }
                            }

                        }
                    }
                    if (!foundNode)
                    {
                        if (nodeInfo.Name == "tbody")
                            continue;
                        stopIndex = index;
                        //return null;
                    }
                }
            }

            return currentNode;
        }

        /// <summary>
        /// Function Determine HtmlElement of Article by attributes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="detailXpath"></param>
        /// <returns></returns>
        public static List<HtmlNode> GetNodesByXpathAttr(HtmlDocument source, string detailXpath)
        {
            List<HtmlNode> result = new List<HtmlNode>();
            string splitForm = @"/\w+(\[\d+\])*(\{([^\}^\{])+?\})*";
            MatchCollection matches = Regex.Matches(detailXpath, splitForm);
            List<string> xpathlist = new List<string>();
            foreach (Match m in matches)
                xpathlist.Add(m.Value);
            xpathlist.RemoveAll(x => x == string.Empty);
            NodeXpathInfo nodeInfo = new NodeXpathInfo();

            if (xpathlist.Count == 0)
                return null;

            nodeInfo.AnalyzeXpath(xpathlist[xpathlist.Count - 1]);

            List<HtmlNode> matchElements = (from element in source.DocumentNode.SelectNodes(@"//*").Cast<HtmlNode>() where CheckMatchAttr(element, nodeInfo) select element).ToList();

            List<HtmlNode> candidate = FindMostMatchElements(matchElements, nodeInfo);

            if (candidate.Count == 1)
            {
                return candidate;
            }
            else
            {
                foreach (var item in candidate)
                {
                    if (ExamineCandidate(item.ParentNode, xpathlist, xpathlist.Count - 2))
                        result.Add(item);
                }
                if (result.Count > 0)
                {
                    result = (result.OrderBy(x => Math.Abs(GetElementPosition(x.XPath) - nodeInfo.Position))).ToList();
                    return result;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Get the position of the element in the HtmlDocument from xpath
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private static int GetElementPosition(string xpath)
        {
            if (xpath == string.Empty)
                return 0;
            string s = "";
            for (int i = xpath.Length - 2; i >= 0; i--)
            {
                if (xpath[i] == '[')
                    break;
                s += xpath[i];
            }
            s.Reverse();
            return int.Parse(s);
        }

        /// <summary>
        /// Function Examine if the HtmlElement is match with xpath defined before
        /// </summary>
        /// <param name="item"></param>
        /// <param name="xpathlist"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static bool ExamineCandidate(HtmlNode item, List<string> xpathlist, int i)
        {
            var byPass = 0;
            for (; i >= 0; i--)
            {
                NodeXpathInfo nodeInfo = new NodeXpathInfo();
                nodeInfo.AnalyzeXpath(xpathlist[i]);
                float delta = MatchDelta(item, nodeInfo);

                if (delta == 1)
                {
                    item = item.ParentNode;
                    continue;
                }
                else
                {
                    if (nodeInfo.Name == "tbody")
                        continue;
                    else if (byPass < 3)
                        byPass++;
                    else
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Function Examine to get all element have same attr or name with element info defined before
        /// </summary>
        /// <param name="element"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        public static bool CheckMatchAttr(HtmlNode element, NodeXpathInfo nodeInfo)
        {

            if (element.Name != nodeInfo.Name)
                return false;

            List<NodeAttribute> elementAttr = GetElementAttr(element);

            foreach (var item in elementAttr)
            {
                if (!NodeAttribute.MatchInList(item, nodeInfo.Attr))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get the attributes of the HtmlElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static List<NodeAttribute> GetElementAttr(HtmlNode element)
        {
            List<NodeAttribute> elementAttr = new List<NodeAttribute>();

            foreach (var item in element.Attributes)
            {
                string name = item.Name;
                string value = item.Value;

                elementAttr.Add(new NodeAttribute { Name = name, Value = value });
            }

            return elementAttr;
        }

        /// <summary>
        /// Canculate the percent of name and attribute matching
        /// </summary>
        /// <param name="element"></param>
        /// <param name="eleInfo"></param>
        /// <returns></returns>
        private static float MatchDelta(HtmlNode element, NodeXpathInfo eleInfo)
        {
            float result = 0;

            int matchCount = 0;

            List<NodeAttribute> elementAttr = GetElementAttr(element);

            if (element.Name == eleInfo.Name)
                matchCount++;

            foreach (NodeAttribute item in elementAttr)
                if (NodeAttribute.ContainInList(item, eleInfo.Attr))
                    matchCount++;
            result = ((float)matchCount / (eleInfo.Attr.Count + 1));

            return result;
        }

        /// <summary>
        /// Return all element have MatchDelta Value max
        /// </summary>
        /// <param name="matchElements"></param>
        /// <param name="nodeInfo"></param>
        /// <returns></returns>
        private static List<HtmlNode> FindMostMatchElements(List<HtmlNode> matchElements, NodeXpathInfo nodeInfo)
        {
            List<HtmlNode> result = new List<HtmlNode>();
            float maxValue = -1;

            foreach (HtmlNode item in matchElements)
            {
                float matchDelta = MatchDelta(item, nodeInfo);
                if (matchDelta > maxValue)
                {
                    result.Clear();
                    maxValue = matchDelta;
                    result.Add(item);
                }
                else if (matchDelta == maxValue)
                    result.Add(item);
            }
            return result;
        }
    }
}