using SearchLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Helper
{
    public class EstablishKeyphrase
    {
        //Ham nay chua test - LUAN VAN XEM LAI HAM NAY
        //Ham nay de quy moi keyphrase de tim ra tat ca keyphrase don
        //Vi du: "nang suat lao dong thap" se tim duoc "nang suat lao dong". Sau do voi "nang suat lao dong" se tim duoc "nang suat" va "lao dong"
        public static void GetEstablish_KeyphraseAndRelation_XEMLAI(string keyphrase, ref List<ExtractKeyphraseEntity> estabKeyphrases, ref List<string> strEstabRelation)
        {
            var formOrderEntity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, keyphrase) == 0).FirstOrDefault();
            if (formOrderEntity != null)
            {
                if (formOrderEntity.MonoKeyphrases != null)
                {
                    string aRelation = String.Empty;
                    string[] estabElements = formOrderEntity.MonoKeyphrases.Split(new string[] { "," }, StringSplitOptions.None);
                    if (estabElements.Length == 1)
                    {
                        aRelation = "(" + keyphrase + "," + estabElements[0] + ",ESTABLISHMENT,26)";
                        //if (!strEstabRelation.Contains(aRelation))
                        strEstabRelation.Add(aRelation);
                        GetEstablish_KeyphraseAndRelation(estabElements[0], ref estabKeyphrases, ref strEstabRelation);
                    }
                    else //estabElements.Length == 2
                    {
                        aRelation = "(" + keyphrase + "," + estabElements[0] + ",ESTABLISHMENT,26)";
                        //if (!strEstabRelation.Contains(aRelation))
                        strEstabRelation.Add(aRelation);
                        aRelation = "(" + keyphrase + "," + estabElements[1] + ",ESTABLISHMENT,26)";
                        //if (!strEstabRelation.Contains(aRelation))
                        strEstabRelation.Add(aRelation);
                        GetEstablish_KeyphraseAndRelation(estabElements[0], ref estabKeyphrases, ref strEstabRelation);
                        GetEstablish_KeyphraseAndRelation(estabElements[1], ref estabKeyphrases, ref strEstabRelation);
                    }
                }
                else
                    estabKeyphrases.Add(new ExtractKeyphraseEntity { Id = formOrderEntity.Id, Keyphrase = formOrderEntity.Name });
            }
        }


        //Ham nay chi phan tich keyphrase 1 lan de tim ra keyphrase don
        //Vi du: "nang suat lao dong thap" se tim duoc "nang suat lao dong". Khong tim nua
        public static void GetEstablish_KeyphraseAndRelation(string keyphrase, ref List<ExtractKeyphraseEntity> estabKeyphrases, ref List<string> estabRelation)
        {
            var formOrderEntity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, keyphrase) == 0).FirstOrDefault();
            if (formOrderEntity != null)
            {
                if (formOrderEntity.MonoKeyphrases != null)
                {
                    string[] estabElements = formOrderEntity.MonoKeyphrases.Split(new string[] { "," }, StringSplitOptions.None);
                    if (estabElements.Length == 1)
                    {
                        string strTemp = estabElements[0].Trim();
                        var entity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp) == 0).FirstOrDefault();
                        if (entity != null)
                        {
                            estabKeyphrases.Add(new ExtractKeyphraseEntity { Id = entity.Id, Keyphrase = entity.Name });
                            estabRelation.Add("(" + keyphrase + "," + strTemp + ",ESTABLISHMENT,26)");
                        }
                    }
                    else //estabElements.Length == 2
                    {
                        string strTemp = estabElements[0].Trim();
                        var entity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp) == 0).FirstOrDefault();
                        if (entity != null)
                        {
                            estabKeyphrases.Add(new ExtractKeyphraseEntity { Id = entity.Id, Keyphrase = entity.Name });
                            estabRelation.Add("(" + keyphrase + "," + strTemp + ",ESTABLISHMENT,26)");
                        }
                        string strTemp1 = estabElements[1].Trim();
                        var entity1 = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp1) == 0).FirstOrDefault();
                        if (entity1 != null)
                        {
                            estabKeyphrases.Add(new ExtractKeyphraseEntity { Id = entity1.Id, Keyphrase = entity1.Name });
                            estabRelation.Add("(" + keyphrase + "," + strTemp1 + ",ESTABLISHMENT,26)");
                        }
                    }
                }
            }
        }//end GetEstablish_KeyphraseAndRelation

        //Ham nay chi phan tich keyphrase 1 lan de tim ra keyphrase don
        //Vi du: "nang suat lao dong thap" se tim duoc "nang suat lao dong". Khong tim nua
        public static void GetEstablish_KeyphraseAndRelation(string keyphrase, ref List<NameEntity> estabKeyphrases, string monoKeyphrases)
        {
            string[] estabElements = monoKeyphrases.Split(new string[] { "," }, StringSplitOptions.None);
            if (estabElements.Length == 1)
            {
                string strTemp = estabElements[0].Trim();
                var entity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp) == 0).FirstOrDefault();
                if (entity != null)
                {
                    estabKeyphrases.Add(new NameEntity { Id = entity.Id, Name = entity.Name });
                }
            }
            else //estabElements.Length == 2
            {
                string strTemp = estabElements[0].Trim();
                var entity = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp) == 0).FirstOrDefault();
                if (entity != null)
                {
                    estabKeyphrases.Add(new NameEntity { Id = entity.Id, Name = entity.Name });
                }
                string strTemp1 = estabElements[1].Trim();
                var entity1 = SemanticSearch.Keyphrases.Where(p => String.Compare(p.Name, strTemp1) == 0).FirstOrDefault();
                if (entity1 != null)
                {
                    estabKeyphrases.Add(new NameEntity { Id = entity1.Id, Name = entity1.Name });
                }
            }
                
        }//end GetEstablish_KeyphraseAndRelation
    }
}
