using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class Projection
    {
        static List<List<AlphaEntity>> alphaList;
        static List<List<float?>> betaList;
        List<AlphaEntity> anAlpha;
        List<float?> anBeta;

        public void ResetData()
        {
            alphaList = new List<List<AlphaEntity>>();
            betaList = new List<List<float?>>();
            anAlpha = new List<AlphaEntity>();
            anBeta = new List<float?>();
        }
        //Tim do tuong quan giua 2 do thi
        public double FindRelevance(GraphEntity H, GraphEntity G)
        {
            double maxRelevance = 0;
            double relevance;
            List<GraphEntity> subGraphHList = SubGraphs.FindSubGraphs(H);
            foreach (GraphEntity subH in subGraphHList)
            {
                ResetData();
                FindProjection(subH, G);
                relevance = ComputeRelevanceOfSubHGraph(subH, G);
                if (maxRelevance < relevance)
                    maxRelevance = relevance;
            }
            return maxRelevance;
        }

        //Tim phep chieu giua 2 do thi H va G. Voi H la 1 do thi con cua do thi can tim do tuong quan
        public void FindProjection(GraphEntity subH, GraphEntity G)
        {
            if (subH.relationList.Count <= G.relationList.Count)
            {
                int numKeyphInH = subH.isolateKeyphraseList.Union(subH.keyphraseList).Count();
                int numKeyphInG = G.isolateKeyphraseList.Union(G.keyphraseList).Count();
                if (numKeyphInH <= numKeyphInG)
                {
                    FindRelationProjection(subH, G);
                }
            }
        }//end FindProjection

        public void FindRelationProjection(GraphEntity H, GraphEntity G)
        {
            if (H.relationList.Count == 0)
            {
                List<KeyphraseEntity> keyphraseInRelationG = GetKeyphraseFromRelation(G.relationList);
                List<KeyphraseEntity> keyphraseInG = G.isolateKeyphraseList.Union(keyphraseInRelationG).ToList();
                FindIsolateProjection(H.isolateKeyphraseList, keyphraseInG);
                return;
            }
            else
            {
                for (int i = 0; i < H.relationList.Count; i++)
                {
                    List<RelatedRelationEntity> relatedRelation1 = (from g in G.relationList
                                                                    where H.relationList[i].idRelation == g.idRelation
                                                                    select new RelatedRelationEntity
                                                                    {
                                                                        relation = g,
                                                                        value = 1
                                                                    }).ToList();
                    List<RelatedRelationEntity> relatedRelation2 = (from g in G.relationList
                                                                    let x = SemanticSearch.BetaDatabase.FirstOrDefault(b => b.FirstRelationId == g.idRelation && b.SecondRelationId == H.relationList[i].idRelation)
                                                                    where x != null
                                                                    select new RelatedRelationEntity
                                                                    {
                                                                        relation = g,
                                                                        value = x.RelevanceValue
                                                                    }).ToList();
                    List<RelatedRelationEntity> relatedRelation = relatedRelation1.Union(relatedRelation2).ToList();
                    for (int j = 0; j < relatedRelation.Count; j++)
                    {
                        AlphaEntity alpha1;
                        if (H.relationList[i].keyphrase1.id == relatedRelation[j].relation.keyphrase1.id)
                        {
                            alpha1 = new AlphaEntity
                            {
                                idKeyphInH = H.relationList[i].keyphrase1.id,
                                idKeyphInG = relatedRelation[j].relation.keyphrase1.id,
                                valueAlpha = 1,
                                tfKeyphInG = relatedRelation[j].relation.keyphrase1.tf,
                                ipKeyphInG = relatedRelation[j].relation.keyphrase1.ip
                            };
                        }
                        else
                        {
                            alpha1 = (from a in SemanticSearch.AlphaDatabase
                                      where (a.FirstKeyphraseId == H.relationList[i].keyphrase1.id && a.SecondKeyphraseId == relatedRelation[j].relation.keyphrase1.id)
                                         || (a.SecondKeyphraseId == H.relationList[i].keyphrase1.id && a.FirstKeyphraseId == relatedRelation[j].relation.keyphrase1.id)
                                      select new AlphaEntity
                                      {
                                          idKeyphInH = H.relationList[i].keyphrase1.id,
                                          idKeyphInG = relatedRelation[j].relation.keyphrase1.id,
                                          valueAlpha = a.AlphaValue,
                                          tfKeyphInG = relatedRelation[j].relation.keyphrase1.tf,
                                          ipKeyphInG = relatedRelation[j].relation.keyphrase1.ip
                                      }).FirstOrDefault();
                        }
                        AlphaEntity alpha2;
                        if (H.relationList[i].keyphrase2.id == relatedRelation[j].relation.keyphrase2.id)
                        {
                            alpha2 = new AlphaEntity
                            {
                                idKeyphInH = H.relationList[i].keyphrase2.id,
                                idKeyphInG = relatedRelation[j].relation.keyphrase2.id,
                                valueAlpha = 1,
                                tfKeyphInG = relatedRelation[j].relation.keyphrase2.tf,
                                ipKeyphInG = relatedRelation[j].relation.keyphrase2.ip
                            };
                        }
                        else
                        {
                            alpha2 = (from a in SemanticSearch.AlphaDatabase
                                      where (a.FirstKeyphraseId == H.relationList[i].keyphrase2.id && a.SecondKeyphraseId == relatedRelation[j].relation.keyphrase2.id)
                                         || (a.SecondKeyphraseId == H.relationList[i].keyphrase2.id && a.FirstKeyphraseId == relatedRelation[j].relation.keyphrase2.id)
                                      select new AlphaEntity
                                      {
                                          idKeyphInH = H.relationList[i].keyphrase2.id,
                                          idKeyphInG = relatedRelation[j].relation.keyphrase2.id,
                                          valueAlpha = a.AlphaValue,
                                          tfKeyphInG = relatedRelation[j].relation.keyphrase2.tf,
                                          ipKeyphInG = relatedRelation[j].relation.keyphrase2.ip
                                      }).FirstOrDefault();
                        }

                        if (alpha1 != null && alpha2 != null)
                        {
                            anAlpha.Add(new AlphaEntity { idKeyphInH = alpha1.idKeyphInH, idKeyphInG = alpha1.idKeyphInG, valueAlpha = alpha1.valueAlpha, tfKeyphInG = alpha1.tfKeyphInG, ipKeyphInG = alpha1.ipKeyphInG });
                            anAlpha.Add(new AlphaEntity { idKeyphInH = alpha1.idKeyphInH, idKeyphInG = alpha2.idKeyphInG, valueAlpha = alpha2.valueAlpha, tfKeyphInG = alpha2.tfKeyphInG, ipKeyphInG = alpha2.ipKeyphInG });
                            anBeta.Add(relatedRelation[j].value);

                            ////Xoa quan he trong H va G
                            List<RelationEntity> reduceRelationH = H.relationList.GetRange(0, i).Union(H.relationList.GetRange(i + 1, H.relationList.Count - i - 1)).ToList();
                            GraphEntity reduceH = new GraphEntity { isolateKeyphraseList = H.isolateKeyphraseList, relationList = reduceRelationH };
                            List<RelationEntity> reduceRelationG = G.relationList.Where(r =>
                                r.idRelation != relatedRelation[j].relation.idRelation || r.keyphrase1.id != relatedRelation[j].relation.keyphrase1.id
                                || r.keyphrase2.id != relatedRelation[j].relation.keyphrase2.id).ToList();
                            GraphEntity reduceG = new GraphEntity { isolateKeyphraseList = G.isolateKeyphraseList, relationList = reduceRelationG };

                            FindRelationProjection(reduceH, reduceG);
                            anAlpha.RemoveRange(anAlpha.Count - 2, 2);
                            anBeta.RemoveAt(anBeta.Count - 1);
                        }
                    }
                }//end duyet tap quan he
            }
        }//end FindProjection

        void FindIsolateProjection(List<KeyphraseEntity> isolateKeyphInH, List<KeyphraseEntity> keyphraseInG)
        {
            if (isolateKeyphInH.Count == 0)
            {
                if (CheckExistAlpha() == 0)
                {
                    List<AlphaEntity> alphaTemp = new List<AlphaEntity>();
                    alphaTemp.AddRange(anAlpha);
                    alphaList.Add(alphaTemp);
                    List<float?> betaTemp = new List<float?>();
                    betaTemp.AddRange(anBeta);
                    betaList.Add(betaTemp);
                }
                return;
            }
            for (int i = 0; i < isolateKeyphInH.Count; i++)
            {
                List<RelatedKeyphraseEntity> relatedKeyphaseInG = FindKeyphraseInGWithPositiveAlpha(isolateKeyphInH[i], keyphraseInG);
                for (int j = 0; j < relatedKeyphaseInG.Count; j++)
                {
                    anAlpha.Add(new AlphaEntity { idKeyphInH = isolateKeyphInH[i].id, idKeyphInG = relatedKeyphaseInG[j].k.id, valueAlpha = relatedKeyphaseInG[j].value, tfKeyphInG = relatedKeyphaseInG[j].k.tf, ipKeyphInG = relatedKeyphaseInG[j].k.ip });

                    //Xoa keyphrase trong H va G
                    List<KeyphraseEntity> reduceIsolateKeyphInH = isolateKeyphInH.GetRange(0, i).Union(isolateKeyphInH.GetRange(i + 1, isolateKeyphInH.Count - i - 1)).ToList();
                    List<KeyphraseEntity> reduceKeyphInG = keyphraseInG.Where(k => k.id != relatedKeyphaseInG[j].k.id).ToList();

                    FindIsolateProjection(reduceIsolateKeyphInH, reduceKeyphInG);
                    anAlpha.RemoveAt(anAlpha.Count - 1);
                }
            }//end duyet tap dinh co lap
        }

        //Tim cac keyphrase trong quan he
        List<KeyphraseEntity> GetKeyphraseFromRelation(List<RelationEntity> relationList)
        {
            List<KeyphraseEntity> result = new List<KeyphraseEntity>();
            foreach (RelationEntity aRelation in relationList)
            {
                KeyphraseEntity k1 = result.Where(k => k.id == aRelation.keyphrase1.id).Select(t => t).FirstOrDefault();
                KeyphraseEntity k2 = result.Where(k => k.id == aRelation.keyphrase2.id).Select(t => t).FirstOrDefault();
                if (k1 == null)
                    result.Add(aRelation.keyphrase1);
                if (k2 == null)
                    result.Add(aRelation.keyphrase2);
            }
            return result;
        }

        //Tim tat ca cac keyphrase trong G (keyphraseInG) sao cho alpha(isolateH,keyphraseInG)>0
        //isIsolation: 0->xet cac keyphrase khong co lap trong G, 1-> xet cac keyphrase co lap trong G
        List<RelatedKeyphraseEntity> FindKeyphraseInGWithPositiveAlpha(KeyphraseEntity isolateH, List<KeyphraseEntity> keyphraseInG)
        {
            List<RelatedKeyphraseEntity> result = new List<RelatedKeyphraseEntity>();
            float? valueAlpha;
            //AAAAAAA : Neu keyphraseInG khong chua isolateH thi khong thuc hien nua
            List<KeyphraseEntity> keyphraseInG_Contain_isolateH = keyphraseInG.Where(k => (String.Compare(k.keyphrase, isolateH.keyphrase) == 0)).ToList();
            foreach (KeyphraseEntity kph in keyphraseInG_Contain_isolateH)
            {
                valueAlpha = GetAlpha(isolateH, kph);
                if (valueAlpha != null)
                     result.Add(new RelatedKeyphraseEntity { k = kph, value = valueAlpha });
            }
            //BBBBBBBBBB: tim nhung keyphraseInG co quan he [đồng nghĩa, gần nghĩa, viết tắt]  voi isolateH
            foreach (KeyphraseEntity kph in keyphraseInG)
            {
                valueAlpha = GetAlphaSynonym(isolateH, kph);
                if (valueAlpha != null)
                    result.Add(new RelatedKeyphraseEntity { k = kph, value = valueAlpha });
            }
            ///Day la doan code chinh thuc. Nhung vi ket qua khong chinh xac lam nen hien tai dung doan code AAAAAAA và BBBBBBBBBB tren
            ///
            //foreach (KeyphraseEntity kph in keyphraseInG)
            //{
            //    valueAlpha = GetAlpha(isolateH, kph);
            //    if (valueAlpha != null)
            //        result.Add(new RelatedKeyphraseEntity { k = kph, value = valueAlpha });
            //}
            return result;
        }

        //Tim nhung keyphrase thuoc cac quan he [đồng nghĩa, gần nghĩa, viết tắt]
        float? GetAlphaSynonym(KeyphraseEntity k1, KeyphraseEntity k2)
        {
            if (k1.id == k2.id)
                return 1;
            float? alpha = (from a in SemanticSearch.AlphaDatabase
                            where (a.FirstKeyphraseId == k1.id && a.SecondKeyphraseId == k2.id && a.AlphaValue >= 0.9) || (a.SecondKeyphraseId == k1.id && a.FirstKeyphraseId == k2.id && a.AlphaValue >= 0.9)
                            select a.AlphaValue).FirstOrDefault();
            return alpha;
        }

        float? GetAlpha(KeyphraseEntity k1, KeyphraseEntity k2)
        {
            if (k1.id == k2.id)
                return 1;
            float? alpha = (from a in SemanticSearch.AlphaDatabase
                            where (a.FirstKeyphraseId == k1.id && a.SecondKeyphraseId == k2.id) || (a.SecondKeyphraseId == k1.id && a.FirstKeyphraseId == k2.id)
                            select a.AlphaValue).FirstOrDefault();
            return alpha;
        }

        //=1: ton tai roi, =0: chua ton tai
        int CheckExistAlpha()
        {
            if (alphaList.Count > 0)
            {
                List<AlphaEntity> ixistedAlpha = alphaList[0];
                if (ixistedAlpha.Count != anAlpha.Count)
                    return 0;
                for (int i = 0; i < ixistedAlpha.Count; i++)
                {
                    int flag = 0;
                    for (int j = 0; j < anAlpha.Count; j++)
                    {
                        //kiem tra neu 1 thang anAlpha da ton tai trong alpha thi tiep tuc
                        if (anAlpha[j].idKeyphInH == ixistedAlpha[i].idKeyphInH && anAlpha[j].idKeyphInG == ixistedAlpha[i].idKeyphInG)
                        {
                            flag = 1;
                            break;
                        }
                    }
                    if (flag == 0)
                        return 0;
                }
                return 1;
            }
            return 0;
        }//end CheckExistAlpha

        //Tinh do tuong quan cua 1 do thi con voi do thi G
        double ComputeRelevanceOfSubHGraph(GraphEntity subH, GraphEntity G)
        {
            double maxRelevance = 0;
            double anRelevanceValue;
            double alpha = 0;
            long num = subH.isolateKeyphraseList.Count + subH.keyphraseList.Count + subH.relationList.Count;
            if (alphaList.Count > 0)
            {
                for (int i = 0; i < alphaList.Count; i++)
                {
                    anRelevanceValue = 0;
                    alpha = 0;
                    foreach (AlphaEntity value in alphaList[i])
                    {
                        //alpha = value.valueAlpha != null ? Convert.ToDouble(value.valueAlpha) : 0;
                        //anRelevanceValue += (value.tfKeyphInG * value.ipKeyphInG * alpha);

                        alpha = value.valueAlpha != null ? Convert.ToDouble(value.valueAlpha) : 0;
                        anRelevanceValue += alpha;
                    }
                    foreach (float? beta in betaList[i])
                    {
                        anRelevanceValue += beta != null ? Convert.ToDouble(beta) : 0;
                    }
                    anRelevanceValue = anRelevanceValue / num;
                    if (maxRelevance < anRelevanceValue)
                        maxRelevance = anRelevanceValue;
                }
            }
            return maxRelevance;
        }//end ComputeRelevanceOfSubHGraph
    }
}
