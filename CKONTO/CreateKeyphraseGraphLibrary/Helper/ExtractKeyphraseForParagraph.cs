using CreateKeyphraseGraphLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateKeyphraseGraphLibrary.Helper
{
    class ExtractKeyphraseForParagraph
    {
        //content da duoc trim()
        public static List<NumberKeyphraseEntity> GetKeyphraseForParagraph(String content)
        {
            String[] sentenceList = content.Split(new string[] { "...", ".", "!", "?" }, StringSplitOptions.None);
            List<NumberKeyphraseEntity> result = null;
            List<NumberKeyphraseEntity> aSentenceKeyphraseEntity = new List<NumberKeyphraseEntity>();
            bool isExist;
            foreach (String aSentence in sentenceList)
            {
                aSentenceKeyphraseEntity = GetKeyphraseForSentence(aSentence);
                if (aSentenceKeyphraseEntity.Count > 0)
                {
                    if (result != null)
                    {
                        foreach (NumberKeyphraseEntity aSK in aSentenceKeyphraseEntity)
                        {
                            isExist = false;
                            foreach (NumberKeyphraseEntity aResult in result)
                            {
                                if (String.Compare(aSK.keyphrase, aResult.keyphrase) == 0)
                                {
                                    aResult.number += aSK.number;
                                    isExist = true;
                                    break;
                                }
                            }
                            if (isExist == false)
                                result.Add(aSK);
                        }
                    }
                    else
                    {
                        result = new List<NumberKeyphraseEntity>();
                        result = aSentenceKeyphraseEntity;
                    }
                }
            }
            return result;
        }
        static List<NumberKeyphraseEntity> GetKeyphraseForSentence(String aSentence)
        {
            ExtractKeyphrases extractKeyphrase = new ExtractKeyphrases();
            List<POSEntity> SegAndPoslist;
            List<NameEntity> keyphraseList;
            List<NumberKeyphraseEntity> result = new List<NumberKeyphraseEntity>();
            NumberKeyphraseEntity temp;
            int number = 0;
            int j;
            SegAndPoslist = extractKeyphrase.SegmenAndPOSTagging(aSentence);
            keyphraseList = extractKeyphrase.ChooseKeyphrase(SegAndPoslist);
            //Duyet cac keyphrase co MonoKeyphrases khac NULL
            List<NameEntity> estabKeyphrases = new List<NameEntity>();
            List<NameEntity> aEstabKey;
            foreach (NameEntity aKeyphrase in keyphraseList)
            {
                var entity = KeyphraseGraph.Keyphrases.Where(p => String.Compare(p.Name, aKeyphrase.Name) == 0).FirstOrDefault();
                if (entity != null)
                {
                    if (!String.IsNullOrEmpty(entity.MonoKeyphrases))
                    {
                        aEstabKey = new List<NameEntity>();
                        EstablishKeyphrase.GetEstablish_KeyphraseAndRelation(aKeyphrase.Name, ref aEstabKey, entity.MonoKeyphrases);
                        estabKeyphrases.AddRange(aEstabKey);
                    }
                }
            }

            //Them keyphrase duoc thiet lap vao danh sach keyphrase hien co
            keyphraseList.AddRange(estabKeyphrases);
            for (int i = 0; i < keyphraseList.Count; i++)
            {
                j = i + 1;
                number = 1;
                while (j < keyphraseList.Count)
                {
                    if (String.Compare(keyphraseList[i].Name, keyphraseList[j].Name) == 0)
                    {
                        keyphraseList.RemoveAt(j);
                        number++;
                    }
                    else
                        j++;
                }
                temp = new NumberKeyphraseEntity();
                temp.Id = keyphraseList[i].Id;
                temp.keyphrase = keyphraseList[i].Name;
                temp.number = number;
                result.Add(temp);
            }
            return result;
        }
    }
}
