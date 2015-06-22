using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.DTCDTNN
{
    public class ExtractKeyphrase_DTCDTNN
    {
        //content da duoc trim()
        public static List<NameEntity> GetKeyphraseForParagraph(String content)
        {
            String[] sentenceList = content.Split(new string[] { "...", ".", "!", "?" }, StringSplitOptions.None);
            List<NameEntity> result = null;
            List<NameEntity> aSentenceKeyphraseEntity = new List<NameEntity>();
            bool isExist;
            foreach (String aSentence in sentenceList)
            {
                aSentenceKeyphraseEntity = GetKeyphraseForSentence(aSentence);
                if (aSentenceKeyphraseEntity.Count > 0)
                {
                    if (result != null)
                    {
                        foreach (NameEntity aSK in aSentenceKeyphraseEntity)
                        {
                            isExist = false;
                            foreach (NameEntity aResult in result)
                            {
                                if (String.Compare(aSK.Name, aResult.Name) == 0)
                                {
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
                        result = new List<NameEntity>();
                        result = aSentenceKeyphraseEntity;
                    }
                }
            }


            return result;
        }
        static List<NameEntity> GetKeyphraseForSentence(String aSentence)
        {
            ExtractKeyphraseForASentence_DTCDTNN extractKeyphrase = new ExtractKeyphraseForASentence_DTCDTNN();
            List<POSEntity> SegAndPoslist;
            List<NameEntity> keyphraseList;
            int j;
            SegAndPoslist = extractKeyphrase.SegmenAndPOSTagging(aSentence);
            keyphraseList = extractKeyphrase.ChooseKeyphrase(SegAndPoslist);
            for (int i = 0; i < keyphraseList.Count; i++)
            {
                j = i + 1;
                //number = 1;
                while (j < keyphraseList.Count)
                {
                    if (String.Compare(keyphraseList[i].Name, keyphraseList[j].Name) == 0)
                    {
                        keyphraseList.RemoveAt(j);
                        //number++;
                    }
                    else
                        j++;
                }
            }
            return keyphraseList;
        }
    }
}
