using SemaNewsCore.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class CollectorConfigVM
    {
        public CollectingMode CollectingMode { get; set; }
        public CollectingApproach CollectingApproach { get; set; }
        public TimeSpan CollectingInterval { get; set; }
        public TimeSpan CollectingDelay { get; set; }
        public IEnumerable<DateTime> CollectingTimes { get; set; }
        public List<string> Approaches { get; set; }
        public List<string> CollectingModes { get; set; }
        public int NumberOfPageToCrawl { get; set; }
        public CollectorConfigVM()
        {
            this.CollectingMode = CollectorConfigManager.CollectingMode;
            this.CollectingApproach = CollectorConfigManager.CollectingApproach;
            this.CollectingInterval = CollectorConfigManager.CollectingInterval;
            this.CollectingDelay = CollectorConfigManager.CollectingDelay;
            this.CollectingTimes = CollectorConfigManager.CollectingTimes;
            this.NumberOfPageToCrawl = CollectorConfigManager.NumberOfPageToCrawl;

            Approaches = Enum.GetNames(typeof(CollectingApproach)).ToList();
            CollectingModes = Enum.GetNames(typeof(CollectingMode)).ToList();
        }
    }

  
}