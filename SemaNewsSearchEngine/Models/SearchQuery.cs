using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using SemaNews.Utilities;
using SemaNewsSearchEngine.Config;

namespace SemaNewsSearchEngine.Models
{
    public class SearchQuery
    {
        public string SearchString { get; set; }

        public SearchMode SearchMode { get; set; }

        public string AllWord { get; set; }
        public List<string> ExactWords { get; set; }
        public List<string> NoneOfWords { get; set; }

        public List<string> SearchFields { get; set;}

        public List<string> Newspapers { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Topics { get; set; }

        public DateTime? TimePublishedFrom { get; set; }
        public DateTime? TimePublishedTo { get; set; }
        public DateTime? TimeCollectedFrom { get; set; }
        public DateTime? TimeCollectedTo { get; set; }

        public bool RelatedToLocalOnly { get; set; }

        public SearchQuery()
        {
            ExactWords = new List<string>();
            NoneOfWords = new List<string>();
            SearchFields = new List<string>();
            Newspapers = new List<string>();
            Categories = new List<string>();
            Topics = new List<string>();
        }

        public string ToLuceneQuery()
        {
            var normalQuery = this.ToSimpleQueryString();
            string luceneQuery = string.Empty;

            //Adding default search field
            if (SearchFields == null || SearchFields.Count == 0)
            {
                SearchFields = new List<string>() { 
                IndexField.Title,
                IndexField.Abstract,
                IndexField.Tags,
                IndexField.Text,
                };
            }

            foreach (var field in SearchFields)
                luceneQuery += string.Format("{0}:({1}) ", field, normalQuery);

            luceneQuery = luceneQuery.Trim();

            
            return luceneQuery;
        }

        //MultiFieldQueryParser.Parse(Lucene.Net.Util.Version.LUCENE_CURRENT, normalQuery, SearchFields);

        public string ToSimpleQueryString()
        {
            if (!string.IsNullOrEmpty(SearchString))
                return SearchString;
            string query = string.Empty, nonFieldQuery = string.Empty;

            query = string.Format("{0} {1} {2}",
                AllWord.NormalizeSpace(),
                ExactWords.ParseListToString("\"", "\"", " "),
                NoneOfWords.ParseListToString("-\"", "\"", " "))
                .Trim();

            return query;
        }

        public SearchQuery DeepCopy()
        {
            SearchQuery other = (SearchQuery)this.MemberwiseClone();
            return other;
        }

        /// <summary>
        /// Parse a query stirng to SearchQuery model
        /// </summary>
        /// <param name="query">query string in json format or simple query format</param>
        /// <returns></returns>
        public static SearchQuery Parse(string query)
        {
            SearchQuery parsedQuery = new SearchQuery();

            try
            {
                parsedQuery = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchQuery>(query);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return parsedQuery;
            
        }

        public static SearchQuery ParseKeyWords(string query)
        {
            SearchQuery parsedQuery = new SearchQuery();
            var pattern = @"([\w]+?):([\(\[\{])(.*?)([\)\}\]])";
            var notPattern = "(-\")(.*?)(\")";
            var exactPattern = "(\")(.*?)(\")";

            query = Regex.Replace(query, pattern, "").Trim();

            //None of words
            var noneWordMatches = Regex.Matches(query, notPattern);
            for (int i = 0; i < noneWordMatches.Count; i++ )
            {
                var noneWord = noneWordMatches[i].Groups[2].Value.Trim();
                if (string.IsNullOrEmpty(noneWord)) continue;
                if (parsedQuery.NoneOfWords.Contains(noneWord) == false)
                    parsedQuery.NoneOfWords.Add(noneWord);
            }

            //Exact words
            query = Regex.Replace(query, notPattern, "");
            var exactWordMatches = Regex.Matches(query, exactPattern);
            for (int i = 0; i < exactWordMatches.Count; i++ )
            {
                var exactWord = exactWordMatches[i].Groups[2].Value.Trim();
                if (string.IsNullOrEmpty(exactWord)) continue;
                if (parsedQuery.ExactWords.Contains(exactWord) == false)
                    parsedQuery.ExactWords.Add(exactWord);
            }

            query = Regex.Replace(query, exactPattern, "");
            parsedQuery.AllWord = query.Trim();

            //Exact words
            return parsedQuery;
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}