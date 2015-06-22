using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaNews.Collector
{
    public delegate void URLVisited(CollectorUnitBase unit, string url, string text);
    public delegate void ArticleCollected(CollectorUnitBase unit, SemaNewsCore.Models.Article article);
    public delegate void MessageAdded(CollectorUnitBase unit, string message);
    public delegate void CollectorUnitState(CollectorUnitBase unit);

    public abstract class CollectorUnitBase
    {
        protected int _articleCount = 0;
        public SemaNewsCore.Models.Field Field { get; set; }
        public SemaNewsCore.Models.Newspaper Newspaper { get; set; }
        public SemaNewsCore.Configurations.CollectorStatus Status { get; set; }
        public int ArticleCount
        {
            get { return _articleCount; }
        }
        public float Progress { get; set; }

        public event URLVisited URLVisited;
        public event ArticleCollected NewArticleCollected;
        public event MessageAdded NewMesssageAdded;
        public event CollectorUnitState CollectorUnitFinished;
        public event CollectorUnitState CollectorUnitStarted;

        protected void OnURLVisted(string url, string text)
        {
            if (this.URLVisited != null && !string.IsNullOrEmpty(url))
                URLVisited(this, url, text);
        }

        protected void OnNewArticleCollected(SemaNewsCore.Models.Article article)
        {
            if (this.NewArticleCollected != null)
            {
                NewArticleCollected(this, article);
                Interlocked.Increment(ref _articleCount);
                UpdateProgress();
            }
        }

        protected void OnNewMessageAdded(string message)
        {
            if (this.NewMesssageAdded != null)
                NewMesssageAdded(this, message);
        }

        protected void OnCollectorUnitFinished()
        {
            if (this.CollectorUnitFinished != null)
                CollectorUnitFinished(this);
        }

        protected void OnCollectorUnitStarted()
        {
            if (this.CollectorUnitStarted != null)
                CollectorUnitStarted(this);
        }

        public CollectorUnitBase(int fieldId)
        {
            using (SemaNewsCore.Models.SemaNewsDBContext db = new SemaNewsCore.Models.SemaNewsDBContext())
            {
                this.Field = db.Fields.Find(fieldId);
                if (this.Field == null)
                    throw new Exception("Field with ID {0} is not found");

                this.Newspaper = this.Field.Newspaper;
                this.Status = SemaNewsCore.Configurations.CollectorStatus.Stopped;
                this.Progress = 0f;
                _articleCount = 0;
            }
        }

        public virtual void Start()
        {
            this.Status = SemaNewsCore.Configurations.CollectorStatus.Started;
            OnCollectorUnitStarted();
            this.CollectNews();
            Stop();
        }

        public virtual async Task StartAsync()
        {
            this.Status = SemaNewsCore.Configurations.CollectorStatus.Started;
            OnCollectorUnitStarted();
            await Task.Run(() => this.CollectNews());
            Stop();
        }

        public virtual void Stop()
        {
            this.Status = SemaNewsCore.Configurations.CollectorStatus.Stopped;
            OnCollectorUnitFinished();
        }

        protected abstract void CollectNews();

        protected abstract void UpdateProgress();
    }
}
