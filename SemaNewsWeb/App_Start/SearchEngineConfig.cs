using SemaNewsSearchEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb
{
    public static class SearchEngineConfig
    {
        public static void InitalizeData()
        {
            SemanticSearchEngine.InitializeData();
        }
    }
}