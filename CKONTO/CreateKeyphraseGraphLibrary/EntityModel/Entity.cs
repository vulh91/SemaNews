using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateKeyphraseGraphLibrary.EntityModel
{
    public class NumberKeyphraseEntity
    {
        public long Id;
        public string keyphrase;
        public long number;
    }

    public class NameEntity
    {
        public long Id;
        public string Name;
    }

    public class KKRelationEntity
    {
        public string Keyphrase1;
        public string Keyphrase2;
        public string NameRelation;
        public long idrelation;
    }
}
