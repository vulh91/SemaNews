//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CreateKeyphraseGraphLibrary.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Class_ClassRelationship
    {
        public long SuperClassId { get; set; }
        public long SubClassId { get; set; }
        public long RelationshipId { get; set; }
    
        public virtual ClassRelationship ClassRelationship { get; set; }
        public virtual KeyphraseClass KeyphraseClass { get; set; }
        public virtual KeyphraseClass KeyphraseClass1 { get; set; }
    }
}
