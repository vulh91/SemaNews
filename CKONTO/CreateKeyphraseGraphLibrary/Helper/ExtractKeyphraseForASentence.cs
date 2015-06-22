using CreateKeyphraseGraphLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateKeyphraseGraphLibrary.Helper
{
    public class POSEntity
    {
        public long Id;
        public string Keyphrase;
        public string Type;
        public string FormOrder;
    }

    public class ExtractKeyphraseEntity
    {
        public long Id;
        public string Keyphrase;
        public string FormOrder;
    }

    public class ExtractKeyphrases
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
                List<POSEntity> inPOSdic = (from pp in KeyphraseGraph.POSDictionaries
                                            join cc in KeyphraseGraph.PartOfSpeeches on pp.PartOfSpeechId equals cc.Id
                                            where (pp.Name.IndexOf(anWord) == 0)
                                            select new POSEntity { Keyphrase = pp.Name, Type = cc.Name }).ToList();
                if (inPOSdic.Count > 0)
                {
                    resultList = GetWordForPOSDictionary(inPOSdic, wordList, length, ref pos);
                    if (resultList != null)
                    {
                        UpdatePosList(ref posList, resultList, ref undeterminedWord, wordList, pos);
                        continue;
                    }
                }
                List<string> inPhrase = (from p in KeyphraseGraph.Phrases where p.Name.IndexOf(anWord) == 0 select p.Name.Trim()).ToList();
                if (inPhrase.Count > 0)
                {
                    resultList = GetWordForPhrase(inPhrase, "Phrase", wordList, length, ref pos);
                    if (resultList != null)
                    {
                        UpdatePosList(ref posList, resultList, ref undeterminedWord, wordList, pos);
                        continue;
                    }
                }

                List<ExtractKeyphraseEntity> inKeyphrase = (from p in KeyphraseGraph.Keyphrases
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

        POSEntity GetWordForPOSDictionary(List<POSEntity> typeList, String[] wordList, int length, ref int currentPos)
        {
            string word = "";

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
                        POSEntity posList = new POSEntity();
                        posList.Keyphrase = word;
                        posList.Type = isExact[0].Type;
                        currentPos = pos;
                        return posList;
                    }
                }
                else
                    return null;
            }
            return null;
        }//end GetWordForPOSDictionary

        POSEntity GetWordForPhrase(List<string> typeList, string type, String[] wordList, int length, ref int currentPos)
        {
            string word = "";

            for (int pos = currentPos; pos < length; pos++)
            {
                word = word + " " + wordList[pos];
                word = word.Trim();
                var isContain = typeList.Where(t => t.IndexOf(word) == 0).Distinct().ToList();
                if (isContain.Count > 0)
                {
                    var isExact = typeList.Where(t => t.CompareTo(word) == 0).Distinct().ToList();
                    if (isExact.Count > 0)
                    {
                        POSEntity posList = new POSEntity();
                        posList.Keyphrase = word;
                        posList.Type = type;

                        currentPos = pos;
                        return posList;
                    }
                }
                else
                    return null;
            }
            return null;
        }//end GetWordForPhrase

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
            POSEntity currentKeyphrase = null;
            string word = String.Empty;
            Int64 length = reduceList.Count;
            for (int i = 0; i < length; i++)
            {
                if (currentKeyphrase == null)
                {
                    GetKeyphraseFromNewKeyphrases(reduceList, ref i, ref currentKeyphrase, ref keyphraseList, length);
                }
                else
                {
                    GetKeyphraseFromBuiltKeyphrases(reduceList, i, ref currentKeyphrase, ref keyphraseList, length);
                }
                if (i == length - 1 && currentKeyphrase != null) //truong hop: lao dong va viec lam
                    //keyphraseList.Add(currentKeyphrase.Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
            }
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

        ////Xóa các từ A đứng trước từ B (B có từ loại là tính từ) sao cho A có từ loại thuộc {GioiTu,LienTu,DongTu}
        //int CheckRedundantWord(List<POSEntity> reduceList)
        //{
        //    int length = reduceList.Count;
        //    for (int i = length - 1; i > 0; i--)
        //    {
        //        if (reduceList[i].Type.Contains("TinhTu"))
        //            if (reduceList[i - 1].Type.Contains("GioiTu") || reduceList[i - 1].Type.Contains("LienTu") || reduceList[i - 1].Type.Contains("DongTu"))
        //                return (i - 1);
        //    }
        //    return -1;
        //}

        void GetKeyphraseFromNewKeyphrases(List<POSEntity> reduceList, ref int i, ref POSEntity currentKeyphrase, ref List<NameEntity> keyphraseList, Int64 length)
        {
            string word = String.Empty;
            if (reduceList[i].Type.Contains("Phrase"))
            {
                if (i + 1 < length && reduceList[i + 1].Type.Contains("GioiTu")) //truong hop 1: phraseG p  k
                {
                    if (i + 2 < length && reduceList[i + 2].Type.Contains("Keyphrase"))
                    {
                        if (reduceList[i + 2].FormOrder != null && reduceList[i + 2].FormOrder.Contains("2"))
                        {
                            word = reduceList[i].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                i = i + 2;
                                return;
                            }
                        }
                    }
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("Keyphrase")) //truong hop 2: phraseG  k
                {
                    if (reduceList[i + 1].FormOrder != null && reduceList[i + 1].FormOrder.Contains("2"))
                    {
                        word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                            i = i + 1;
                            return;
                        }
                    }
                }
            }
            else if (reduceList[i].Type.Contains("Keyphrase"))
            {
                if (i + 1 < length && reduceList[i + 1].Type.Contains("Keyphrase")) //truong hop 3: k1 k2
                {
                    if (CanFormOrder(reduceList[i].FormOrder, "3", "first") == true && CanFormOrder(reduceList[i + 1].FormOrder, "3", "after") == true)
                    {
                        word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            //keyphraseList.Add(reduceList[i].Keyphrase);
                            //keyphraseList.Add(reduceList[i+1].Keyphrase);
                            keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                            keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                            i = i + 1;
                            return;
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("TinhTu")) //truong hop 8: k a
                {
                    if (reduceList[i].FormOrder != null && reduceList[i].FormOrder.Contains("8"))
                    {
                        word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            //keyphraseList.Add(reduceList[i].Keyphrase);
                            keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                            i = i + 1;
                            return;
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("GioiTu")) //truong hop 4: k1 p k2
                {
                    if (i + 2 < length && reduceList[i + 2].Type.Contains("Keyphrase"))
                    {
                        if (CanFormOrder(reduceList[i].FormOrder, "4", "first") == true && CanFormOrder(reduceList[i + 2].FormOrder, "4", "after") == true)
                        {
                            word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                //keyphraseList.Add(reduceList[i].Keyphrase);
                                //keyphraseList.Add(reduceList[i+2].Keyphrase);
                                keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                i = i + 2;
                                return;
                            }
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("LienTu")) //truong hop 5: k1 c k2
                {
                    if (i + 2 < length && reduceList[i + 2].Type.Contains("Keyphrase"))
                    {
                        if (CanFormOrder(reduceList[i].FormOrder, "5", "first") == true && CanFormOrder(reduceList[i + 2].FormOrder, "5", "after") == true)
                        {
                            word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                //keyphraseList.Add(reduceList[i].Keyphrase);
                                //keyphraseList.Add(reduceList[i+2].Keyphrase);
                                keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                i = i + 2;
                                return;
                            }
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("PhuTu"))
                {
                    if (i + 2 < length)
                    {
                        if (reduceList[i + 2].Type.Contains("Keyphrase")) //truong hop 6: k1 r k2
                        {
                            if (CanFormOrder(reduceList[i].FormOrder, "6", "first") == true && CanFormOrder(reduceList[i + 2].FormOrder, "6", "after") == true)
                            {
                                word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                                bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                                if (isSuccess == true)
                                {
                                    //keyphraseList.Add(reduceList[i].Keyphrase);
                                    //keyphraseList.Add(reduceList[i+2].Keyphrase);
                                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                    keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                    i = i + 2;
                                    return;
                                }
                            }
                        }
                        else if (reduceList[i + 2].Type.Contains("TinhTu")) //truong hop 9: k r a
                        {
                            if (reduceList[i].FormOrder != null && reduceList[i].FormOrder.Contains("9"))
                            {
                                word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                                bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                                if (isSuccess == true)
                                {
                                    //keyphraseList.Add(reduceList[i].Keyphrase);
                                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                    i = i + 2;
                                    return;
                                }
                            }
                        }
                        else if (reduceList[i + 2].Type.Contains("DongTu")) //truong hop 7.2: k1 r v k2
                        {
                            if (i + 3 < length && reduceList[i + 3].Type.Contains("Keyphrase"))
                                if (CanFormOrder(reduceList[i].FormOrder, "7.2", "first") == true && CanFormOrder(reduceList[i + 3].FormOrder, "7.2", "after") == true)
                                {
                                    word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase + " " + reduceList[i + 3].Keyphrase;
                                    bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                                    if (isSuccess == true)
                                    {
                                        //keyphraseList.Add(reduceList[i].Keyphrase);
                                        //keyphraseList.Add(reduceList[i+3].Keyphrase);
                                        keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                        keyphraseList.Add(new NameEntity { Id = reduceList[i + 3].Id, Name = reduceList[i + 3].Keyphrase });
                                        i = i + 3;
                                        return;
                                    }
                                }
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else if (i + 1 < length && reduceList[i + 1].Type.Contains("DongTu")) //truong hop 7.1: k1 v k2
                {
                    if (i + 2 < length && reduceList[i + 2].Type.Contains("Keyphrase"))
                    {
                        if (CanFormOrder(reduceList[i].FormOrder, "7.1", "first") == true && CanFormOrder(reduceList[i + 2].FormOrder, "7.1", "after") == true)
                        {
                            word = reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromNewKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                //keyphraseList.Add(reduceList[i].Keyphrase);
                                //keyphraseList.Add(reduceList[i+2].Keyphrase);
                                keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                                keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                i = i + 2;
                                return;
                            }
                        }
                    }
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
                else //reduceList[i] la keyphrase va reduceList[i] khong the tao keyphrase moi -> Them reduceList[i] vao keyphraseList
                {
                    //keyphraseList.Add(reduceList[i].Keyphrase);
                    keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                }
            }//end if (reduceList[i].Type.Contains("Keyphrase"))
        }

        void GetKeyphraseFromBuiltKeyphrases(List<POSEntity> reduceList, int i, ref POSEntity currentKeyphrase, ref List<NameEntity> keyphraseList, Int64 length)
        {
            string word = String.Empty;
            if (reduceList[i].Type.Contains("Keyphrase")) //truong hop 3: k1 k2
            {
                if (CanFormOrder(currentKeyphrase.FormOrder, "3", "first") == true && CanFormOrder(reduceList[i].FormOrder, "3", "after") == true)
                {
                    //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                    NameEntity preKeyphrase = new NameEntity();
                    preKeyphrase.Id = currentKeyphrase.Id;
                    preKeyphrase.Name = currentKeyphrase.Keyphrase;
                    word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase;
                    bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                    if (isSuccess == true)
                    {
                        //keyphraseList.Add(strPreKeyphrase);
                        //keyphraseList.Add(reduceList[i].Keyphrase);
                        keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                        keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
                        return;
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
                //keyphraseList.Add(reduceList[i].Keyphrase);
                keyphraseList.Add(new NameEntity { Id = reduceList[i].Id, Name = reduceList[i].Keyphrase });
            }
            else if (reduceList[i].Type.Contains("TinhTu")) //truong hop 8: k a
            {
                if (currentKeyphrase.FormOrder != null && currentKeyphrase.FormOrder.Contains("8"))
                {
                    //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                    NameEntity preKeyphrase = new NameEntity();
                    preKeyphrase.Id = currentKeyphrase.Id;
                    preKeyphrase.Name = currentKeyphrase.Keyphrase;

                    word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase;
                    bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                    if (isSuccess == true)
                    {
                        //keyphraseList.Add(strPreKeyphrase);
                        keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                        return;
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
            else if (reduceList[i].Type.Contains("GioiTu")) //truong hop 4: k1 p k2
            {
                if (i + 1 < length && reduceList[i + 1].Type.Contains("Keyphrase"))
                {
                    if (CanFormOrder(currentKeyphrase.FormOrder, "4", "first") == true && CanFormOrder(reduceList[i + 1].FormOrder, "4", "after") == true)
                    {
                        //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                        NameEntity preKeyphrase = new NameEntity();
                        preKeyphrase.Id = currentKeyphrase.Id;
                        preKeyphrase.Name = currentKeyphrase.Keyphrase;

                        word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            //keyphraseList.Add(strPreKeyphrase);
                            //keyphraseList.Add(reduceList[i+1].Keyphrase);
                            keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                            keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                            return;
                        }
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
            else if (reduceList[i].Type.Contains("LienTu")) //truong hop 5: k1 c k2
            {
                if (i + 1 < length && reduceList[i + 1].Type.Contains("Keyphrase"))
                {
                    if (CanFormOrder(currentKeyphrase.FormOrder, "5", "first") == true && CanFormOrder(reduceList[i + 1].FormOrder, "5", "after") == true)
                    {
                        //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                        NameEntity preKeyphrase = new NameEntity();
                        preKeyphrase.Id = currentKeyphrase.Id;
                        preKeyphrase.Name = currentKeyphrase.Keyphrase;

                        word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            //keyphraseList.Add(strPreKeyphrase);
                            //keyphraseList.Add(reduceList[i + 1].Keyphrase);
                            keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                            keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                            return;
                        }
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
            else if (reduceList[i].Type.Contains("PhuTu"))
            {
                if (i + 1 < length)
                {
                    if (reduceList[i + 1].Type.Contains("Keyphrase")) //truong hop 6: k1 r k2
                    {
                        if (CanFormOrder(currentKeyphrase.FormOrder, "6", "first") == true && CanFormOrder(reduceList[i + 1].FormOrder, "6", "after") == true)
                        {
                            //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                            NameEntity preKeyphrase = new NameEntity();
                            preKeyphrase.Id = currentKeyphrase.Id;
                            preKeyphrase.Name = currentKeyphrase.Keyphrase;

                            word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                //keyphraseList.Add(strPreKeyphrase);
                                //keyphraseList.Add(reduceList[i + 1].Keyphrase);
                                keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                                keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                                return;
                            }
                        }
                    }
                    else if (reduceList[i + 1].Type.Contains("TinhTu")) //truong hop 9: k r a
                    {
                        if (currentKeyphrase.FormOrder != null && currentKeyphrase.FormOrder.Contains("9"))
                        {
                            //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                            NameEntity preKeyphrase = new NameEntity();
                            preKeyphrase.Id = currentKeyphrase.Id;
                            preKeyphrase.Name = currentKeyphrase.Keyphrase;

                            word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                            bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                            if (isSuccess == true)
                            {
                                //keyphraseList.Add(strPreKeyphrase);
                                keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                                return;
                            }
                        }
                    }
                    else if (reduceList[i + 1].Type.Contains("DongTu")) //truong hop 7.2: k1 r v k2
                    {
                        if (i + 2 < length && reduceList[i + 2].Type.Contains("Keyphrase"))
                            if (CanFormOrder(currentKeyphrase.FormOrder, "7.2", "first") == true && CanFormOrder(reduceList[i + 2].FormOrder, "7.2", "after") == true)
                            {
                                //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                                NameEntity preKeyphrase = new NameEntity();
                                preKeyphrase.Id = currentKeyphrase.Id;
                                preKeyphrase.Name = currentKeyphrase.Keyphrase;

                                word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase + " " + reduceList[i + 2].Keyphrase;
                                bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                                if (isSuccess == true)
                                {
                                    //keyphraseList.Add(strPreKeyphrase);
                                    //keyphraseList.Add(reduceList[i + 2].Keyphrase);
                                    keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                                    keyphraseList.Add(new NameEntity { Id = reduceList[i + 2].Id, Name = reduceList[i + 2].Keyphrase });
                                    return;
                                }
                            }
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
            else if (reduceList[i].Type.Contains("DongTu")) //truong hop 7.1: k1 v k2
            {
                if (i + 1 < length && reduceList[i + 1].Type.Contains("Keyphrase"))
                {
                    if (CanFormOrder(currentKeyphrase.FormOrder, "7.1", "first") == true && CanFormOrder(reduceList[i + 1].FormOrder, "7.1", "after") == true)
                    {
                        //string strPreKeyphrase = currentKeyphrase.Keyphrase;
                        NameEntity preKeyphrase = new NameEntity();
                        preKeyphrase.Id = currentKeyphrase.Id;
                        preKeyphrase.Name = currentKeyphrase.Keyphrase;

                        word = currentKeyphrase.Keyphrase + " " + reduceList[i].Keyphrase + " " + reduceList[i + 1].Keyphrase;
                        bool isSuccess = CheckFormedKeyphrase_FromBuiltKeyphrases(word, ref currentKeyphrase);
                        if (isSuccess == true)
                        {
                            //keyphraseList.Add(strPreKeyphrase);
                            //keyphraseList.Add(reduceList[i + 1].Keyphrase);
                            keyphraseList.Add(new NameEntity { Id = preKeyphrase.Id, Name = preKeyphrase.Name });
                            keyphraseList.Add(new NameEntity { Id = reduceList[i + 1].Id, Name = reduceList[i + 1].Keyphrase });
                            return;
                        }
                    }
                }
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
            else //currentKeyphrase la keyphrase va currentKeyphrase khong the tao keyphrase moi -> Them currentKeyphrase vao keyphraseList
            {
                //keyphraseList.Add(currentKeyphrase.Keyphrase);
                keyphraseList.Add(new NameEntity { Id = currentKeyphrase.Id, Name = currentKeyphrase.Keyphrase });
                currentKeyphrase = null;
            }
        }

        //Kiem tra keyphrase duoc thiet lap moi (duoc tao thanh tu 2 keyphrase don) co trong CKOntology
        bool CheckFormedKeyphrase_FromNewKeyphrases(string word, ref POSEntity currentKeyphrase)
        {
            //using (db = new CKONTOLOGYEntities())
            //{
            //var isExist = db.Keyphrases.Where(p => String.Compare(p.Name, word) == 0).FirstOrDefault(); //luc truoc su dung ham nay. Vi cai tien toc do nen db.Keyphrases duoc load len o ham InitialData()
            var isExist = KeyphraseGraph.Keyphrases.Where(p => String.Compare(p.Name, word) == 0).FirstOrDefault();
            if (isExist != null)//neu keyphrase moi thiet lap ton tai trong Ontology
            {
                currentKeyphrase = new POSEntity();
                currentKeyphrase.Id = isExist.Id;
                currentKeyphrase.Keyphrase = isExist.Name;
                currentKeyphrase.Type = "Keyphrase";
                currentKeyphrase.FormOrder = isExist.FormOrder;
                return true;
            }
            //}
            return false;
        }

        //Kiem tra keyphrase duoc thiet lap moi (duoc tao thanh tu 1 keyphrase to hop va 1 keyphrase don) co trong CKOntology
        bool CheckFormedKeyphrase_FromBuiltKeyphrases(string word, ref POSEntity currentKeyphrase)
        {
            //using (db = new CKONTOLOGYEntities())
            //{
            //var isExist = db.Keyphrases.Where(p => String.Compare(p.Name, word) == 0).FirstOrDefault(); //luc truoc su dung ham nay. Vi cai tien toc do nen db.Keyphrases duoc load len o ham InitialData()
            var isExist = KeyphraseGraph.Keyphrases.Where(p => String.Compare(p.Name, word) == 0).FirstOrDefault();
            if (isExist != null)//neu keyphrase moi thiet lap ton tai trong Ontology
            {
                //currentKeyphrase = new POSEntity();
                currentKeyphrase.Id = isExist.Id;
                currentKeyphrase.Keyphrase = isExist.Name;
                currentKeyphrase.Type = "Keyphrase";
                currentKeyphrase.FormOrder = isExist.FormOrder;
                return true;
            }
            //}
            return false;
        }

        //structurePos: cau truc thiet lap nao duoc su dung
        //condition: dung truoc, sau hay co the dung truoc va sau
        bool CanFormOrder(string formOrder, string structurePos, string condition)
        {
            if (formOrder == null)
                return false;
            int indexForm = formOrder.IndexOf(structurePos);
            if (indexForm > 0)
            {
                string subForm = String.Empty;
                for (int i = indexForm - 1; i < formOrder.Length; i++)
                {
                    subForm += formOrder[i];
                    if (formOrder[i] == ')')
                        break;
                }
                if (subForm.Contains(condition) || subForm.Contains("all"))
                    return true;
            }
            return false;
        }

        #endregion Tìm keyphrase dự tuyển và lựa chọn keyphrase cuối cùng

    }//end class ExtractKeyphrases
}
