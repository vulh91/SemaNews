using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using SemaNewsCore.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class CollectorReportsVM
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToData { get; set; }

        public DotNet.Highcharts.Highcharts ChartArticlesByDays { get; set; }
        public DotNet.Highcharts.Highcharts ChartArticlesByNewspapers { get; set; }
        public DotNet.Highcharts.Highcharts ChartArticlesByCategories { get; set; }

        public CollectorReportsVM()
        {
        }

        public Highcharts InitChartArticlesByDays(DateTime fromDate, DateTime toDate)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                var datesInRange = Enumerable.Range(0, 1 + toDate.Subtract(fromDate).Days)
                    .Select(offset => fromDate.AddDays(offset)).ToList();

                var localCounts = new List<object>();
                var nonLocalCounts = new List<object>();
                foreach (var date in datesInRange)
                {
                    var articlesInDay = db.Articles.Where(a=>a.CollectedDate.HasValue).ToList().Where(a=> DateTime.Compare(a.CollectedDate.Value.Date, date.Date) == 0);
                    var totalCount = articlesInDay.Count();
                    var localCount = articlesInDay.Where(a => a.Newspaper.IsLocal == true).Count();
                    var nonLocalCount = totalCount - localCount;
                    ////
                    //Random rand = new Random();
                    //localCount = rand.Next(100, 300);
                    //nonLocalCount = rand.Next(1000, 1500);
                    ////
                    localCounts.Add(localCount);
                    nonLocalCounts.Add(nonLocalCount);
                }

                List<Series> datas = new List<Series>()
                {
                    new Series(){Name = "Báo trong tỉnh", Data = new Data(localCounts.ToArray())},
                    new Series(){Name = "Báo ngoài tỉnh", Data = new Data(nonLocalCounts.ToArray())},
                };

                ChartArticlesByDays = new Highcharts("ChartArticlesByDays")
                .InitChart(new Chart()
                {
                    Type = ChartTypes.Column,
                })
                .SetTitle(new Title()
                {
                    Text = "Thống kê số lượng tin bài thu thập được theo ngày",
                })
                .SetSubtitle(new Subtitle()
                {
                    Text = string.Format("Từ ngày {0} đến ngày {1}", fromDate.ToString("dd/MMM/yyyy"), toDate.ToString("dd/MMM/yyyy"))
                })
                .SetXAxis(new XAxis()
                {
                    Categories = datesInRange.Select(m => m.ToString("dd/MMM/yyyy")).ToArray()
                })
                .SetYAxis(new YAxis()
                {
                    Min = 0,
                    Title = new YAxisTitle() { Text = "Tổng số lượng tin bài thu thập được" },
                    StackLabels = new YAxisStackLabels()
                    {
                        Enabled = true,
                        Style = "fontWeight: 'bold',color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'",
                    }
                })
                .SetLegend(new Legend()
                {
                    Align = HorizontalAligns.Right,
                    X = Number.GetNumber(-70),
                    VerticalAlign = VerticalAligns.Top,
                    Y = Number.GetNumber(20),
                    Floating = true,
                    BackgroundColor = new BackColorOrGradient(System.Drawing.Color.Gray)
                })
                .SetTooltip(new Tooltip()
                {
                    Formatter = @"function() {
                    return 'Ngày' + '<b>'+ this.x +'</b><br/>'+
                        this.series.name +': '+ this.y +'<br/>'+
                        'Tổng cộng: '+ this.point.stackTotal;
                    }",
                })
                .SetPlotOptions(new PlotOptions()
                {
                    Column = new PlotOptionsColumn()
                    {
                        Stacking = Stackings.Normal,
                        DataLabels = new PlotOptionsColumnDataLabels()
                        {
                            Enabled = true,
                            Color = System.Drawing.Color.LightCyan,
                            //Style = "textShadow: '0 0 3px black, 0 0 3px black'"
                        }
                    }
                })
                .SetSeries(datas.ToArray());

                return ChartArticlesByDays;
            }
        }

        public Highcharts InitChartArticlesByNewspapers(DateTime? fromTime, DateTime? toTime)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                if (toTime.HasValue)
                    toTime = toTime.Value.Date.AddDays(1).AddSeconds(-1);
                var newspapers = db.Newspapers.ToList();
                List<object> articlesCountPerNewspaper = new List<object>();
                foreach (var newspaper in newspapers)
                {
                    if (fromTime.HasValue && toTime.HasValue)
                        articlesCountPerNewspaper.Add(newspaper.Articles.Where(a => a.CollectedDate >= fromTime.Value && a.CollectedDate <= toTime.Value).Count());
                    else if(fromTime.HasValue && !toTime.HasValue)
                        articlesCountPerNewspaper.Add(newspaper.Articles.Where(a => a.CollectedDate >= fromTime.Value).Count());
                    else if(!fromTime.HasValue && toTime.HasValue)
                        articlesCountPerNewspaper.Add(newspaper.Articles.Where(a => a.CollectedDate <= toTime.Value).Count());
                    else
                        articlesCountPerNewspaper.Add(newspaper.Articles.Count());
                }
                //
                //articlesCountPerNewspaper = new List<object>();
                //Random rand =new Random();
                //for (int i = 0; i < newspapers.Count; i++)
                //    articlesCountPerNewspaper.Add(rand.Next(1000, 3000));
                //    //

                    ChartArticlesByNewspapers = new DotNet.Highcharts.Highcharts("bar")
                        .InitChart(new Chart()
                        {
                            Type = DotNet.Highcharts.Enums.ChartTypes.Bar
                        })
                        .SetTitle(new DotNet.Highcharts.Options.Title()
                        {
                            Text = "Thống kê số lượng tin bài theo từng trang báo điện tử",
                        })
                        .SetSubtitle(new DotNet.Highcharts.Options.Subtitle()
                        {
                            Text = (fromTime.HasValue ? string.Format("Từ ngày {0} ", fromTime.Value.ToString("dd-MMM-yyyy")) : "")
                                    + (toTime.HasValue ? string.Format("Đến ngày {0}", toTime.Value.ToString("dd-MMM-yyyy")) : ""),
                        })
                        .SetXAxis(new DotNet.Highcharts.Options.XAxis()
                        {
                            Categories = newspapers.Select(m => m.Name).ToArray(),
                        })
                        .SetYAxis(new DotNet.Highcharts.Options.YAxis()
                        {
                            Min = 0,
                            Title = new DotNet.Highcharts.Options.YAxisTitle()
                            {
                                Text = "Tổng số lượng tin bài đã thu thập được"
                            }
                        })
                        .SetLegend(new DotNet.Highcharts.Options.Legend()
                        {
                            Reversed = true,
                        })
                        .SetTooltip(new DotNet.Highcharts.Options.Tooltip()
                        {
                        })
                        .SetPlotOptions(new DotNet.Highcharts.Options.PlotOptions()
                        {
                            Series = new DotNet.Highcharts.Options.PlotOptionsSeries()
                            {
                                Stacking = DotNet.Highcharts.Enums.Stackings.Normal,
                            }
                        })
                        .SetSeries(new DotNet.Highcharts.Options.Series()
                        {
                            Name = "Số lượng tin bài",

                            Data = new DotNet.Highcharts.Helpers.Data(articlesCountPerNewspaper.ToArray())
                        });
            }

            return ChartArticlesByNewspapers;
        }

        public Highcharts InitChartArticlesByCategories(DateTime? fromTime, DateTime? toTime)
        {
            using (SemaNewsDBContext db = new SemaNewsDBContext())
            {
                if (toTime.HasValue)
                    toTime = toTime.Value.Date.AddDays(1).AddSeconds(-1);
                var topCatgs = db.GetRootGFiels().ToList().OrderBy(m => m.GetArticles(db,true,false).Count()).ToList();
                topCatgs.RemoveAll(m => m.GetArticles(db, true, false).Count == 0);

                List<object[]> datas = new List<object[]>();
                foreach (var catg in topCatgs)
                {
                    int articleCount = 0;
                    if (fromTime.HasValue && toTime.HasValue)
                        articleCount = catg.Articles.Where(a => a.CollectedDate >= fromTime.Value && a.CollectedDate <= toTime.Value).Count();
                    else if (fromTime.HasValue && !toTime.HasValue)
                        articleCount = catg.Articles.Where(a => a.CollectedDate >= fromTime.Value).Count();
                    else if (!fromTime.HasValue && toTime.HasValue)
                        articleCount = catg.Articles.Where(a => a.CollectedDate <= toTime.Value).Count();
                    else
                        articleCount = catg.Articles.Count();

                    datas.Add(new object[] { catg.Name, articleCount });
                }

                ChartArticlesByCategories =  new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = "Phân bố tin bài theo từng lĩnh vực phân loại" })
                .SetSubtitle(new DotNet.Highcharts.Options.Subtitle()
                {
                    Text = (fromTime.HasValue ? string.Format("Từ ngày {0} ", fromTime.Value.ToString("dd-MMM-yyyy")) : "")
                            + (toTime.HasValue ? string.Format("Đến ngày {0}", toTime.Value.ToString("dd-MMM-yyyy")) : ""),
                })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage.toFixed(2) +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage.toFixed(2) +' %'; }"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Category share",
                    Data = new Data(datas.ToArray())
                });
            }
            return ChartArticlesByCategories;
        }
    }
}