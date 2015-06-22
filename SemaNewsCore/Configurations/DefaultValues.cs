using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsCore.Configurations
{
    public class DefaultValues
    {
        public static CollectingApproach CollectingApproach = Configurations.CollectingApproach.Auto;
        public static TimeSpan CollectingInterval = new TimeSpan(12, 0, 0);
        public static TimeSpan CollectingDelay = new TimeSpan(12, 0, 0);
        public static List<DateTime> CollectingTimes = new List<DateTime>()
        {
            new DateTime(1,1,1, 0,0,0),
            new DateTime(1,1,1, 12,0,0),
        };
        public static bool IsPagingCollecting = false;
        public static int QtyOfArticlesExpect = 100;
        public static CollectingMode CollectingMode = CollectingMode.Scheduled;
        public static bool IsCollectorBusy = false;
        public static DateTime LastUpdateTime = new DateTime();
        public static bool RequestToCollectNews = false;
        public static bool RequestStopCollectNews = false;
        public static CollectorInfo CollectorInfo = new CollectorInfo();
        public static int NumberOfPageToCrawl = 1;
    }
}
