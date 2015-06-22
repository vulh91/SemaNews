using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemaNews.CollectorEditor.Commands
{
    public static class CollectorCommands
    {
        public static RoutedUICommand Start = new RoutedUICommand
        (
            "Start",
            "Start",
            typeof(CollectorCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F5, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Stop = new RoutedUICommand
        (
            "Stop",
            "Stop",
            typeof(CollectorCommands),
            new InputGestureCollection()
            {
            }
        );

        public static readonly RoutedUICommand Reset = new RoutedUICommand
        (
            "Reset",
            "Reset",
            typeof(CollectorCommands),
            new InputGestureCollection()
            {
            }
        );

        public static readonly RoutedUICommand CollectNews = new RoutedUICommand
        (
            "Collect News",
            "Collect News",
            typeof(CollectorCommands),
            new InputGestureCollection()
            {
            }
        );

        public static readonly RoutedUICommand StopCollectNews = new RoutedUICommand
       (
           "Stop Collect News",
           "Stop Collect News",
           typeof(CollectorCommands),
           new InputGestureCollection()
           {
           }
       );

        public static readonly RoutedUICommand Config = new RoutedUICommand
        (
            "Config",
            "Config",
            typeof(CollectorCommands),
            new InputGestureCollection()
            {
            }
        );
    }
}
