using SearchLibrary.DatabaseModel;
using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class KeyphraseUtility
    {
        static CKONTOLOGYEntities dbOntology;
        static DTC_DTNN_CKONTOLOGYEntities DTC_DTNN_dbOntology;
        public static List<KeyphraseEntity> GetKeyphraseFromArticleKG(string articleKG)
        {
            List<KeyphraseEntity> keyphraseList = new List<KeyphraseEntity>();
            if (String.IsNullOrEmpty(articleKG))
                return keyphraseList;

            String[] elementList = articleKG.Split(new String[] { "Keyphrases", "Relations" }, StringSplitOptions.None);
            String strKeyphrase = elementList[1];
            MatchCollection matches = Regex.Matches(strKeyphrase, @"\(([^)]*)\)");
            string[] values;
            foreach (Match m in matches)
            {
                values = m.Value.Trim('(', ')').Split('_');
                keyphraseList.Add(new KeyphraseEntity
                {
                    id = Convert.ToInt64(values[0]),
                    keyphrase = values[1],
                    tf = Convert.ToDouble(values[2]),
                    ip = Convert.ToDouble(values[3])
                });
            }
            return keyphraseList;
        }

        public static List<Keyphrase> GetKeyphraseFromDatabase()
        {
            List<Keyphrase> keyphraseList = new List<Keyphrase>();
            using (dbOntology = new CKONTOLOGYEntities())
            {
                keyphraseList = dbOntology.Keyphrases.ToList();
            }
            return keyphraseList;
        }

        public static List<Keyphrase> GetKeyphraseFromDatabase_DTCDTNN()
        {
            List<Keyphrase> keyphraseList = new List<Keyphrase>();
            using (DTC_DTNN_dbOntology = new DTC_DTNN_CKONTOLOGYEntities())
            {
                keyphraseList = DTC_DTNN_dbOntology.Keyphrases.ToList();
            }
            return keyphraseList;
        }
    }
}
