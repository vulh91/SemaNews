using SearchLibrary.DatabaseModel;
using SearchLibrary.DTCDTNN;
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
    public class BuildKeyphraseGraph
    {
        //Dung lai do thi keyphrase cho 1 do thi lay tu database
        public GraphEntity BuildAnKeyphraseGraph(String strGraph)
        {
            String[] elementList = strGraph.Split(new String[] { "Keyphrases", "Relations" }, StringSplitOptions.None);

            List<KeyphraseEntity> isolateKeyphraseList = new List<KeyphraseEntity>(); //chua cac keyphrase co lap
            List<KeyphraseEntity> keyphraseList = new List<KeyphraseEntity>(); //chua cac keyphrase khong phai keyphrase co lap
            String strKeyphrase = elementList[1];
            MatchCollection matches = Regex.Matches(strKeyphrase, @"\(([^)]*)\)");
            string[] valueList;
            foreach (Match m in matches)
            {
                valueList = m.Value.Trim('(', ')').Split('_');
                if (valueList[4] == "1") //la keyphrase co lap
                {
                    isolateKeyphraseList.Add(new KeyphraseEntity
                    {
                        id = Convert.ToInt64(valueList[0]),
                        keyphrase = valueList[1],
                        tf = Convert.ToDouble(valueList[2]),
                        ip = Convert.ToDouble(valueList[3])
                    });
                }
                else //khong phai keyphrase co lap
                {
                    keyphraseList.Add(new KeyphraseEntity
                    {
                        id = Convert.ToInt64(valueList[0]),
                        keyphrase = valueList[1],
                        tf = Convert.ToDouble(valueList[2]),
                        ip = Convert.ToDouble(valueList[3])
                    });
                }
            }

            List<RelationEntity> relationList = new List<RelationEntity>();
            KeyphraseEntity k1;
            KeyphraseEntity k2;
            String strRelation = elementList[2];
            matches = Regex.Matches(strRelation, @"\(([^)]*)\)");
            foreach (Match m in matches)
            {
                valueList = m.Value.Trim('(', ')').Split(',');
                k1 = keyphraseList.Where(t => String.Compare(t.keyphrase, valueList[0]) == 0).FirstOrDefault();
                k2 = keyphraseList.Where(t => String.Compare(t.keyphrase, valueList[1]) == 0).FirstOrDefault();
                relationList.Add(new RelationEntity
                {
                    keyphrase1 = k1,
                    keyphrase2 = k2,
                    nameRelation = valueList[2],
                    idRelation = Convert.ToInt64(valueList[3])
                });
            }

            GraphEntity graph = new GraphEntity();
            graph.keyphraseList = keyphraseList;
            graph.isolateKeyphraseList = isolateKeyphraseList;
            graph.relationList = relationList;
            return graph;
        }

        //Xay dung lai do thi keyphrase cho 1 cau truy van - Linh vuc Lao dong-Viec lam
        public GraphEntity BuildKeyphraseGraphForQuery(String query)
        {
            List<NameEntity> keyphraseList = ExtractKeyphrase.GetKeyphraseForParagraph(query);
            if (keyphraseList == null)
                return null;
            Int64 numKeyphraseList = keyphraseList.Count();
            
            //Lay quan he giua cac keyphrase trong danh sach keyphraseList
            List<KKRelationEntity> tempRelationList = new List<KKRelationEntity>();

            for (Int32 i = 0; i < numKeyphraseList - 1; i++)
            {
                for (Int32 j = i + 1; j < numKeyphraseList; j++)
                {
                    KKRelationEntity relation1 = (from kk in SemanticSearch.KKRelationships
                                                  where kk.SourceId == keyphraseList[i].Id && kk.TargetId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[i].Name,
                                                      Keyphrase2 = keyphraseList[j].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    KKRelationEntity relation2 = (from kk in SemanticSearch.KKRelationships
                                                  where kk.TargetId == keyphraseList[i].Id && kk.SourceId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[j].Name,
                                                      Keyphrase2 = keyphraseList[i].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    string name = String.Empty;
                    if (relation1 != null)
                    {
                        name = (from k in SemanticSearch.KeyphraseRelationships
                                where k.Id == relation1.idrelation
                                select k.Name).FirstOrDefault();
                        relation1.NameRelation = name;
                        tempRelationList.Add(relation1);
                    }
                    if (relation2 != null)
                    {
                        if (String.IsNullOrEmpty(name))
                            name = (from k in SemanticSearch.KeyphraseRelationships
                                    where k.Id == relation2.idrelation
                                    select k.Name).FirstOrDefault();
                        relation2.NameRelation = name;
                        tempRelationList.Add(relation2);
                    }
                }

            }

            List<KeyphraseEntity> notIsolateKeyphraseList = new List<KeyphraseEntity>();
            List<KeyphraseEntity> isolateKeyphraseList = new List<KeyphraseEntity>();
            List<RelationEntity> relationList = new List<RelationEntity>();
            foreach (NameEntity aKeyphrase in keyphraseList)
            {
                var existKeyphrase = tempRelationList.Where(r => (String.Compare(r.Keyphrase1, aKeyphrase.Name) == 0 ) || (String.Compare(r.Keyphrase2, aKeyphrase.Name) == 0)).FirstOrDefault();
                if (existKeyphrase!=null) //keyphrase khong co lap
                    notIsolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
                else //keyphrase co lap
                    isolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
            }
            //Tao entity quan he
            foreach (KKRelationEntity anRelation in tempRelationList)
            {
                KeyphraseEntity k1 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase1) == 0).FirstOrDefault();
                KeyphraseEntity k2 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase2) == 0).FirstOrDefault();
                if (k1 != null && k2 != null)
                    relationList.Add(new RelationEntity { idRelation = anRelation.idrelation, keyphrase1 = k1, keyphrase2 = k2, nameRelation = anRelation.NameRelation });
            }

            GraphEntity graph = null;
            if (isolateKeyphraseList.Count != 0 || notIsolateKeyphraseList.Count != 0)
            {
                graph = new GraphEntity { isolateKeyphraseList = isolateKeyphraseList, keyphraseList = notIsolateKeyphraseList, relationList = relationList };
            }
            return graph;
        }

        /// <summary>
        /// Xay dung lai do thi keyphrase cho 1 cau truy van - Linh vuc Dau tu cong - Dau tu nuoc ngoai
        /// Y tuong la duyet keyphrase trong CKOntology de tim keyphrase cho tin bai
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public GraphEntity BuildKeyphraseGraphForQuery_TraverseOntology_DTCDTNN(String query)
        {
            List<NameEntity> keyphraseList = new List<NameEntity>();
            foreach (Keyphrase k in SemanticSearch_DTCDTNN.Keyphrases)
            {
                if (Regex.Matches(query, k.Name).Count > 0)
                    keyphraseList.Add(new NameEntity { Id = k.Id, Name = k.Name });
            }
            if (keyphraseList.Count <= 0)
                return null;
            Int64 numKeyphraseList = keyphraseList.Count();

            //Lay quan he giua cac keyphrase trong danh sach keyphraseList
            List<KKRelationEntity> tempRelationList = new List<KKRelationEntity>();

            for (Int32 i = 0; i < numKeyphraseList - 1; i++)
            {
                for (Int32 j = i + 1; j < numKeyphraseList; j++)
                {
                    KKRelationEntity relation1 = (from kk in SemanticSearch.KKRelationships
                                                  where kk.SourceId == keyphraseList[i].Id && kk.TargetId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[i].Name,
                                                      Keyphrase2 = keyphraseList[j].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    KKRelationEntity relation2 = (from kk in SemanticSearch.KKRelationships
                                                  where kk.TargetId == keyphraseList[i].Id && kk.SourceId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[j].Name,
                                                      Keyphrase2 = keyphraseList[i].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    string name = String.Empty;
                    if (relation1 != null)
                    {
                        name = (from k in SemanticSearch.KeyphraseRelationships
                                where k.Id == relation1.idrelation
                                select k.Name).FirstOrDefault();
                        relation1.NameRelation = name;
                        tempRelationList.Add(relation1);
                    }
                    if (relation2 != null)
                    {
                        if (String.IsNullOrEmpty(name))
                            name = (from k in SemanticSearch.KeyphraseRelationships
                                    where k.Id == relation2.idrelation
                                    select k.Name).FirstOrDefault();
                        relation2.NameRelation = name;
                        tempRelationList.Add(relation2);
                    }
                }

            }

            List<KeyphraseEntity> notIsolateKeyphraseList = new List<KeyphraseEntity>();
            List<KeyphraseEntity> isolateKeyphraseList = new List<KeyphraseEntity>();
            List<RelationEntity> relationList = new List<RelationEntity>();
            foreach (NameEntity aKeyphrase in keyphraseList)
            {
                var existKeyphrase = tempRelationList.Where(r => (String.Compare(r.Keyphrase1, aKeyphrase.Name) == 0) || (String.Compare(r.Keyphrase2, aKeyphrase.Name) == 0)).FirstOrDefault();
                if (existKeyphrase != null) //keyphrase khong co lap
                    notIsolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
                else //keyphrase co lap
                    isolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
            }
            //Tao entity quan he
            foreach (KKRelationEntity anRelation in tempRelationList)
            {
                KeyphraseEntity k1 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase1) == 0).FirstOrDefault();
                KeyphraseEntity k2 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase2) == 0).FirstOrDefault();
                if (k1 != null && k2 != null)
                    relationList.Add(new RelationEntity { idRelation = anRelation.idrelation, keyphrase1 = k1, keyphrase2 = k2, nameRelation = anRelation.NameRelation });
            }

            GraphEntity graph = null;
            if (isolateKeyphraseList.Count != 0 || notIsolateKeyphraseList.Count != 0)
            {
                graph = new GraphEntity { isolateKeyphraseList = isolateKeyphraseList, keyphraseList = notIsolateKeyphraseList, relationList = relationList };
            }
            return graph;
        }

        /// <summary>
        /// Xay dung lai do thi keyphrase cho 1 cau truy van - Linh vuc Dau tu cong - Dau tu nuoc ngoai
        /// Y tuong la duyet keyphrase trong CKOntology de tim keyphrase cho tin bai
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public GraphEntity BuildKeyphraseGraphForQuery_DTCDTNN(String query)
        {
            List<NameEntity> keyphraseList = ExtractKeyphrase_DTCDTNN.GetKeyphraseForParagraph(query);
            if (keyphraseList == null)
                return null;
            Int64 numKeyphraseList = keyphraseList.Count();

            //Lay quan he giua cac keyphrase trong danh sach keyphraseList
            List<KKRelationEntity> tempRelationList = new List<KKRelationEntity>();

            for (Int32 i = 0; i < numKeyphraseList - 1; i++)
            {
                for (Int32 j = i + 1; j < numKeyphraseList; j++)
                {
                    KKRelationEntity relation1 = (from kk in SemanticSearch_DTCDTNN.KKRelationships
                                                  where kk.SourceId == keyphraseList[i].Id && kk.TargetId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[i].Name,
                                                      Keyphrase2 = keyphraseList[j].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    KKRelationEntity relation2 = (from kk in SemanticSearch_DTCDTNN.KKRelationships
                                                  where kk.TargetId == keyphraseList[i].Id && kk.SourceId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[j].Name,
                                                      Keyphrase2 = keyphraseList[i].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    string name = String.Empty;
                    if (relation1 != null)
                    {
                        name = (from k in SemanticSearch_DTCDTNN.KeyphraseRelationships
                                where k.Id == relation1.idrelation
                                select k.Name).FirstOrDefault();
                        relation1.NameRelation = name;
                        tempRelationList.Add(relation1);
                    }
                    if (relation2 != null)
                    {
                        if (String.IsNullOrEmpty(name))
                            name = (from k in SemanticSearch_DTCDTNN.KeyphraseRelationships
                                    where k.Id == relation2.idrelation
                                    select k.Name).FirstOrDefault();
                        relation2.NameRelation = name;
                        tempRelationList.Add(relation2);
                    }
                }

            }

            List<KeyphraseEntity> notIsolateKeyphraseList = new List<KeyphraseEntity>();
            List<KeyphraseEntity> isolateKeyphraseList = new List<KeyphraseEntity>();
            List<RelationEntity> relationList = new List<RelationEntity>();
            foreach (NameEntity aKeyphrase in keyphraseList)
            {
                var existKeyphrase = tempRelationList.Where(r => (String.Compare(r.Keyphrase1, aKeyphrase.Name) == 0) || (String.Compare(r.Keyphrase2, aKeyphrase.Name) == 0)).FirstOrDefault();
                if (existKeyphrase != null) //keyphrase khong co lap
                    notIsolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
                else //keyphrase co lap
                    isolateKeyphraseList.Add(new KeyphraseEntity { id = aKeyphrase.Id, keyphrase = aKeyphrase.Name });
            }
            //Tao entity quan he
            foreach (KKRelationEntity anRelation in tempRelationList)
            {
                KeyphraseEntity k1 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase1) == 0).FirstOrDefault();
                KeyphraseEntity k2 = notIsolateKeyphraseList.Where(t => String.Compare(t.keyphrase, anRelation.Keyphrase2) == 0).FirstOrDefault();
                if (k1 != null && k2 != null)
                    relationList.Add(new RelationEntity { idRelation = anRelation.idrelation, keyphrase1 = k1, keyphrase2 = k2, nameRelation = anRelation.NameRelation });
            }

            GraphEntity graph = null;
            if (isolateKeyphraseList.Count != 0 || notIsolateKeyphraseList.Count != 0)
            {
                graph = new GraphEntity { isolateKeyphraseList = isolateKeyphraseList, keyphraseList = notIsolateKeyphraseList, relationList = relationList };
            }
            return graph;
        }
    }
}
