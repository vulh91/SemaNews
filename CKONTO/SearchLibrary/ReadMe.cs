using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary
{
    class ReadMe
    {
        //*********Tinh hang cua 1 cau truy van va 1 do thi keyphrase cua 1 tin bai**********
        //SearchLibrary.SemanticSearch.InitialData()
        //SearchLibrary.SemanticSearch.GetRank(queryKG, articleKG)
        //trong do: queryKG la GraphEntity, articleKG la GraphEntity

        /*Vi du:       articleList: danh sach tin bai
        SearchLibrary.SemanticSearch.InitialData();
        SearchLibrary.BuildKeyphraseGraph process = new SearchLibrary.BuildKeyphraseGraph();
        SearchLibrary.EntityModel.GraphEntity queryKG = process.BuildKeyphraseGraphForQuery(query);
        if (queryKG == null)
                return null;
        double? rank;
        foreach (anArticle in articleList)
        {
          SearchLibrary.EntityModel.GraphEntity graphG = process.BuildAnKeyphraseGraph(anArticle.articleKG);
          rank = SearchLibrary.SemanticSearch.GetRank(queryKG, graphG);
        }
         * Chu y: rank>0.05
        */



        //*********Tinh hang cua 1 tin bai co lien quan toi tinh Binh Duong**********
        //SearchLibrary.RelatedBinhDuong.InitialData()
        //SearchLibrary.RelatedBinhDuong.GetRank(_title, _abstract, _content, _tags)

        /*Vi du:         articleList: danh sach tin bai
        SearchLibrary.RelatedBinhDuong.InitialData();
        double rank;
        foreach (anArticle in articleList)
        {
          rank = SearchLibrary.RelatedBinhDuong.GetRank(anArticle.title, anArticle.abstract, anArticle.content, anArticle.tags);
        }
         Chu y: rank>0.5 
         
        */



        //*********Tinh hang cua 1 chu de va 1 tin bai**********
        //SearchLibrary.SearchByTopic.InitialData()
        //SearchLibrary.SearchByTopic.GetKeyphraseIDFromTopic(topics)
        //SearchLibrary.SearchByTopic.GetRankForTopic(keyphraseIdFromTopic, articleKG)
        //trong do: //topics: chuoi chua danh sach keyphrase cua 1 chu de. Quy uoc cho topics nhu sau: 
                                //topics = (id1, keyphrase1), (id2, keyphrase2), ..., (idn, keyphrasen)
                    //articleKG: chuoi chua do thi keyphrase
                    //keyphraseIdFromTopic: kieu List<long>   (danh sach id cac keyphrase trong topics)
        //Chu y: lay rank >=0.05

        /*Vi du:    articleList: danh sach tin bai   
        SearchLibrary.SearchByTopic.InitialData();
        List<long> keyphraseIdFromTopic = SearchLibrary.SearchByTopic.GetKeyphraseIDFromTopic(topics);
        double rank;
        foreach (anArticle in articleList)
        {
           rank = SearchLibrary.SearchByTopic.GetRankForTopic(keyphraseIdFromTopic, anArticle.articleKG);
        }*/




        //*********Ham tinh muc do giong nhau giua 2 tin bai A va B**********
        //SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromArticleKG(articleKG)
        //SearchLibrary.Helper.NearDuplicateArticle.GetRelevance(keyphraseA, keyphraseB)
        //trong do: articleKG la chuoi chua do thi keyphrase
        //          keyphraseA la List<KeyphraseEntity>, keyphraseB la List<KeyphraseEntity>

        /*Vi du: 
        articleList: danh sach tin bai
        double rank;
        for (int i=0;i<articleList.Count-1;i++)
        {
           List<KeyphraseEntity> keyphraseA = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromArticleKG(articleList[i].articleKG);
            for (int j=i+1;j<articleList.Count;j++)
            {
                List<KeyphraseEntity> keyphraseB = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromArticleKG(articleList[j].articleKG);
                relevance = SearchLibrary.NearDuplicateArticle.GetRelevance(keyphraseA, keyphraseB);
                if (relevance>0.8)
                    HAI TIN BAI GIONG NHAU
            }
        }
        */


        //*********Lay keyphrase tu database**********
        //List<SearchLibrary.DatabaseModel.Keyphrase> keyphraseList = SearchLibrary.Helper.KeyphraseUtility.GetKeyphraseFromDatabase();
    }
}
