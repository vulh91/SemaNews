using Microsoft.Win32;
using SemaNews.Collector;
using SemaNewsCore.Configurations;
using SemaNewsCore.Models;
using SemaNewsSearchEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemaNews.CollectorEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CollectorEngine collector;
        private ObservableCollection<string> _messages;

        public CollectorEngine Collector
        {
            get
            {
                if (collector == null)
                    collector = new CollectorEngine();
                return collector;
            }
            set
            {
                collector = value;
                RaisePropertyChanged(() => Collector);
            }
        }
        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        //Commands
        public RoutedCommand MinimizeToTray = new RoutedCommand("MinimizeToTray", typeof(MainWindow));

        public MainWindow()
        {
            this.DataContext = this;
            InitializeData();
            InitializeComponent();
        }

        private void InitializeData()
        {
            try
            {
                _messages = new ObservableCollection<string>();
                Collector = new CollectorEngine();

                Collector.EventNewMessageAdded += HandleNewMessage;
                Collector.EventNewArticleCollected += HandleNewArticle;
                Collector.NewURLVisited += Collector_NewURLVisited;

                Collector.Start();

                //_messages.Add("Hello, this message will be cleared when you click button start");
                ArticleIndexer.LuceneDir = Properties.Settings.Default.LuceneIndexDir;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Initialization failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Collector_NewURLVisited(CollectorUnitBase unit, string url, string text)
        {
            try
            {
                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    var link = db.VisitedLinks.FirstOrDefault(m => m.URL == url);
                    if (link == null)
                    {
                        link = new VisitedLink()
                        {
                            URL = url,
                            Name = text,
                            VisitCount = 1,
                            Time = DateTime.Now,
                        };
                        db.VisitedLinks.Add(link);
                    }
                    else
                    {
                        link.Name = text;
                        link.Time = DateTime.Now;
                        link.VisitCount++;
                        db.Entry(link).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                NotifyMessage(string.Format("UPDATE CRAWLING HISTORY ERROR: \"{0}\"", e.Message));
            }
        }

        private void HandleNewArticle(object sender, object data)
        {
            try
            {
                var article = data as Article;
                using (SemaNewsDBContext db = new SemaNewsDBContext())
                {
                    db.Articles.Add(article);
                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                NotifyMessage(string.Format("FAILED to save article:\"{0}\"", e.Message));
            }
        }

        private void HandleNewMessage(object sender, object data)
        {
            NotifyMessage(data as string);
        }

        protected virtual void RaisePropertyChanged<TResult>(Expression<Func<TResult>> selector)
        {
            if (PropertyChanged == null) return;
            var memberExpression = selector.Body as MemberExpression;
            if (memberExpression != null)
                PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ClearMessages();
            Collector.CollectNewsAsync();
        }

        private void NotifyMessage(string message)
        {
            try
            {
                App.Current.Dispatcher.BeginInvoke(new Action(() =>  // <--- HERE
                {
                    message = string.Format("[{0}] - {1}", DateTime.Now.ToShortTimeString(), message);
                    if (_messages.Count >= 2000)
                        _messages.Clear();
                    _messages.Insert(0, message);
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ClearMessages()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() => // <--- HERE
            {
                _messages.Clear();
            }));
        }

        //Commands Implementation
        private void StartCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CollectorConfigManager.CollectorInfo.Status == SemaNewsCore.Configurations.CollectorStatus.Started)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        private void StartCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            collector.Start();
        }
        private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CollectorConfigManager.CollectorInfo.Status == SemaNewsCore.Configurations.CollectorStatus.Stopped)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            collector.Stop();
        }
        private void ResetCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ResetCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            collector.Reset();
        }
        private void CollectNewsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true)
                e.CanExecute = false;
            else
                e.CanExecute = true;
        }
        private void CollectNewsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearMessages();
            Collector.CollectNewsAsync();
        }
        private void StopCollectNewsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CollectorConfigManager.CollectorInfo.IsCollectorBusy == true)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void StopCollectNewsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            collector.StopCollectNews();
        }
        private void ConfigCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ConfigCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }
        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            collector.StopCollectNews();
            collector.Dispose();
        }

        private void ExitWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private void ExitWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void MinimizeWindowToTrayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void MinimizeWindowToTrayCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RestoreWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
        }
        private void RestoreWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AboutWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Version 1.0", "NEWS COLLECTOR - SEMANEWS", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AboutWindowCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case System.Windows.WindowState.Minimized:
                    this.ShowInTaskbar = false;
                    break;
                case System.Windows.WindowState.Maximized:
                    this.ShowInTaskbar = true;
                    break;
                case System.Windows.WindowState.Normal:
                    this.ShowInTaskbar = true;
                    break;
            }
        }
    }
}
