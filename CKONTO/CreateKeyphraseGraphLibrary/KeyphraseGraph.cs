using CreateKeyphraseGraphLibrary.DatabaseModel;
using CreateKeyphraseGraphLibrary.EntityModel;
using CreateKeyphraseGraphLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateKeyphraseGraphLibrary
{
    public class KeyphraseGraph
    {
        static CKONTOLOGYCreateKGEntities dbCKOntology;

        static List<POSDictionary> POSDict = new List<POSDictionary>();
        static List<PartOfSpeech> POS = new List<PartOfSpeech>();
        static List<Phrase> Phrase = new List<Phrase>();
        static List<Keyphrase> Keyphrase = new List<Keyphrase>();
        static List<IPWeight> ipWeights = new List<IPWeight>();
        static List<Keyphrase_KeyphraseRelationship> kkRelationship = new List<Keyphrase_KeyphraseRelationship>();
        static List<KeyphraseRelationship> keyphraseRelationship = new List<KeyphraseRelationship>();

        public static void InitialData()
        {
            using (dbCKOntology = new CKONTOLOGYCreateKGEntities())
            {
                POSDict = (from pp in dbCKOntology.POSDictionaries select pp).ToList();
                POS = (from p in dbCKOntology.PartOfSpeeches select p).ToList();
                Phrase = (from p in dbCKOntology.Phrases select p).ToList();
                Keyphrase = (from k in dbCKOntology.Keyphrases select k).ToList();
                ipWeights = (from i in dbCKOntology.IPWeights select i).ToList();
                kkRelationship = (from k in dbCKOntology.Keyphrase_KeyphraseRelationship select k).ToList();
                keyphraseRelationship = (from k in dbCKOntology.KeyphraseRelationships select k).ToList();
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
        public static List<IPWeight> IPWeights
        {
            get
            {
                return ipWeights;
            }
        }
        public static List<Keyphrase_KeyphraseRelationship> KKRelationship
        {
            get
            {
                return kkRelationship;
            }
        }
        public static List<KeyphraseRelationship> KeyphraseRelationship
        {
            get
            {
                return keyphraseRelationship;
            }
        }

        /// <summary>
        /// Ham tao do thi keyphrase cho 1 tin bai
        /// </summary>
        /// Cac tham so da duoc Normalize (tuc da duoc chuan hoa o dang chuan)
        /// <param name="_articleId"></param>
        /// <param name="_title"></param>
        /// <param name="_abstract"></param>
        /// <param name="_content"></param>
        /// <param name="_tags"></param>
        /// <returns>
        /// tra ve chuoi chua do thi keyphrase cho 1 tin bai
        /// </returns>
        public static string GetKeyphraseGraph(int _articleId, string _title, string _abstract, string _content, string _tags)
        {
            List<NumberKeyphraseEntity> titleList = null;
            List<NumberKeyphraseEntity> abstractList = null;
            List<NumberKeyphraseEntity> contentList = null;
            List<NumberKeyphraseEntity> tagsList = null;
            List<NameEntity> keyphraseList;
            List<NameEntity> tempList = new List<NameEntity>();
                        
            if (!String.IsNullOrEmpty(_title))
                titleList = ExtractKeyphraseForParagraph.GetKeyphraseForParagraph(_title.Trim());
            if (!String.IsNullOrEmpty(_abstract))
                abstractList = ExtractKeyphraseForParagraph.GetKeyphraseForParagraph(_abstract.Trim());
            if (!String.IsNullOrEmpty(_content))
                contentList = ExtractKeyphraseForParagraph.GetKeyphraseForParagraph(_content.Trim());
            if (!String.IsNullOrEmpty(_tags))
                tagsList = ExtractKeyphraseForParagraph.GetKeyphraseForParagraph(_tags.Trim());

            float numTotalKeyphrase = 0;
            keyphraseList = new List<NameEntity>();
            if (titleList != null)
            {
                keyphraseList = titleList.Select(i => new NameEntity { Id = i.Id, Name = i.keyphrase }).ToList();
                numTotalKeyphrase = titleList.Select(i => i.number).Sum();
            }
            if (abstractList != null)
            {
                //keyphraseList = (abstractList.Select(i => i.keyphrase).Union(keyphraseList)).ToList();
                tempList = abstractList.Select(i => new NameEntity { Id = i.Id, Name = i.keyphrase }).ToList();
                keyphraseList = UnionList(tempList, keyphraseList);
                numTotalKeyphrase += abstractList.Select(i => i.number).Sum();
            }
            if (tagsList != null)
            {
                //keyphraseList = (tagsList.Select(i => i.keyphrase).Union(keyphraseList)).ToList();
                tempList = tagsList.Select(i => new NameEntity { Id = i.Id, Name = i.keyphrase }).ToList();
                keyphraseList = UnionList(tempList, keyphraseList);
                numTotalKeyphrase += tagsList.Select(i => i.number).Sum();
            }
            if (contentList != null)
            {
                //keyphraseList = (contentList.Select(i => i.keyphrase).Union(keyphraseList)).ToList();
                tempList = contentList.Select(i => new NameEntity { Id = i.Id, Name = i.keyphrase }).ToList();
                keyphraseList = UnionList(tempList, keyphraseList);
                numTotalKeyphrase += contentList.Select(i => i.number).Sum();
            }

            #region Tim quan he giua cac keyphrase
            
            //Lay quan he giua cac keyphrase trong danh sach keyphraseList (cac quan he khong phai la quan he thiet lap)
            List<KKRelationEntity> tempRelationList = new List<KKRelationEntity>();
            String strRelation = "Relations:{";
            string strARelation = String.Empty;
            Int64 numKeyphraseList = keyphraseList.Count();
            List<KKRelationEntity> relationList = new List<KKRelationEntity>();
            for (Int32 i = 0; i < numKeyphraseList - 1; i++)
            {
                for (Int32 j = i + 1; j < numKeyphraseList; j++)
                {
                    KKRelationEntity relation1 = (from kk in KKRelationship
                                                  where kk.SourceId == keyphraseList[i].Id && kk.TargetId == keyphraseList[j].Id
                                                  select new KKRelationEntity
                                                  {
                                                      Keyphrase1 = keyphraseList[i].Name,
                                                      Keyphrase2 = keyphraseList[j].Name,
                                                      idrelation = kk.RelationshipId,
                                                      NameRelation = ""
                                                  }).FirstOrDefault();
                    KKRelationEntity relation2 = (from kk in KKRelationship
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
                        name = (from k in KeyphraseRelationship
                                where k.Id == relation1.idrelation
                                select k.Name).FirstOrDefault();
                        relation1.NameRelation = name;
                        //relationList.Add(relation1);
                        strARelation = "(" + relation1.Keyphrase1 + "," + relation1.Keyphrase2 + "," + relation1.NameRelation + "," + relation1.idrelation + "),";
                        strRelation = String.Concat(strRelation, strARelation);
                        tempRelationList.Add(relation1);
                    }
                    if (relation2 != null)
                    {
                        if (String.IsNullOrEmpty(name))
                            name = (from k in KeyphraseRelationship
                                    where k.Id == relation2.idrelation
                                    select k.Name).FirstOrDefault();
                        relation2.NameRelation = name;
                        //relationList.Add(relation2);
                        strARelation = "(" + relation2.Keyphrase1 + "," + relation2.Keyphrase2 + "," + relation2.NameRelation + "," + relation2.idrelation + "),";
                        strRelation = String.Concat(strRelation, strARelation);
                        tempRelationList.Add(relation2);
                    }
                }

            }//end for (long i = 0; i < numKeyphraseList - 1; i++)
            
            if (strRelation.Length > 11)
                strRelation = strRelation.Remove(strRelation.Length - 1);
            strRelation = String.Concat(strRelation, "}");
            #endregion Tim quan he giua cac keyphrase

            #region Tinh tf va ip cho cac keyphrase

            long number = 0;
            float? weightTotal = 0;
            NumberKeyphraseEntity temp;
            //List<float?> ipWeightList = new List<float?>();
            //List<float?> tfList = new List<float?>();
            float? ipWeight = 0;
            float? tf = 0;
            String strKeyphrase = "Keyphrases:{";
            String strAKeyphrase = String.Empty;
            int isIsolation; //1 la dinh co lap, 0 la dinh khong co lap

            foreach (NameEntity aKeyphrase in keyphraseList)
            {
                isIsolation = 1;
                number = 0;
                weightTotal = 0;
                if (titleList != null)
                {
                    temp = titleList.Where(i => String.Compare(i.keyphrase, aKeyphrase.Name) == 0).FirstOrDefault();
                    if (temp != null)
                    {
                        number = temp.number;
                        weightTotal = temp.number * IPWeights[0].Value;
                    }
                }
                if (abstractList != null)
                {
                    temp = abstractList.Where(i => String.Compare(i.keyphrase, aKeyphrase.Name) == 0).FirstOrDefault();
                    if (temp != null)
                    {
                        number += temp.number;
                        weightTotal += temp.number * IPWeights[1].Value;
                    }
                }
                if (contentList != null)
                {
                    temp = contentList.Where(i => String.Compare(i.keyphrase, aKeyphrase.Name) == 0).FirstOrDefault();
                    if (temp != null)
                    {
                        number += temp.number;
                        weightTotal += temp.number * IPWeights[2].Value;
                    }
                }
                if (abstractList != null)
                {
                    temp = abstractList.Where(i => String.Compare(i.keyphrase, aKeyphrase.Name) == 0).FirstOrDefault();
                    if (temp != null)
                    {
                        number += temp.number;
                        weightTotal += temp.number * IPWeights[3].Value;
                    }
                }
                //ipWeightList.Add(weightTotal / number);
                //tfList.Add(number/numTotalKeyphrase);
                ipWeight = weightTotal / number;
                tf = number / numTotalKeyphrase;
                var existKeyphrase = tempRelationList.Where(r => (String.Compare(r.Keyphrase1, aKeyphrase.Name) == 0) || (String.Compare(r.Keyphrase2, aKeyphrase.Name) == 0)).FirstOrDefault();
                if (existKeyphrase != null) //keyphrase khong co lap
                    isIsolation = 0;
                strAKeyphrase = "(" + aKeyphrase.Id + "_" + aKeyphrase.Name + "_" + tf + "_" + ipWeight + "_" + isIsolation + "),";
                strKeyphrase = String.Concat(strKeyphrase, strAKeyphrase);

            }//end foreach (string aKeyphrase in keyphraseList
            if (strKeyphrase.Length > 12)
                strKeyphrase = strKeyphrase.Remove(strKeyphrase.Length - 1);
            strKeyphrase = String.Concat(strKeyphrase, "}\n");
            #endregion Tinh tf va ip cho cac keyphrase

            String articleKG = "articleID:" + _articleId + "\n";
            articleKG = String.Concat(String.Concat(articleKG, strKeyphrase), strRelation);

            return articleKG;
        }

        static List<NameEntity> UnionList(List<NameEntity> list1, List<NameEntity> list2)
        {
            var dict = list2.ToDictionary(p => p.Id);
            foreach (var nameEntity in list1)
            {
                dict[nameEntity.Id] = nameEntity;
            }
            List<NameEntity> mergedList = dict.Values.ToList();
            return mergedList;
        }
    }
}
