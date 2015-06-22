using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemaNews.CollectorEditor.Commands
{
    public static class WindowCommands
    {
        public static RoutedCommand MinimizeWindowToTray = new RoutedCommand("MinimizeWindowToTray", typeof(WindowCommands));

        public static RoutedCommand RestoreWindow = new RoutedCommand("RestoreWindow", typeof(WindowCommands));

        public static RoutedCommand ExitWindow = new RoutedCommand("ExitWindow", typeof(WindowCommands));

        public static RoutedCommand AboutWindow = new RoutedCommand("AboutWindow", typeof(WindowCommands));
    }
}
