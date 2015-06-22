using SearchLibrary.DatabaseModel;
using SearchLibrary.EntityModel;
using SearchLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class SearchByTopic
    {
        static CKONTOLOGYEntities dbCKOntology;
        static List<Alpha> Alphas = new List<Alpha>();
        
        public static List<Alpha> AlphaDatabase
        {
            get
            {
                return Alphas;
            }
        }
        public static void InitialData()
        {
            if (SemanticSearch.AlphaDatabase.Count > 0)
                Alphas = SemanticSearch.AlphaDatabase;
            else
            {
                using (dbCKOntology = new CKONTOLOGYEntities())
                {
                    Alphas = dbCKOntology.Alphas.ToList();
                }
            }
        }
        public static double GetRankForTopic(List<long> keyphraseIdFromTopic, string articleKG)
        {
            double rank = 0;

            List<KeyphraseEntity> keyphraseIdFromArticle = KeyphraseUtility.GetKeyphraseFromArticleKG(articleKG);
            if (keyphraseIdFromArticle.Count <= 0)
                return rank;
            double max, relevanceKK, weight = 0;
            foreach (long idKeyphraseTopic in keyphraseIdFromTopic)
            {
                max = 0;
                foreach (KeyphraseEntity keyphrase in keyphraseIdFromArticle)
                {
                    Alpha alphaObject = (from a in AlphaDatabase
                                       where (a.FirstKeyphraseId == idKeyphraseTopic && a.SecondKeyphraseId == keyphrase.id)
                                          || (a.FirstKeyphraseId == keyphrase.id && a.SecondKeyphraseId == idKeyphraseTopic)
                                       select a).FirstOrDefault();
                    if (alphaObject != null)
                    {
                        ///Day la ham su dung trong cong thuc
                        //relevanceKK = keyphrase.tf * Convert.ToDouble(alphaObject.AlphaValue) * keyphrase.ip;
                        ///
                        relevanceKK = Convert.ToDouble(alphaObject.AlphaValue);
                        if (max < relevanceKK)
                            max = relevanceKK;
                    }
                }
                weight += max;
            }
            rank = weight / keyphraseIdFromTopic.Count;
            return rank;
        }
        public static List<long> GetKeyphraseIDFromTopic(string topics)
        {
            List<long> IdKeyphrase = new List<long>();
            MatchCollection matches = Regex.Matches(topics, @"\(([^)]*)\)");
            string[] values;
            foreach (Match m in matches)
            {
                values = m.Value.Trim('(', ')').Split(',');
                IdKeyphrase.Add(Convert.ToInt64(values[0]));
            }
            return IdKeyphrase;
        }
        
    }
}
