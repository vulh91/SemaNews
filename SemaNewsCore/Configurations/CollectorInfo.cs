using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Configurations
{
    public class CollectorInfo : INotifyPropertyChanged
    {
        private CollectorStatus collectorStatus;
        private float progress;
        private DateTime? startTime;
        private DateTime? endTime;
        private int articlesCount;
        private Article recentCollectedArticle;
        private bool isCollectorBusy;

        public CollectorStatus Status
        {
            get
            {
                return collectorStatus;
            }
            set
            {
                if (collectorStatus != value)
                {
                    collectorStatus = value;
                    RaisePropertyChanged(() => Status);

                }
            }
        }
        public float Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged(() => Progress);
                }
            }
        }
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    RaisePropertyChanged(() => StartTime);
                }
            }
        }
        public DateTime? EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    RaisePropertyChanged(() => EndTime);
                }
            }
        }
        public int ArticlesCount
        {
            get { return articlesCount; }
            set
            {
                if (articlesCount != value)
                {
                    articlesCount = value;
                    RaisePropertyChanged(() => ArticlesCount);
                }
            }
        }
        public Article RecentCollectedArticle
        {
            get { return recentCollectedArticle; }
            set
            {
                if (recentCollectedArticle != value)
                {
                    recentCollectedArticle = value;
                    RaisePropertyChanged(() => RecentCollectedArticle);
                }
            }
        }
        public bool IsCollectorBusy
        {
            get
            {
                return isCollectorBusy;
            }
            set
            {
                if (isCollectorBusy != value)
                {
                    isCollectorBusy = value;
                    RaisePropertyChanged(() => IsCollectorBusy);
                }
            }
        }

        public CollectorInfo()
        {
        }

        protected virtual void RaisePropertyChanged<TResult>(Expression<Func<TResult>> selector)
        {
            if (PropertyChanged == null) return;
            var memberExpression = selector.Body as MemberExpression;
            if (memberExpression != null)
                PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        public string ToJsonString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static CollectorInfo ParseInfo(string infoStr)
        {
            return (CollectorInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(infoStr, typeof(CollectorInfo));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
