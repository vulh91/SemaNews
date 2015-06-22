using SearchLibrary.DatabaseModel;
using SearchLibrary.EntityModel;
using SearchLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class SemanticSearch
    {
        static CKONTOLOGYEntities dbCKOntology;

        static List<POSDictionary> POSDict = new List<POSDictionary>();
        static List<PartOfSpeech> POS = new List<PartOfSpeech>();
        static List<Phrase> Phrase = new List<Phrase>();
        static List<Keyphrase> Keyphrase = new List<Keyphrase>();
        static List<Keyphrase_KeyphraseRelationship> KKRelationship = new List<Keyphrase_KeyphraseRelationship>();
        static List<KeyphraseRelationship> KRelationship = new List<KeyphraseRelationship>();
        static List<Alpha> Alphas = new List<Alpha>();
        static List<Beta> Betas = new List<Beta>();

        public static void InitialData()
        {
            using (dbCKOntology = new CKONTOLOGYEntities())
            {
                POSDict = (from pp in dbCKOntology.POSDictionaries select pp).ToList();
                POS = (from p in dbCKOntology.PartOfSpeeches select p).ToList();
                Phrase = (from p in dbCKOntology.Phrases select p).ToList();
                Keyphrase = (from k in dbCKOntology.Keyphrases select k).ToList();
                KKRelationship = (from k in dbCKOntology.Keyphrase_KeyphraseRelationship select k).ToList();
                KRelationship = (from k in dbCKOntology.KeyphraseRelationships select k).ToList();
                Alphas = dbCKOntology.Alphas.ToList();
                Betas = dbCKOntology.Betas.ToList();
            }
        }

        public static List<POSDictionary> POSDictionaries
        {
            get
            {
                return POSDict;
            }
        }
        public static List<PartOfSpeech> PartOfSpeeches
        {
            get
            {
                return POS;
            }
        }
        public static List<Phrase> Phrases
        {
            get
            {
                return Phrase;
            }
        }
        public static List<Keyphrase> Keyphrases
        {
            get
            {
                return Keyphrase;
            }
        }
        public static List<Keyphrase_KeyphraseRelationship> KKRelationships
        {
            get
            {
                return KKRelationship;
            }
        }
        public static List<KeyphraseRelationship> KeyphraseRelationships
        {
            get
            {
                return KRelationship;
            }
        }
        public static List<Alpha> AlphaDatabase
        {
            get
            {
                return Alphas;
            }
        }
        public static List<Beta> BetaDatabase
        {
            get
            {
                return Betas;
            }
        }

        /// <summary>
        /// Ham nay tinh hang cua 1 cau truy van va 1 do thi keyphrase cua 1 tin bai
        /// </summary>
        /// <param name="query"></param>
        /// <param name="articleKG"></param>
        /// <returns>
        /// neu khong xay dung duoc do thi keyphrase cho query thi ket qua tra ve la NULL
        /// Nguoc lai tra ve rank cua query va articleKG
        /// </returns>
        public static double? GetRank(string query, string articleKG)
        {
            double rank = 0;
            //Xay dung do thi keyphrase cho cau truy van
            BuildKeyphraseGraph process = new BuildKeyphraseGraph();
            GraphEntity queryKG = process.BuildKeyphraseGraphForQuery(query);
            if (queryKG == null)
                return null;

            GraphEntity graphG = process.BuildAnKeyphraseGraph(articleKG);
            Projection projection = new Projection();
            rank = projection.FindRelevance(queryKG, graphG);
            return rank;
        }

        public static double? GetRank(GraphEntity queryKG, GraphEntity articleKG)
        {
            double rank = 0;
            Projection projection = new Projection();
            rank = projection.FindRelevance(queryKG, articleKG);
            return rank;
        }
    }
}
