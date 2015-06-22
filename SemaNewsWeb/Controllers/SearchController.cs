using SemaNewsCore.Models;
using SemaNewsSearchEngine;
using SemaNewsSearchEngine.Models;
using SemaNewsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SemaNewsWeb.Controllers
{
    public class SearchController : Controller
    {
        SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index(string query, bool isSemantic = false, DisplayMode display = DisplayMode.Default)
        {
            int DEFAULT_PAGESIZE = 10;
            List<SearchMatch> searchMatches = new List<SearchMatch>();
            SearchResultVM searchResults = new SearchResultVM();
            var queryModel = TempData["SearchQuery"] as SearchQuery;
            if (queryModel == null)
                queryModel = new SearchQuery() 
                { 
                    SearchMode = isSemantic?SearchMode.SemanticSearch:SearchMode.TextSearch, 
                    SearchString = query 
                };

            if (isSemantic)
                searchMatches = SemanticSearch(queryModel);
            else
                searchMatches = TextSearch(queryModel);

            //Adding allowed newspapers, categories, topics for future filter
            searchResults.ArticlesCount = searchMatches.Count;
            searchResults.Display = display;
            searchResults.Query = queryModel;
            searchResults.AllowNewspapers = queryModel.Newspapers;
            searchResults.AllowCategories = queryModel.Categories;
            searchResults.AllowTopics = queryModel.Topics;

            if (searchResults.AllowNewspapers == null || searchResults.AllowNewspapers.Count == 0)
                searchResults.AllowNewspapers = entities.Newspapers.Select(m => m.Name).ToList();
            if (searchResults.AllowCategories == null || searchResults.AllowCategories.Count == 0)
                searchResults.AllowCategories = entities.GFields.Select(m => m.Name).ToList();
            if (searchResults.AllowTopics == null || searchResults.AllowTopics.Count == 0)
                searchResults.AllowTopics = entities.Topics.Select(m => m.Name).ToList();

            switch (display)
            {
                case DisplayMode.Default:
                    var defaultGroup = new SearchResultGroupVM();
                    defaultGroup.Id = 0;
                    defaultGroup.Name = "CÁC TIN BÀI TÌM ĐƯỢC";
                    defaultGroup.PageSize = DEFAULT_PAGESIZE;
                    defaultGroup.CurrentPage = 1;
                    defaultGroup.Query = queryModel;
                    defaultGroup.Articles = searchMatches;
                    defaultGroup.ArticlesCount = searchMatches.Count;

                    searchResults.RestultGroups.Add(defaultGroup);
                    break;
                case DisplayMode.ByNewspapers:
                    var newspaperGroups = searchMatches.GroupBy(a => a.Article.NewspaperId);
                    foreach (var group in newspaperGroups)
                    {

                        if (group.Key.HasValue)
                        {
                            var newGroup = new SearchResultGroupVM();
                            newGroup.Id = group.Key.Value;
                            newGroup.Name = entities.Newspapers.Find(group.Key.Value).Name;
                            newGroup.PageSize = DEFAULT_PAGESIZE;
                            newGroup.CurrentPage = 1;
                            newGroup.Articles = group.ToList();
                            newGroup.Query = queryModel;
                            newGroup.ArticlesCount = searchMatches.Count;

                            searchResults.RestultGroups.Add(newGroup);
                        }
                    }

                    break;
                case DisplayMode.ByCategories:
                    var categoryGroups = searchMatches.GroupBy(a => a.Article.GFieldId);
                    foreach (var group in categoryGroups)
                    {
                        var newGroup = new SearchResultGroupVM();
                        newGroup.Id = group.Key.Value;
                        newGroup.Name = entities.GFields.Find(group.Key.Value).Name;
                        newGroup.PageSize = DEFAULT_PAGESIZE;
                        newGroup.CurrentPage = 1;
                        newGroup.Articles = group.ToList();
                        newGroup.Query = queryModel;

                        searchResults.RestultGroups.Add(newGroup);
                    }
                    break;
                case DisplayMode.ByTopics:
                    foreach (var topic in entities.Topics)
                    {
                        var newGroup = new SearchResultGroupVM();
                        newGroup.Id = topic.Id;
                        newGroup.Name = topic.Name;
                        newGroup.PageSize = DEFAULT_PAGESIZE;
                        newGroup.CurrentPage = 1;
                        newGroup.Articles = searchMatches.Where(m => m.Topics.Contains(topic.Name)).ToList();
                        searchResults.RestultGroups.Add(newGroup);
                    }

                    var unknownGroup = new SearchResultGroupVM();
                    unknownGroup.Id = 0;
                    unknownGroup.Name = "Không xác định";
                    unknownGroup.PageSize = DEFAULT_PAGESIZE;
                    unknownGroup.CurrentPage = 1;
                    unknownGroup.Articles = searchMatches.Where(m => m.Topics.Count == 0).ToList();
                    searchResults.RestultGroups.Add(unknownGroup);
                    searchResults.RestultGroups.RemoveAll(m => m.Articles.Count == 0);
                    break;
                case DisplayMode.ByGeography:
                    var groups = searchMatches.GroupBy(a => a.Article.Newspaper.IsLocal.Value);
                    foreach (var group in groups)
                {
                        var newGroup = new SearchResultGroupVM();
                        if (group.Key == true)
                        {
                            newGroup.Id = 1;
                            newGroup.Name = "Báo trong tỉnh";
                        }
                        else if (group.Key == false)
                        {
                            newGroup.Id = 0;
                            newGroup.Name = "Báo ngoài tỉnh";
                        }

                        newGroup.Query = queryModel;
                        newGroup.PageSize = DEFAULT_PAGESIZE;
                        newGroup.CurrentPage = 1;
                        newGroup.Articles = group.ToList();
                        searchResults.RestultGroups.Add(newGroup);
                    }
                    break;
            }

            ViewBag.IsSemantic = isSemantic;
            ViewBag.Query = query;
            ViewBag.ArticleIds = string.Join(",", searchMatches.Select(m => m.Article.Id));
            return View(searchResults);
        }

        [HttpPost]
        public ActionResult Index(SearchResultVM searchResult)
        {
            List<SearchMatch> articles = new List<SearchMatch>();
            //Check null
            if (searchResult.Query.SearchMode == SearchMode.SemanticSearch)
                articles = SemanticSearch(searchResult.Query);
            else
                articles = TextSearch(searchResult.Query);

            switch (searchResult.Display)
            {
                case DisplayMode.Default:
                    var defaultGroup = new SearchResultGroupVM();
                    break;
                case DisplayMode.ByNewspapers:
                    break;
                case DisplayMode.ByCategories:
                    break;
                case DisplayMode.ByTopics:
                    break;
                case DisplayMode.ByGeography:
                    break;
            }

            return View(searchResult);
        }

        public ActionResult TextSearch(string query)
        {
            return View(new TextSearchVM());
        }

        public ActionResult SemanticSearch()
        {
            return View(new SemanticSearchVM());
        }

        [HttpPost]
        public ActionResult SearchSubmit(FormCollection form)
        {
            var queryString = form["query"];
            SearchQueryVM query = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchQueryVM>(queryString);
            char[] separator = new char[] { ';', ',', '|' };
            SearchQuery searchQuery = new SearchQuery();
            searchQuery.SearchMode = SearchMode.TextSearch;

            if (string.IsNullOrEmpty(query.SearchString) == false)
            {
                var temp = SearchQuery.ParseKeyWords(query.SearchString);
                searchQuery.AllWord = temp.AllWord;
                searchQuery.ExactWords = temp.ExactWords;
                searchQuery.NoneOfWords = temp.NoneOfWords;
            }

            if (!string.IsNullOrEmpty(query.AllWord))
                searchQuery.AllWord = query.AllWord;

            if (query.ExactPhrases != null && query.ExactPhrases.Count() != 0)
                searchQuery.ExactWords = query.ExactPhrases.Split(separator).ToList();

            if (query.NoneOfWords != null && query.NoneOfWords.Count() != 0)
                searchQuery.NoneOfWords = query.NoneOfWords.Split(separator).ToList();

            if (query.SelectedSearchFields != null)
                searchQuery.SearchFields = query.SelectedSearchFields.Split(separator).ToList();

            if (query.SelectedCategories != null)
                searchQuery.Categories = query.SelectedCategories.Split(separator).ToList();

            if (query.SelectedNewspapers != null)
                searchQuery.Newspapers = query.SelectedNewspapers.Split(separator).ToList();

            if (query.SelectedTopics != null)
                searchQuery.Topics = query.SelectedTopics.Split(separator).ToList();

            if (!string.IsNullOrEmpty(query.PostedTimeFrom))
                searchQuery.TimePublishedFrom = DateTime.Parse(query.PostedTimeFrom);

            if (!string.IsNullOrEmpty(query.PostedTimeTo))
                searchQuery.TimePublishedTo = DateTime.Parse(query.PostedTimeTo);

            if (!string.IsNullOrEmpty(query.CollectedTimeFrom))
                searchQuery.TimeCollectedFrom = DateTime.Parse(query.CollectedTimeFrom);

            if (!string.IsNullOrEmpty(query.CollectedTimeTo))
                searchQuery.TimeCollectedTo = DateTime.Parse(query.CollectedTimeTo);

            searchQuery.RelatedToLocalOnly = query.IsRelevantToLocalOnly;

            TempData["SearchQuery"] = searchQuery;
            return RedirectToAction("Index", new { @query = searchQuery.ToSimpleQueryString(), @isSemantic = query.IsSemantic });
        }

        private List<SearchMatch> SemanticSearch(SearchQuery query)
        {
            SemanticSearchEngine searchEngine = new SemanticSearchEngine();
            return searchEngine.Search(query, SemaNewsWeb.Properties.Settings.Default.SemanticThreshold).ToList();
        }

        private List<SearchMatch> TextSearch(SearchQuery query)
        {
            TextSearchEngine searchEngine = new TextSearchEngine();
            return searchEngine.Search(query, SemaNewsWeb.Properties.Settings.Default.TextThreshold).ToList();
        }

        [HttpPost]
        public ActionResult SaveSearchResult(SaveSearchResultVM saveSearchResultVM)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (ModelState.IsValid)
                {
                    var user = entities.Users.FirstOrDefault(m => m.Name.ToLower() == User.Identity.Name.ToLower());
                    if (user != null)
                    {
                        UserQuery userQuery = new UserQuery()
                        {
                            Name = saveSearchResultVM.Name,
                            Description = saveSearchResultVM.Description,
                            IsSaved = true,
                            SavedTime = DateTime.Now,
                            UserId = user.Id,
                            SearchQuery = saveSearchResultVM.QueryContent,
                        };
                        ICollection<int> ignoreArticleIds = new List<int>();
                        if (saveSearchResultVM.IgnoreArticleIds != null)
                            ignoreArticleIds = saveSearchResultVM.IgnoreArticleIds.Split(',').Select(m => int.Parse(m)).ToList();
                        var articleIds = saveSearchResultVM.ArticleIds.Split(',').Select(m => int.Parse(m)).Where(m => ignoreArticleIds.Contains(m) == false);

                        List<SavedArticle> savedArticles = new List<SavedArticle>();
                        foreach (var item in Article.GetAllArticles(entities, articleIds.ToArray()))
                        {
                            savedArticles.Add(new SavedArticle()
                            {
                                ArticleId = item.Id,
                                Title = item.Title,
                                Abstract = item.Abstract,
                                Content = item.Content,
                                Tags = item.Tags,
                                SavedTime = DateTime.Now,
                                Url = item.Url,
                                ReleasedDate = item.ReleasedDate,
                                CollectedDate = item.CollectedDate,
                                FieldId = item.FieldId,
                                GFieldId = item.GFieldId,
                                NewspaperId = item.NewspaperId,
                                UserQuery = userQuery
                            });
                        }

                        entities.UserQueries.Add(userQuery);
                        entities.SavedArticles.AddRange(savedArticles);
                        entities.SaveChanges();
                        return Json(new { OK = true, Message = "THÀNH CÔNG! Kết quả tìm kiếm này đã được lưu vào thư mục lưu trữ của bạn..." });
                    }
                }
            }
            return Json(new { OK = false, Message = "LỖI! Đã có lỗi xảy ra trong quá trình thực hiện yêu cầu..." });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                entities.Dispose();
            base.Dispose(disposing);
        }
    }
}