using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.EntityModel
{
    public class GraphEntity
    {
        public List<KeyphraseEntity> keyphraseList;
        public List<KeyphraseEntity> isolateKeyphraseList;
        public List<RelationEntity> relationList;
    }
    public class KeyphraseEntity
    {
        public long id;
        public string keyphrase;
        public double tf;
        public double ip;
    }
    public class RelationEntity
    {
        public KeyphraseEntity keyphrase1;
        public KeyphraseEntity keyphrase2;
        public string nameRelation;
        public long idRelation;
    }
    public class AlphaEntity
    {
        public long idKeyphInH; //neu can test thi dung, khong thi khoi dung de tang toc do
        public long idKeyphInG;
        public float? valueAlpha;
        public double tfKeyphInG;
        public double ipKeyphInG;
    }

    public class RelatedRelationEntity
    {
        public RelationEntity relation;
        public float? value;
    }
    public class RelatedKeyphraseEntity
    {
        public KeyphraseEntity k;
        public float? value;
    }
    public class IntersectTFKeyphraseEntity
    {
        public double tfA;
        public double tfB;
    }
}
