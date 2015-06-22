using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemaNewsSearchEngine.Config
{
    public class IndexField
    {
        public const string Id = "id";
        public const string Text = "text";
        public const string Title = "title";
        public const string Abstract = "abstract";
        public const string Tags = "tags";
        public const string SourceNews = "source";
        public const string Category = "category";
        public const string IsLocalPage = "is_local";
        public const string PublishedTime = "time_published";
        public const string CollectedTime = "time_collected";

        public const string Topics = "topic";

        public const string IsRelevantOnly = "is_relevant_only";
    }
}
