using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Configurations
{
    public static class CollectorConfigManager
    {
        public static CollectingApproach CollectingApproach
        {
            get
            {
                try
                {
                    var approachValueString = GetConfigInfo(CollectorPropNames.PropCollectingApproach);
                    return (CollectingApproach)Enum.Parse(typeof(CollectingApproach), approachValueString, true);
                }
                catch
                {
                    return DefaultValues.CollectingApproach;
                }
            }
            set
            {
                var approachValueStr = value;
                AdjustConfig(CollectorPropNames.PropCollectingApproach, approachValueStr.ToString());
            }
        }

        public static TimeSpan CollectingInterval
        {
            get
            {
                try
                {
                    var timeSpanStr = GetConfigInfo(CollectorPropNames.PropCollectingInterval);
                    return TimeSpan.Parse(timeSpanStr);
                }
                catch
                {
                    return DefaultValues.CollectingInterval;
                }
            }
            set
            {
                var timeSpanStr = value.ToString();
                AdjustConfig(CollectorPropNames.PropCollectingInterval, timeSpanStr);
            }
        }

        public static TimeSpan CollectingDelay
        {
            get
            {
                try
                {
                    var timeSpanStr = GetConfigInfo(CollectorPropNames.PropCollectingDelay);
                    return TimeSpan.Parse(timeSpanStr);
                }
                catch
                {
                    return DefaultValues.CollectingDelay;
                }
            }

            set
            {
                var timeSpanStr = value.ToString();
                AdjustConfig(CollectorPropNames.PropCollectingDelay, timeSpanStr);
            }
        }

        public static IEnumerable<DateTime> CollectingTimes
        {
            get
            {
                try
                {
                    var timesStr = GetConfigInfo(CollectorPropNames.PropCollectingTimes);
                    if (string.IsNullOrEmpty(timesStr))
                        return new List<DateTime>();
                    return timesStr.Split(',', ';', '|').Select(m => DateTime.Parse(m)).OrderBy(m => m.TimeOfDay);
                }
                catch
                {
                    return DefaultValues.CollectingTimes;
                }
            }
            set
            {
                var timeStr = string.Join(";",value.Select(m => m.TimeOfDay.ToString()));
                AdjustConfig(CollectorPropNames.PropCollectingTimes, timeStr);
            }
        }

        public static int NumberOfPageToCrawl
        {
            get
            {
                try
                {
                    return int.Parse(GetConfigInfo(CollectorPropNames.PropNumberOfPageToCrawl));
                }
                catch
                {
                    return DefaultValues.NumberOfPageToCrawl;
                }

            }
            set
            {
                AdjustConfig(CollectorPropNames.PropNumberOfPageToCrawl, value.ToString());
            }
        }

        public static int QtyOfArticlesExpect
        {
            get
            {
                try
                {
                    return int.Parse(GetConfigInfo(CollectorPropNames.PropQtyOfArticlesExpect));
                }
                catch
                {
                    return DefaultValues.QtyOfArticlesExpect;
                }
            }
            set
            {
                AdjustConfig(CollectorPropNames.PropQtyOfArticlesExpect, value.ToString());
            }
        }

        public static CollectingMode CollectingMode
        {
            get
            {
                try
                {
                    var modeStr = GetConfigInfo(CollectorPropNames.PropCollectingMode);
                    return (CollectingMode)Enum.Parse(typeof(CollectingMode), modeStr, true);
                }
                catch
                {
                    return DefaultValues.CollectingMode;
                }
            }
            set
            {
                var collectingMode = value;
                AdjustConfig(CollectorPropNames.PropCollectingMode, collectingMode.ToString());
            }
        }

        public static CollectorInfo CollectorInfo
        {
            get
            {
                try
                {
                    var infoStr = GetConfigInfo(CollectorPropNames.PropCollectorInfo);
                    var collectorInfo = CollectorInfo.ParseInfo(infoStr);
                    if (collectorInfo == null)
                        throw new FormatException();
                    return collectorInfo;
                }
                catch
                {
                    return DefaultValues.CollectorInfo;
                }
            }
            set
            {
                AdjustConfig(CollectorPropNames.PropCollectorInfo, value.ToJsonString());
            }
        }

        public static bool RequestToCollect
        {
            get
            {
                try
                {
                    return bool.Parse(GetConfigInfo(CollectorPropNames.PropRequestCollectNews));
                }
                catch
                {
                    return DefaultValues.RequestToCollectNews;
                }
            }
            set
            {
                AdjustConfig(CollectorPropNames.PropRequestCollectNews, value.ToString());
            }
        }

        public static bool RequestStopCollect
        {
            get
            {
                try
                {
                    return bool.Parse(GetConfigInfo(CollectorPropNames.PropRequestStopCollectNews));
                }
                catch
                {
                    return DefaultValues.RequestStopCollectNews;
                }
            }
            set
            {
                AdjustConfig(CollectorPropNames.PropRequestStopCollectNews, value.ToString());
            }
        }

        private static void AdjustConfig(string property, string value)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var prop = db.CollectorConfigurations.FirstOrDefault(m => m.Name == property);
                if (prop == null)
                {
                    prop = new CollectorConfiguration()
                    {
                        Name = property,
                    };
                    db.CollectorConfigurations.Add(prop);
                    db.Entry(prop).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                }
                prop.Value = value;
                db.Entry(prop).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        private static string GetConfigInfo(string property)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var prop = db.CollectorConfigurations.FirstOrDefault(m => m.Name == property);
                return prop == null ? "" : prop.Value;
            }
        }
    }
}
