using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    public class NearDuplicateArticle
    {
        #region Ham cu
        //public static double GetRelevance(List<KeyphraseEntity> keyphraseA, List<KeyphraseEntity> keyphraseB)
        //{
        //    double relevance = 0;
        //    List<IntersectTFKeyphraseEntity> keyphraseIntersects = IntersectTFKeyphrase(keyphraseA, keyphraseB);
        //    foreach (IntersectTFKeyphraseEntity tfKeyphrase in keyphraseIntersects)
        //    {
        //        relevance += tfKeyphrase.tfA / tfKeyphrase.tfB;
        //    }
        //    relevance = relevance / keyphraseA.Count;
        //    return relevance;
        //}
        //static List<IntersectTFKeyphraseEntity> IntersectTFKeyphrase(List<KeyphraseEntity> keyphraseListA, List<KeyphraseEntity> keyphraseListB)
        //{
        //    List<IntersectTFKeyphraseEntity> intersect = new List<IntersectTFKeyphraseEntity>();
        //    foreach (KeyphraseEntity keyphraseA in keyphraseListA)
        //    {
        //        foreach (KeyphraseEntity keyphraseB in keyphraseListB)
        //        {
        //            if (keyphraseA.id == keyphraseB.id)
        //            {
        //                intersect.Add(new IntersectTFKeyphraseEntity { tfA = keyphraseA.tf, tfB = keyphraseB.tf });
        //                break;
        //            }
        //        }
        //    }
        //    return intersect;
        //}
        #endregion Ham cu
        //sentenceListA va  sentenceListB da duoc chuan hoa (duoc trim(), xoa nhung ky tu du thua)
        public static double GetRelevanceByData(List<String> sentenceListA, List<String> sentenceListB)
        {
            double relevance = 0, max, similarity2string;
            long numberSentenceA = sentenceListA.Count();
            long numberSentenceB = sentenceListB.Count();
            if (numberSentenceA <= 0 || numberSentenceB <= 0)
                return relevance;
            string word = string.Empty;
            foreach (String aSentenceA in sentenceListA)
            {
                max = 0;
                foreach (String aSentenceB in sentenceListB)
                {
                    similarity2string = GetSimilariyBetween2String(aSentenceA, aSentenceB);
                    if (max < similarity2string)
                    {
                        max = similarity2string;
                        word = aSentenceB;
                    }
                }
                relevance += max;
                if (max >= 0.8)//cau trong B gan giong cau trong A
                    sentenceListB.Remove(word);
                //Kiem tra truong hop tat ca cac cau trong B gan giong voi cac cau trong A
                if (sentenceListB.Count() <= 0)
                {
                    relevance = relevance / numberSentenceB;
                    return relevance;
                }
            }
            relevance = relevance / numberSentenceA;
            return relevance;
        }

        public static double GetRelevanceBySemantic(List<KeyphraseEntity> keyphraseA, List<KeyphraseEntity> keyphraseB)
        {
            double relevance = 0;
            if (keyphraseA.Count <= 0 || keyphraseB.Count <= 0)
                return relevance;
            List<double> intersects = GetIntersectSet(keyphraseA,keyphraseB);
            List<double> unions = GetUnionSet(keyphraseA, keyphraseB);
            double numerator=0;
            foreach (double value in intersects)
                numerator += value;
            double denominator = 0;
            foreach (double value in unions)
                denominator += value;
            relevance = numerator / denominator;
            return relevance;
        }
        static List<double> GetIntersectSet(List<KeyphraseEntity> keyphraseListA, List<KeyphraseEntity> keyphraseListB)
        {
            List<double> intersects = new List<double>();
            foreach (KeyphraseEntity keyphraseA in keyphraseListA)
            {
                foreach (KeyphraseEntity keyphraseB in keyphraseListB)
                {
                    if (keyphraseA.id == keyphraseB.id)
                    {
                        if (keyphraseA.tf>keyphraseB.tf)
                            intersects.Add(keyphraseB.tf);
                        else
                            intersects.Add(keyphraseA.tf);
                        break;
                    }
                }
            }
            return intersects;
        }
        static List<double> GetUnionSet(List<KeyphraseEntity> keyphraseListA, List<KeyphraseEntity> keyphraseListB)
        {
            List<double> unionValue = new List<double>();
            List<KeyphraseEntity> unions = keyphraseListA;
            bool exist;
            foreach (KeyphraseEntity keyphraseB in keyphraseListB)
            {
                exist = false;
                foreach (KeyphraseEntity k in unions)
                {
                    if (keyphraseB.id == k.id)
                    {
                        if (keyphraseB.tf>k.tf)
                            k.tf = keyphraseB.tf;
                        exist = true;
                        break;
                    }
                }
                if (exist == false)
                    unions.Add(new KeyphraseEntity{id=keyphraseB.id, tf=keyphraseB.tf, ip=keyphraseB.ip, keyphrase =keyphraseB.keyphrase });
            }
            foreach (KeyphraseEntity k in unions)
            {
                unionValue.Add(k.tf);
            }
            return unionValue;
        }

        static double GetSimilariyBetween2String(string strA, string strB)
        {
            double similarity = 0;
            string[] listA = strA.Split(new string[] {" "}, StringSplitOptions.None);
            string[] listB = strB.Split(new string[] { " " }, StringSplitOptions.None);
            var intersect = strA.Intersect(strB);
            similarity = (2 * intersect.Count()) / (listA.Count() + listB.Count());
            return similarity;
        }
    }
}
