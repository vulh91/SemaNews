using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lucene.Net;
using Lucene.Net.Store;
using Lucene.Net.Index;
using SemaNewsCore.Models;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using SemaNews.Utilities;
using Lucene.Net.Analysis.Standard;
using SemaNewsSearchEngine.Config;
using SemaNewsCore;

namespace SemaNewsSearchEngine
{
    public class ArticleIndexer
    {
        public static string LuceneDir;
        private static Directory _directoryTemp;

        public static Directory Directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new System.IO.DirectoryInfo(LuceneDir));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = System.IO.Path.Combine(LuceneDir, "write.lock");

                if (System.IO.File.Exists(lockFilePath))
                    System.IO.File.Delete(lockFilePath);

                return _directoryTemp;
            }
        }

        private static void AddToLuceneIndex(Article article, IndexWriter writer)
        {
            //remove older index entry
            var searchQuery = new TermQuery(new Term(IndexField.Id, article.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            //add new index entry
            Document doc = new Document();

            //add lucene fields mapped to db fields
            var id = new Lucene.Net.Documents.Field(IndexField.Id, article.Id.ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED);
            var title = new Lucene.Net.Documents.Field(IndexField.Title, HtmlHandler.StripHtmlTags(article.Title), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);
            var intro = new Lucene.Net.Documents.Field(IndexField.Abstract, HtmlHandler.StripHtmlTags(article.Abstract), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);
            var tags = new Lucene.Net.Documents.Field(IndexField.Tags, HtmlHandler.StripHtmlTags(article.Tags), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);
            var text = new Lucene.Net.Documents.Field(IndexField.Text, HtmlHandler.StripHtmlTags(article.Content), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);

            doc.Add(id);
            doc.Add(title);
            doc.Add(intro);
            doc.Add(tags);
            doc.Add(text);

            //add entry to index
            writer.AddDocument(doc);
        }

        public static void AddUpdateLuceneIndex(IEnumerable<Article> articles)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var item in articles)
                    AddToLuceneIndex(item, writer);
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static void AddUpdateLuceneIndex(Article article)
        {
            AddUpdateLuceneIndex(new List<Article> { article });
        }

        public static void ClearLuceneIndexRecord(int recordId)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);
                analyzer.Close();
                writer.Dispose();
            }
        }

        public static bool ClearLuceneIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                    analyzer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void Optimize()
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
        }
        
        public static void GenerateGraphs(IEnumerable<Article> articles, string domain)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                foreach (var article in articles)
                {
                    var articleKG = db.ArticleKGs.Find(article.Id);
                    if (articleKG == null)
                    {
                        articleKG = new ArticleKG() { Id = article.Id };
                        db.ArticleKGs.Add(articleKG);
                    }

                    if (domain == SemanticDomains.LDVL)
                    {
                        var graph_LDVL = CreateKeyphraseGraphLibrary.KeyphraseGraph.GetKeyphraseGraph(
                            article.Id,
                            string.IsNullOrEmpty(article.Title) ? "" : article.Title,
                            string.IsNullOrEmpty(article.Abstract) ? "" : article.Abstract,
                            HtmlHandler.StripHtmlTags(string.IsNullOrEmpty(article.Content) ? "" : article.Content),
                            string.IsNullOrEmpty(article.Tags) ? "" : article.Tags);

                        articleKG.LDVL_Graph = graph_LDVL;
                    }
                    else if (domain == SemanticDomains.DTC_DTNN)
                    {
                        var graph_DT = CreateKeyphraseGraphLibrary.KeyphraseGraph_DTCDTNN.GetKeyphraseGraph(
                            article.Id,
                            string.IsNullOrEmpty(article.Title) ? "" : article.Title,
                            string.IsNullOrEmpty(article.Abstract) ? "" : article.Abstract,
                            HtmlHandler.StripHtmlTags(string.IsNullOrEmpty(article.Content) ? "" : article.Content),
                            string.IsNullOrEmpty(article.Tags) ? "" : article.Tags);

                        articleKG.DT_Graph = graph_DT;
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
