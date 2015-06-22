using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.EntityModel
{
    public class NumberKeyphraseEntity
    {
        public long Id;
        public string keyphrase;
        public long number;
    }
    public class POSEntity
    {
        public long Id;
        public string Keyphrase;
        public string Type;
        public string FormOrder;
    }
    public class NameEntity
    {
        public long Id;
        public string Name;
    }
    public class ExtractKeyphraseEntity
    {
        public long Id;
        public string Keyphrase;
        public string FormOrder;
    }
    public class KKRelationEntity
    {
        public string Keyphrase1;
        public string Keyphrase2;
        public string NameRelation;
        public long idrelation;
    }
}
