using SearchLibrary.DatabaseModel;
using SearchLibrary.DTCDTNN;
using SearchLibrary.EntityModel;
using SearchLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class SemanticSearch_DTCDTNN
    {
        static DTC_DTNN_CKONTOLOGYEntities dbCKOntology;

        static List<Keyphrase> Keyphrase = new List<Keyphrase>();
        static List<Keyphrase_KeyphraseRelationship> KKRelationship = new List<Keyphrase_KeyphraseRelationship>();
        static List<KeyphraseRelationship> KRelationship = new List<KeyphraseRelationship>();
        static List<Alpha> Alphas = new List<Alpha>();
        static List<Beta> Betas = new List<Beta>();

        public static void InitialData()
        {
            using (dbCKOntology = new DTC_DTNN_CKONTOLOGYEntities())
            {
                Keyphrase = (from k in dbCKOntology.Keyphrases select k).ToList();
                KKRelationship = (from k in dbCKOntology.Keyphrase_KeyphraseRelationship select k).ToList();
                KRelationship = (from k in dbCKOntology.KeyphraseRelationships select k).ToList();
                Alphas = dbCKOntology.Alphas.ToList();
                Betas = dbCKOntology.Betas.ToList();
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
            Projection_DTCDTNN projection = new Projection_DTCDTNN();
            rank = projection.FindRelevance(queryKG, graphG);
            return rank;
        }

        public static double? GetRank(GraphEntity queryKG, GraphEntity articleKG)
        {
            double rank = 0;
            Projection_DTCDTNN projection = new Projection_DTCDTNN();
            rank = projection.FindRelevance(queryKG, articleKG);
            return rank;
        }
    }
}
