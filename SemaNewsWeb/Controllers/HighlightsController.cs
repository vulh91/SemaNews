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
    public class HighlightsController : Controller
    {
        SemaNewsDBContext entities = new SemaNewsDBContext();

        public ActionResult Index(DateTime? fromTime, DateTime? toTime, string newspaper, string category, string topic,
            DisplayMode display = DisplayMode.Default, bool isLocalOnly = false, bool isRelevantToLocalOnly = false, int page = 0)
        {
            int DEFAULT_PAGESIZE = 10;
            HighlightVM results = new HighlightVM();

            #region Analyzing QueryString
            var newspapers = newspaper != null ? newspaper.Split(';') : new string[] { };
            var categories = category != null ? category.Split(';') : new string[] { };
            var topics = topic != null ? topic.Split(';') : new string[] { };

            if (newspapers != null && newspapers.Length != 0)
            {
                foreach (var item in newspapers)
                {
                    try
                    {
                        var targetNewspaper = Newspaper.Search(entities, item);
                        if (targetNewspaper != null)
                            results.Newspapers.Add(targetNewspaper);
                    }
                    catch
                    {
                        Console.WriteLine("Newspaper {0} is not found", item);
                    }
                }
            }

            if (categories != null && categories.Length!=0)
            {
                foreach (var item in categories)
                {
                    try
                    {
                        var targetCategory = GField.Search(entities, item);
                        if (targetCategory != null)
                            results.Categories.Add(targetCategory);
                    }
                    catch
                    {
                        Console.WriteLine("Category {0} is not found", item);
                    }
                }
            }

            if (topics != null && topics.Length!=0)
            {
                foreach (var item in topics)
                {
                    try
                    {
                        var targetTopic = Topic.Search(entities, item);
                        if (targetTopic != null)
                            results.Topics.Add(targetTopic);
                    }
                    catch
                    {
                        Console.WriteLine("Topic {0} is not found", item);
                    }
                }
            }

            results.Display = display;
            results.IsLocalSourceOnly = isLocalOnly;
            results.IsRelevantToLocalOnly = isRelevantToLocalOnly;
            #endregion

            #region Retrieve approriate articles matching filter conditions
            //var articleList = entities.Articles.ToList();
            TextSearchEngine searchEngine = new TextSearchEngine();

            fromTime = fromTime.HasValue ? fromTime.Value.Date : DateTime.Today;
            toTime = toTime.HasValue ? toTime.Value.Date.AddDays(1).AddSeconds(-1) : DateTime.Today.AddDays(1).AddSeconds(-1);
            results.FromTime = fromTime.Value.Date;
            results.ToTime = toTime.Value.Date;

            SearchQuery searchQuery = new SearchQuery()
            {
                 TimePublishedFrom = fromTime.Value,
                 TimePublishedTo = toTime.Value,
                 RelatedToLocalOnly = isRelevantToLocalOnly,
                 Newspapers = results.Newspapers.Select(m=>m.Name).ToList(),
                 Categories = results.Categories.Select(m=>m.Name).ToList(),
                 Topics = results.Topics.Select(m=>m.Name).ToList(),
            };
            var articleList = searchEngine.GetNewsHighLight(searchQuery, fromTime.Value, toTime.Value).ToList();

            #endregion

            #region Organizing articles based on chosen DisplayMode
            switch (display)
            {
                case DisplayMode.Default:
                    var defaultGroup = new ArticleGroup()
                    {
                        Id = 0,
                        Name = "Danh sách các tin bài",
                        CurrentPage = 0,
                        Articles = articleList,
                    };
                    results.ViewGroups.Add(defaultGroup);
                    break;
                case DisplayMode.ByNewspapers:
                    var newspaperGroups = articleList.GroupBy(m => m.Article.NewspaperId);
                    foreach (var group in newspaperGroups)
                    {
                        var groupNewspaper = entities.Newspapers.Find(group.Key.Value);
                        var newGroup = new ArticleGroup()
                        {
                            Id = group.Key.Value,
                            Name = groupNewspaper.Name,
                            Articles = group.ToList()
                        };
                        results.ViewGroups.Add(newGroup);
                    }
                    break;
                case DisplayMode.ByCategories:
                    var categoryGroups = articleList.GroupBy(m => m.Article.GFieldId);
                    foreach (var group in categoryGroups)
                    {
                        var groupGfield = entities.GFields.Find(group.Key.Value);
                        var newGroup = new ArticleGroup()
                        {
                            Id = group.Key.Value,
                            Name = groupGfield.Name,
                            Articles = group.ToList(),
                        };
                        results.ViewGroups.Add(newGroup);
                    }
                    break;
                case DisplayMode.ByTopics:
                     foreach (var item in entities.Topics)
                    {
                        var newGroup = new ArticleGroup();
                        newGroup.Id = item.Id;
                        newGroup.Name = item.Name;
                        newGroup.PageSize = DEFAULT_PAGESIZE;
                        newGroup.CurrentPage = 1;
                        newGroup.Articles = articleList.Where(m => m.Topics.Contains(item.Name)).ToList();
                        results.ViewGroups.Add(newGroup);
                    }

                    var unknownGroup = new ArticleGroup();
                    unknownGroup.Id = 0;
                    unknownGroup.Name = "Không xác định";
                    unknownGroup.PageSize = DEFAULT_PAGESIZE;
                    unknownGroup.CurrentPage = 1;
                    unknownGroup.Articles = articleList.Where(m => m.Topics.Count == 0).ToList();
                    results.ViewGroups.Add(unknownGroup);
                    results.ViewGroups.RemoveAll(m => m.Articles.Count == 0);
                    break;
                case DisplayMode.ByGeography:
                    var geographyGroups = articleList.GroupBy(m => m.IsLocalNews);
                    foreach (var group in geographyGroups)
                    {
                        var newGroup = new ArticleGroup();

                        if (group.Key == true)
                        {
                            newGroup.Id = 1;
                            newGroup.Name = "Báo trong tỉnh";
                        }
                        else
                        {
                            newGroup.Id = 0;
                            newGroup.Name = "Báo ngoài tỉnh";
                        }
                        newGroup.Articles = group.ToList();
                        results.ViewGroups.Add(newGroup);
                    }
                    break;
            }
            #endregion

            ViewData["AllNewspapers"] = entities.Newspapers.ToArray();
            ViewData["AllCategories"] = entities.GFields.ToArray();
            ViewData["AllTopics"] = entities.Topics.ToArray();
            ViewData["IsRelevantToLocalOnly"] = isRelevantToLocalOnly;
            if (results.FromTime == results.ToTime)
                ViewBag.Title = "ĐIỂM TIN NGÀY " + results.FromTime.ToString("dd/MM/yyyy");
            else
                ViewBag.Title = "ĐIỂM TIN TỪ NGÀY " + results.FromTime.ToString("dd/MM/yyyy") + " ĐẾN NGÀY " + results.ToTime.ToString("dd/MM/yyyy");

            return View(results);
        }

        [HttpPost]
        public ActionResult Index(HighlightVM highlightVM)
        {
            if (highlightVM != null)
            {
                return Index(
                    highlightVM.FromTime,
                    highlightVM.ToTime,
                    string.Join(";",highlightVM.Newspapers.Select(m => m.Name)), 
                    string.Join(";",highlightVM.Categories.Select(m => m.Name)), 
                    string.Join(";",highlightVM.Topics.Select(m => m.Name).ToArray()), 
                    highlightVM.Display, 
                    highlightVM.IsLocalSourceOnly, 
                    highlightVM.IsRelevantToLocalOnly, 0);
            }
            MsgNotification messages = new MsgNotification("Lỗi đã xảy ra trong quá trình thực hiện yêu cầu", MsgType.Error);
            ViewBag.Messages = new MsgNotification[] { messages };
            return Index(DateTime.Today, DateTime.Today, null, null, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                entities.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}