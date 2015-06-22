using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.DTCDTNN
{
    public class ExtractKeyphraseForASentence_DTCDTNN
    {
        String StopWordsErasing(String str)
        {
            String result = str;
            result = result.Replace(",", "").Replace(":", "");
            return result;
        }

        #region Phân đoạn từ và gán nhãn loại từ

        //Ham nay giong ham SegmenAndPOSTaggingOld nhung database duoc load len 1 lan va su dung cho ca chuong trinh
        public List<POSEntity> SegmenAndPOSTagging(String pStr)
        {
            String str = pStr;
            str = str.Trim().ToLower();
            str = StopWordsErasing(str);
            String[] wordList = str.Split(new string[] { " " }, StringSplitOptions.None);
            int length = wordList.Count();

            List<POSEntity> posList = new List<POSEntity>();
            string undeterminedWord = ""; //từ có loại từ là “chưa xác định”
            POSEntity resultList;
            string anWord = String.Empty;
            for (int pos = 0; pos < length; pos++)
            {
                anWord = wordList[pos];

                List<ExtractKeyphraseEntity> inKeyphrase = (from p in SemanticSearch_DTCDTNN.Keyphrases
                                                            where p.Name.IndexOf(anWord) == 0
                                                            select new ExtractKeyphraseEntity
                                                            {
                                                                Id = p.Id,
                                                                Keyphrase = p.Name.Trim(),
                                                                FormOrder = p.FormOrder
                                                            }).ToList();
                if (inKeyphrase.Count > 0)
                {
                    resultList = GetWordForKeyphrase(inKeyphrase, "Keyphrase", wordList, length, ref pos);
                    if (resultList != null)
                    {
                        UpdatePosList(ref posList, resultList, ref undeterminedWord, wordList, pos);
                        continue;
                    }
                }
                if (String.IsNullOrEmpty(undeterminedWord))
                    undeterminedWord = wordList[pos];
                else
                    undeterminedWord += " " + wordList[pos];

            }//end for pos
            if (!String.IsNullOrEmpty(undeterminedWord))
            {
                POSEntity anEntity = new POSEntity();
                anEntity.Keyphrase = undeterminedWord;
                anEntity.Type = "undetermined";
                posList.Add(anEntity);
            }
            return posList;
        } //end SegmenAndPOSTagging

        POSEntity GetWordForKeyphrase(List<ExtractKeyphraseEntity> typeList, string type, String[] wordList, int length, ref int currentPos)
        {
            string word = "";
            POSEntity posList = null;
            for (int pos = currentPos; pos < length; pos++)
            {
                word = word + " " + wordList[pos];
                word = word.Trim();
                var isContain = typeList.Where(t => t.Keyphrase.IndexOf(word) == 0).Distinct().ToList();
                if (isContain.Count > 0)
                {
                    var isExact = typeList.Where(t => t.Keyphrase.CompareTo(word) == 0).Distinct().ToList();
                    if (isExact.Count > 0)
                    {
                        if (posList == null)
                        {
                            posList = new POSEntity();
                            posList.Id = isExact[0].Id;
                            posList.Keyphrase = word;
                            posList.Type = type;
                        }
                        else
                        {
                            if (posList.Keyphrase.Length < word.Length)
                            {
                                posList.Id = isExact[0].Id;
                                posList.Keyphrase = word;
                            }
                        }
                        if (isExact[0].FormOrder != null)
                            posList.FormOrder = isExact[0].FormOrder.Trim();
                        else
                            posList.FormOrder = "";
                        currentPos = pos;
                    }
                }
                else
                    return posList;
            }
            return posList;
        }//end GetWordForKeyphrase

        void UpdatePosList(ref List<POSEntity> posList, POSEntity resultList, ref string undeterminedWord, String[] wordList, int currentPos)
        {
            if (!String.IsNullOrEmpty(undeterminedWord))
            {
                POSEntity anEntity = new POSEntity();
                anEntity.Keyphrase = undeterminedWord;
                anEntity.Type = "undetermined";
                posList.Add(anEntity);
                undeterminedWord = "";
            }
            posList.Add(resultList);
        }//end UpdatePosList

        #endregion Phân đoạn từ và gán nhãn loại từ

        #region Tìm keyphrase dự tuyển và lựa chọn keyphrase cuối cùng

        public List<NameEntity> ChooseKeyphrase(List<POSEntity> posList)
        {
            List<POSEntity> reduceList = posList;
            //Kiểm tra các từ đứng trước 1 từ có từ loại là “Chưa xác định”, nếu các từ đứng trước có từ loại là: GioiTu, LienTu, DongTu, TinhTu, PhuTu thì xóa từ đó
            int indexRedundantWord;
            do
            {
                indexRedundantWord = CheckRedundantBeforeUndeterminedWord(reduceList);
                if (indexRedundantWord != -1)
                    reduceList.RemoveAt(indexRedundantWord);
            } while (indexRedundantWord != -1);

            //Xóa các từ có từ loại là "Chưa xác định"
            for (int i = 0; i < posList.Count; i++)
            {
                if (reduceList[i].Type.Contains("undetermined"))
                    reduceList.RemoveAt(i);
            }

            //Tìm keyphrase
            List<NameEntity> keyphraseList = new List<NameEntity>();
            keyphraseList = (from k in reduceList select new NameEntity { Id = k.Id, Name = k.Keyphrase }).ToList();
            return keyphraseList;
        }//end ChooseKeyphrase

        //Kiểm tra các từ đứng trước 1 từ có từ loại là “Chưa xác định”, nếu các từ đứng trước có từ loại là: GioiTu, LienTu, DongTu, TinhTu, PhuTu thì xóa từ đó
        int CheckRedundantBeforeUndeterminedWord(List<POSEntity> reduceList)
        {
            int length = reduceList.Count;
            for (int i = length - 1; i > 0; i--)
            {
                if (reduceList[i].Type.Contains("undetermined"))
                    if (reduceList[i - 1].Type.Contains("GioiTu") || reduceList[i - 1].Type.Contains("LienTu") || reduceList[i - 1].Type.Contains("DongTu") || reduceList[i - 1].Type.Contains("PhuTu"))
                        return (i - 1);
            }
            return -1;
        }
        #endregion Tìm keyphrase dự tuyển và lựa chọn keyphrase cuối cùng
    }
}
