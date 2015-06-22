using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class HTMLHelper
    {
        public static string NormalizeStr4Search(string sourceCode, bool change_AV = false)
        {
            if (string.IsNullOrEmpty(sourceCode)) return string.Empty;
            sourceCode = Regex.Replace(sourceCode, @"<[\w/]+.*?>", " ");
            sourceCode = Regex.Replace(sourceCode, @"[^\w\s]", " ");
            sourceCode = Regex.Replace(sourceCode, @"\s{2,}", " ");
            if (change_AV)
                return Change_AV(sourceCode.Trim()).ToUpper();
            else
                return sourceCode.Trim().ToUpper();
        }
        public static string Change_AV(string ip_str_change)
        {
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = ip_str_change.Normalize(NormalizationForm.FormD);
            return v_reg_regex.Replace(v_str_FormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static List<String> GetSentenceList(string _content, string _title, string _abstract)
        {
            String[] contents = _content.Split(new string[] { "...", ".", "!", "?" }, StringSplitOptions.None);
            String[] titles = _title.Split(new string[] { "...", ".", "!", "?" }, StringSplitOptions.None);
            String[] abstracts = _abstract.Split(new string[] { "...", ".", "!", "?" }, StringSplitOptions.None);
            List<String> sentenceList = new List<String>();
            sentenceList.AddRange(contents);
            sentenceList.AddRange(titles);
            sentenceList.AddRange(abstracts);
            return sentenceList;
        }
    }
}
