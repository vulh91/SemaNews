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
    
    public partial class Location
    {
        public long Id { get; set; }
        public string LocationName { get; set; }
        public Nullable<double> LocationWeight { get; set; }
        public string Description { get; set; }
    }
}