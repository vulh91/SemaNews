using SemaNews.Collector.Properties;
using SemaNewsCore;
using SemaNewsCore.Configurations;
using SemaNewsCore.Models;
using SemaNewsSearchEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SemaNews.Collector
{
    public class CollectorEngine : INotifyPropertyChanged, IDisposable
    {
        public static int CollectorCheckingInterval = 1000;
        public static int ProgressCheckingInterval = 10;
        public static int Need2IndexCheckingInterval = 30000;
        public static int RemoveVistedLinkInterval = 1000;
        public static TimeSpan DefaultCollectingTime = new TimeSpan(6, 0, 0);

        public static string CategoryName_LDVL = Settings.Default.CatgName_LDVL;
        public static string CategoryName_DTCDTNN = Settings.Default.CatgName_DTCDTNN;

        public float OverallProgress { get; private set; }
        private bool isIndexingGraph = false;
        private bool isIndexText = false;
        private bool isCheckingRelevant2LocalProvince = false;

        public List<Article> ArticlesCollected
        {
            get
            {
                if (results == null)
                    results = new List<Article>();
                return results;
            }
            set
            {
                if (results == null)
                    results = new List<Article>();
                if (results != value)
                {
                    results = value;
                    RaisePropertyChanged(() => ArticlesCollected);
                }
            }
        }
        public List<Article> Articles_LDVL;
        public List<Article> Articles_DT;

        private CollectorInfo collectorInfo;
        private System.Timers.Timer timerCheckingCollector;
        private System.Timers.Timer timerCheckingProgress;
        private System.Timers.Timer timerIndexContentForArticles;
        private System.Timers.Timer timerRemoveVisitedLinks;

        private List<CollectorUnitBase> collectorUnits;
        private List<Article> results;

        public DelChangedEventHandler EventNewMessageAdded;
        public DelChangedEventHandler EventNewArticleCollected;
        public event URLVisited NewURLVisited;
        public event PropertyChangedEventHandler PropertyChanged;

        public CollectorInfo CollectorInfo
        {
            get
            {
                if (collectorInfo == null)
                    collectorInfo = CollectorConfigManager.CollectorInfo;
                return collectorInfo;
            }
            set
            {
                if (collectorInfo == null)
                    collectorInfo = CollectorConfigManager.CollectorInfo;
                collectorInfo = value;
                RaisePropertyChanged(() => CollectorInfo);
            }
        }

        public CollectorEngine()
        {
            timerCheckingCollector = new System.Timers.Timer(CollectorCheckingInterval);
            timerCheckingCollector.Elapsed += timerCounter_Elapsed;

            timerCheckingProgress = new System.Timers.Timer(ProgressCheckingInterval);
            timerCheckingProgress.Elapsed += timerCheckingProgress_Elapsed;

            timerIndexContentForArticles = new System.Timers.Timer(Need2IndexCheckingInterval);
            timerIndexContentForArticles.Elapsed += IndexForArticles;
            timerIndexContentForArticles.Start();

            timerRemoveVisitedLinks = new System.Timers.Timer(RemoveVistedLinkInterval);
            timerRemoveVisitedLinks.Elapsed += timerRemoveVisitedLinks_Elapsed;
            timerRemoveVisitedLinks.Start();

            CollectorInfo.Status = CollectorStatus.Stopped;
            SaveCollectorInfo();

            CreateKeyphraseGraphLibrary.KeyphraseGraph.InitialData();
            CreateKeyphraseGraphLibrary.KeyphraseGraph_DTCDTNN.InitialData();
            SearchLibrary.SemanticSearch.InitialData();
            SearchLibrary.SemanticSearch_DTCDTNN.InitialData();
            SearchLibrary.RelatedBinhDuong.InitialData();
            SearchLibrary.SearchByTopic.InitialData();

            ArticlesCollected = new List<Article>();
            Articles_DT = new List<Article>();
            Articles_LDVL = new List<Article>();
        }

        void timerRemoveVisitedLinks_Elapsed(object sender, ElapsedEventArgs e)
        {
            var allowPointOfTime = DateTime.Now.AddDays(-7);
            int minimumVisitCount = 2;
            using(SemaNewsDBContext db = new SemaNewsDBContext())
            {

                var links = db.VisitedLinks.Where(m => m.Time <=allowPointOfTime
                    && (!string.IsNullOrEmpty(m.Name) || m.VisitCount <= minimumVisitCount));
                db.VisitedLinks.RemoveRange(links);
                db.SaveChanges();
            }
        }

        protected virtual void RaisePropertyChanged<TResult>(Expression<Func<TResult>> selector)
        {
            if (PropertyChanged == null) return;
            var memberExpression = selector.Body as MemberExpression;
            if (memberExpression != null)
                PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        private void timerCheckingProgress_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateProgress();
            Console.WriteLine("PROGRESS {0}", this.OverallProgress);
        }

        private void timerCounter_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (NeedToCollectNews())
                CollectNewsAsync();
            else if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true && CollectorConfigManager.RequestStopCollect)
            {
                StopCollectNews();
                CollectorConfigManager.RequestStopCollect = false;
            }
        }

        private void IndexForArticles(object sender, ElapsedEventArgs e)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var articles = db.Articles.Where(m => m.IsIndexed == false).ToList();
                if (articles != null && articles.Count != 0)
                {
                    AddNewMessage(string.Format("TRYING TO INDEX TEXT FOR {0} ARTICLES", articles.Count));
                    ArticleIndexer.AddUpdateLuceneIndex(articles);
                    for (int i = 0; i < articles.Count; i++)
                    {
                        var article = articles[i];
                        article.IsIndexed = true;
                    }
                    db.SaveChanges();
                    AddNewMessage(string.Format("FINISHED INDEX TEXT FOR {0} ARTICLES", articles.Count));
                }
            }

             using(SemaNewsDBContext db = new SemaNewsDBContext())
             {
                 var articles = db.Articles.Where(m => m.IsRelevantToLocal.HasValue == false).ToList();
                 if (articles != null && articles.Count != 0)
                 {
                     AddNewMessage(string.Format("TRYING TO ESTIMATE THE RELEVANCE OF {0} ARTICLES TO THE PROVINCE", articles.Count));
                     for (int i = 0; i < articles.Count; i++)
                     {
                         var article = articles[i];
                         if (SearchLibrary.RelatedBinhDuong.GetRank(
                                 SemaNews.Utilities.HtmlHandler.StripHtmlTags(article.Title),
                                 SemaNews.Utilities.HtmlHandler.StripHtmlTags(article.Abstract),
                                 SemaNews.Utilities.HtmlHandler.StripHtmlTags(article.Content),
                                 SemaNews.Utilities.HtmlHandler.StripHtmlTags(article.Tags)) > 0.5f)
                             article.IsRelevantToLocal = true;
                         else
                             article.IsRelevantToLocal = false;
                         db.SaveChanges();
                     }
                     AddNewMessage(string.Format("FINISH ESTIMATION THE RELEVANCE OF {0} ARTICLES TO THE PROVINCE", articles.Count));
                 }
             }
        }

        private bool NeedToIndexArticles()
        {
            try
            {
                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var category_LDVL = db.GFields.FirstOrDefault(m => m.Name == SemanticDomains.LDVL);
                    var category_DT = db.GFields.FirstOrDefault(m => m.Name == SemanticDomains.DTC_DTNN);

                    if (category_LDVL != null)
                        Articles_LDVL = category_LDVL.GetArticleByRelation(db, NRelationNotation.RELATED).Where(m => m.ArticleKG == null || m.ArticleKG.LDVL_Graph == null).Take(100).ToList();

                    if (category_DT != null)
                        Articles_DT = category_DT.GetArticleByRelation(db, NRelationNotation.RELATED).Where(m => m.ArticleKG == null || m.ArticleKG.DT_Graph == null).Take(100).ToList();

                    if (Articles_DT != null && Articles_DT.Count != 0)
                        return true;

                    if (Articles_LDVL != null && Articles_LDVL.Count != 0)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void SaveCollectorInfo()
        {
            CollectorConfigManager.CollectorInfo = this.CollectorInfo;
        }

        private bool NeedToCollectNews()
        {
            try
            {
                //Not collect if engine is busy currently
                if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true)
                    return false;

                //Check if any manual request has just been made
                if (CollectorConfigManager.RequestToCollect == true)
                {
                    CollectorConfigManager.RequestToCollect = false;
                    return true;
                }

                //Not collect if collecting mode is MANUAL
                if (CollectorConfigManager.CollectingMode == CollectingMode.Manual)
                    return false;

                //Determine based on current selected collecting mode
                switch (CollectorConfigManager.CollectingApproach)
                {
                    case CollectingApproach.Auto:
                        var defaultCollectingTime = DateTime.Today + DefaultCollectingTime;
                        if (defaultCollectingTime <= DateTime.Now && defaultCollectingTime > CollectorConfigManager.CollectorInfo.EndTime)
                            return true;
                        break;
                    case CollectingApproach.Continuous:
                        var sleepTime = CollectorConfigManager.CollectingDelay;
                        if (CollectorConfigManager.CollectorInfo.EndTime + sleepTime <= DateTime.Now)
                            return true;
                        break;
                    case CollectingApproach.Interval:
                        var intervalTime = CollectorConfigManager.CollectingInterval;
                        if (CollectorConfigManager.CollectorInfo.StartTime + intervalTime <= DateTime.Now)
                            return true;
                        break;
                    case CollectingApproach.PointOfTimes:
                        var times = CollectorConfigManager.CollectingTimes;
                        if (times.Any(t => DateTime.Today + t.TimeOfDay <= DateTime.Now
                            && DateTime.Today + t.TimeOfDay > CollectorConfigManager.CollectorInfo.EndTime))
                            return true;
                        break;
                }
                return false;
            }
            catch (Exception e)
            {
                AddNewMessage(string.Format("ERROR:\"{0}\"", e.Message));
                return false;
            }
        }

        private void UpdateProgress()
        {
            OverallProgress = 0f;
            if (collectorUnits == null || collectorUnits.Count == 0)
                return;

            float unitPoint = 100f / collectorUnits.Count;

            foreach (var collectorUnit in collectorUnits)
            {
                float progressPoint = 0;
                float limitUnitPoint = unitPoint * 99 / 100;
                if (collectorUnit.Status == CollectorStatus.Stopped)
                    progressPoint = unitPoint;
                else
                {
                    progressPoint = (float)(collectorUnit.ArticleCount * unitPoint) / 50;
                    if (progressPoint > limitUnitPoint)
                        progressPoint = limitUnitPoint;
                }
                OverallProgress += progressPoint;
                CollectorInfo.Progress = OverallProgress;
            }

            if (OverallProgress == 100 || (collectorUnits != null && collectorUnits.Count != 0 && collectorUnits.Where(m => m.Status == CollectorStatus.Started).Count() == 0))
            {
                OverallProgress = 0;
                timerCheckingProgress.Stop();
                CollectorInfo.IsCollectorBusy = false;
                CollectorInfo.EndTime = DateTime.Now;
                CollectorInfo.Progress = 0;
                this.collectorUnits.Clear();
                this.AddNewMessage(string.Format("A collecting proccess has finished at {0}", DateTime.Now));
                SaveCollectorInfo();
            }

            SaveCollectorInfo();
        }

        public async void CollectNewsAsync()
        {
            try
            {
                AddNewMessage(string.Format("Start collecting process at {0}", DateTime.Now));

                CollectorInfo.IsCollectorBusy = true;
                CollectorInfo.StartTime = DateTime.Now;
                CollectorInfo.EndTime = null;
                CollectorInfo.ArticlesCount = 0;
                CollectorInfo.RecentCollectedArticle = null;
                SaveCollectorInfo();

                collectorUnits = new List<CollectorUnitBase>();
                ArticlesCollected.Clear();

                //Get all activated newspapers and collect news foreach
                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var newspapers = db.Newspapers.Where(m => m.IsActivated == true);
                    foreach (var newspaper in newspapers)
                        foreach (var field in newspaper.Fields)
                            if (field.IsActivated == true)
                            {
                                var newCollectorUnit = new NormalCollectorUnit(field.Id);
                                collectorUnits.Add(newCollectorUnit);

                                newCollectorUnit.NewArticleCollected += newCollectorUnit_NewArticleCollected;
                                newCollectorUnit.NewMesssageAdded += newCollectorUnit_NewMesssageAdded;
                                newCollectorUnit.URLVisited += newCollectorUnit_URLVisited;
                                newCollectorUnit.CollectorUnitStarted += newCollectorUnit_CollectorUnitStarted;
                                newCollectorUnit.CollectorUnitFinished += newCollectorUnit_CollectorUnitFinished;
                            }
                }

                foreach(var item in collectorUnits)
                {
                    await item.StartAsync();
                }
                timerCheckingProgress.Start();
            }
            catch
            {
                //Logs here
            }
        }

        void newCollectorUnit_NewArticleCollected(CollectorUnitBase unit, Article article)
        {
            ArticlesCollected.Add(article);
            CollectorInfo.RecentCollectedArticle = article;
            CollectorInfo.ArticlesCount = ArticlesCollected.Count;
            OnNewArticleCollected(article);
            Console.WriteLine("A new article has been collected by unit {0}-{1}:{2}", unit.Newspaper.Id, unit.Field.Id, article.Url);

            SaveCollectorInfo();
        }

        void newCollectorUnit_NewMesssageAdded(CollectorUnitBase unit, string message)
        {
            AddNewMessage(message);
        }

        private void newCollectorUnit_CollectorUnitFinished(CollectorUnitBase unit)
        {
            AddNewMessage(string.Format("Collector Unit for {0} - {1} has FINISHED", unit.Newspaper.Name, unit.Field.Name));
        }

        void newCollectorUnit_CollectorUnitStarted(CollectorUnitBase unit)
        {
            AddNewMessage(string.Format("Collector Unit for {0} - {1} has STARTED", unit.Newspaper.Name, unit.Field.Name));
        }

        void newCollectorUnit_URLVisited(CollectorUnitBase unit, string url, string text)
        {
            OnNewURLVisited(unit, url, text);
        }

        public void StopCollectNews()
        {
            if (collectorInfo.IsCollectorBusy == true)
            {
                if (collectorUnits != null && collectorUnits.Count != 0)
                    collectorUnits.Clear();

                collectorInfo.EndTime = DateTime.Now;
                collectorInfo.Progress = 0;
                collectorInfo.IsCollectorBusy = false;
            }
            SaveCollectorInfo();
        }

        public void IndexGraphForArticles()
        {
            try
            {
                if (Articles_LDVL != null && Articles_LDVL.Count != 0)
                {
                    AddNewMessage(string.Format("{0}:TRYING TO GENERATE KEYPHRASE GRAPHS FOR {1} ARTICLES", SemaNewsCore.SemanticDomains.LDVL, Articles_LDVL.Count));
                    ArticleIndexer.GenerateGraphs(Articles_LDVL, SemaNewsCore.SemanticDomains.LDVL);
                }
                if (Articles_DT != null && Articles_DT.Count != 0)
                {
                    AddNewMessage(string.Format("{0}:TRYING TO GENERATE KEYPHRASE GRAPHS FOR {1} ARTICLES", SemaNewsCore.SemanticDomains.DTC_DTNN, Articles_DT.Count));
                    ArticleIndexer.GenerateGraphs(Articles_DT, SemaNewsCore.SemanticDomains.DTC_DTNN);
                }
                Articles_LDVL.Clear();
                Articles_DT.Clear();
                isIndexingGraph = false;
            }
            catch (Exception ex)
            {
                AddNewMessage(ex.Message);
            }
        }

        private void AddNewMessage(string msg)
        {
            if (EventNewMessageAdded != null)
                EventNewMessageAdded(this, msg);
        }

        private void OnNewArticleCollected(Article article)
        {
            if (EventNewArticleCollected != null)
                EventNewArticleCollected(this, article);
        }

        private void OnNewURLVisited(CollectorUnitBase unit, string url, string text)
        {
            if (this.NewURLVisited != null)
                this.NewURLVisited(unit, url, text);
        }

        public void Start()
        {
            timerCheckingCollector.Start();
            CollectorInfo.Status = CollectorStatus.Started;

            SaveCollectorInfo();
        }

        public void Stop()
        {
            timerCheckingCollector.Stop();
            CollectorInfo.Status = CollectorStatus.Stopped;

            SaveCollectorInfo();
        }

        public void Reset()
        {
            Stop();
            timerCheckingCollector.Interval = CollectorCheckingInterval;
            collectorUnits = new List<CollectorUnitBase>();
            ArticlesCollected = new List<Article>();
            Articles_LDVL = new List<Article>();
            Articles_DT = new List<Article>();
            OverallProgress = 0;
            Start();
        }

        public void Dispose()
        {
            //if (collectorThreads != null && collectorThreads.Count != 0)
            //{
            //    foreach (var thread in collectorThreads)
            //    {
            //        thread.Abort();
            //    }

            //}
        }
    }
}
