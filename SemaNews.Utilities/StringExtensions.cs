using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SemaNews.Utilities
{
    public static class StringExtensions
    {
        public static string NormalizeSpace(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            var result = Regex.Replace(s.Trim(), "\\s{2,}", " ");
            return result;
        }

        public static string ParseListToString(this ICollection<string> listStr, string left, string right,string seperator)
        {
            if (listStr == null || listStr.Count == 0)
                return string.Empty;

            var list = listStr.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i])) continue;
                list[i] = string.Format("{0}{1}{2}", left, list[i], right);
            }

            var result = string.Join(seperator, list);
            return result;
        }
    }
}
