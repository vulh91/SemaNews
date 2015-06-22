using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SemaNewsCore.Models.Mapping;

namespace SemaNewsCore.Models
{
    public partial class SemaNewsDBContext : DbContext
    {
        static SemaNewsDBContext()
        {
            Database.SetInitializer<SemaNewsDBContext>(null);
        }

        public SemaNewsDBContext()
            : base("Name=SemaNewsDBContext")
        {
        }

        public DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public DbSet<AARelationInstance> AARelationInstances { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleKG> ArticleKGs { get; set; }
        public DbSet<ArticleWebElement> ArticleWebElements { get; set; }
        public DbSet<CollectorConfiguration> CollectorConfigurations { get; set; }
        public DbSet<FFRelationInstance> FFRelationInstances { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldWebElement> FieldWebElements { get; set; }
        public DbSet<GField> GFields { get; set; }
        public DbSet<GGRelation> GGRelations { get; set; }
        public DbSet<GGRelationInstance> GGRelationInstances { get; set; }
        public DbSet<Newspaper> Newspapers { get; set; }
        public DbSet<NNRelationInstance> NNRelationInstances { get; set; }
        public DbSet<NRelation> NRelations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserQuery> UserQueries { get; set; }
        public DbSet<WebElementType> WebElementTypes { get; set; }
        public DbSet<VisitedLink> VisitedLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new C__RefactorLogMap());
            modelBuilder.Configurations.Add(new AARelationInstanceMap());
            modelBuilder.Configurations.Add(new ArticleMap());
            modelBuilder.Configurations.Add(new ArticleKGMap());
            modelBuilder.Configurations.Add(new ArticleWebElementMap());
            modelBuilder.Configurations.Add(new CollectorConfigurationMap());
            modelBuilder.Configurations.Add(new FFRelationInstanceMap());
            modelBuilder.Configurations.Add(new FieldMap());
            modelBuilder.Configurations.Add(new FieldWebElementMap());
            modelBuilder.Configurations.Add(new GFieldMap());
            modelBuilder.Configurations.Add(new GGRelationMap());
            modelBuilder.Configurations.Add(new GGRelationInstanceMap());
            modelBuilder.Configurations.Add(new NewspaperMap());
            modelBuilder.Configurations.Add(new NNRelationInstanceMap());
            modelBuilder.Configurations.Add(new NRelationMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SavedArticleMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TopicMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new UserQueryMap());
            modelBuilder.Configurations.Add(new WebElementTypeMap());
            modelBuilder.Configurations.Add(new VisitedLinkMap());
        }
    }
}
