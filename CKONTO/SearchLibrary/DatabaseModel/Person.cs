//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SearchLibrary.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public long Id { get; set; }
        public string PersonName { get; set; }
        public string IdentityString { get; set; }
        public string IdentitySynonym { get; set; }
        public Nullable<double> PersonWeight { get; set; }
        public string Description { get; set; }
    }
}
