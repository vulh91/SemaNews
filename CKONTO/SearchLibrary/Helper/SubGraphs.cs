using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class SubGraphs
    {
        public static List<GraphEntity> FindSubGraphs(GraphEntity gp)
        {
            List<GraphEntity> subGraphList = new List<GraphEntity>();
            //Truong hop do thi khong co quan he
            if (gp.relationList.Count <= 0)
            {
                subGraphList.Add(new GraphEntity { keyphraseList = new List<KeyphraseEntity>(), relationList = new List<RelationEntity>(), isolateKeyphraseList = gp.isolateKeyphraseList });
            }
            else
            {
                List<List<KeyphraseEntity>> isolateKeyphraseSubset = FindSubset_IsolateKeyphrase(gp.isolateKeyphraseList);
                List<List<RelationEntity>> relationSubset = FindSubset_Relation(gp.relationList);
                
                foreach (List<KeyphraseEntity> anKeyphraseList in isolateKeyphraseSubset)
                {
                    //subGraphList.Add(new GraphEntity { keyphraseList = gp.keyphraseList, relationList = new List<RelationEntity>(), isolateKeyphraseList = anKeyphraseList });
                    if (anKeyphraseList.Count>1) //lay pair 2 keyphrase co lap. Khong lay do thi chi co 1 keyphrase co lap
                        subGraphList.Add(new GraphEntity { keyphraseList = new List<KeyphraseEntity>(), relationList = new List<RelationEntity>(), isolateKeyphraseList = anKeyphraseList });
                }
                foreach (List<RelationEntity> anRelationList in relationSubset)
                {
                    //subGraphList.Add(new GraphEntity { keyphraseList = gp.keyphraseList, relationList = anRelationList, isolateKeyphraseList = new List<KeyphraseEntity>() });
                    List<KeyphraseEntity> keyphraseList = GetKeyphraseList(anRelationList);
                    subGraphList.Add(new GraphEntity { keyphraseList = keyphraseList, relationList = anRelationList, isolateKeyphraseList = new List<KeyphraseEntity>() });
                }
                if (isolateKeyphraseSubset.Count > 0 && relationSubset.Count > 0)
                {
                    foreach (List<KeyphraseEntity> anKeyphraseList in isolateKeyphraseSubset)
                    {
                        foreach (List<RelationEntity> anRelationList in relationSubset)
                        {
                            //subGraphList.Add(new GraphEntity { keyphraseList = gp.keyphraseList, relationList = anRelationList, isolateKeyphraseList = anKeyphraseList });
                            List<KeyphraseEntity> keyphraseList = GetKeyphraseList(anRelationList);
                            subGraphList.Add(new GraphEntity { keyphraseList = keyphraseList, relationList = anRelationList, isolateKeyphraseList = anKeyphraseList });
                        }
                    }
                    subGraphList.RemoveAt(subGraphList.Count - 1);
                }
            }
            return subGraphList;
        }
        public static List<List<KeyphraseEntity>> FindSubset_IsolateKeyphrase(List<KeyphraseEntity> lst)
        {
            List<List<KeyphraseEntity>> subsetList = new List<List<KeyphraseEntity>>();
            SubSet<KeyphraseEntity> subs = new SubSet<KeyphraseEntity>(lst);
            List<KeyphraseEntity> l = subs.Next();
            while (l != null)
            {
                subsetList.Add(l);
                l = subs.Next();
            }
            subsetList.RemoveAt(0);
            return subsetList;
        }
        public static List<List<RelationEntity>> FindSubset_Relation(List<RelationEntity> lst)
        {
            List<List<RelationEntity>> subsetList = new List<List<RelationEntity>>();
            SubSet<RelationEntity> subs = new SubSet<RelationEntity>(lst);
            List<RelationEntity> l = subs.Next();
            while (l != null)
            {
                subsetList.Add(l);
                l = subs.Next();
            }
            subsetList.RemoveAt(0);
            return subsetList;
        }
        //Ham lay danh sach keyphrase tu danh sach quan he
        static List<KeyphraseEntity> GetKeyphraseList(List<RelationEntity> relationList)
        {
            List<KeyphraseEntity> keyphraseList = new List<KeyphraseEntity>();
            foreach (RelationEntity anRelation in relationList)
            {
                if (!keyphraseList.Any(c => c.id == anRelation.keyphrase1.id))
                    keyphraseList.Add(anRelation.keyphrase1);
                if (!keyphraseList.Any(c => c.id == anRelation.keyphrase2.id))
                    keyphraseList.Add(anRelation.keyphrase2);
            }
            return keyphraseList;
        }
    }
}
