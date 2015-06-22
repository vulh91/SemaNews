﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DTC_DTNN_CKONTOLOGYCreateKGEntities : DbContext
    {
        public DTC_DTNN_CKONTOLOGYCreateKGEntities()
            : base("name=DTC_DTNN_CKONTOLOGYCreateKGEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Alpha> Alphas { get; set; }
        public DbSet<Beta> Betas { get; set; }
        public DbSet<Class_ClassRelationship> Class_ClassRelationship { get; set; }
        public DbSet<ClassRelationship> ClassRelationships { get; set; }
        public DbSet<IPWeight> IPWeights { get; set; }
        public DbSet<Keyphrase> Keyphrases { get; set; }
        public DbSet<Keyphrase_KeyphraseClass> Keyphrase_KeyphraseClass { get; set; }
        public DbSet<Keyphrase_KeyphraseRelationship> Keyphrase_KeyphraseRelationship { get; set; }
        public DbSet<KeyphraseClass> KeyphraseClasses { get; set; }
        public DbSet<KeyphraseGraph> KeyphraseGraphs { get; set; }
        public DbSet<KeyphraseRelationship> KeyphraseRelationships { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PartOfSpeech> PartOfSpeeches { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Phrase> Phrases { get; set; }
        public DbSet<POSDictionary> POSDictionaries { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Topic> Topics { get; set; }
    }
}
