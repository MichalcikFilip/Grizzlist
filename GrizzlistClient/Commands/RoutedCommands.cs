using System.Windows.Input;

namespace Grizzlist.Client.Commands
{
    static class RoutedCommands
    {
        public static RoutedCommand OpenNotifications = new RoutedCommand();
        public static RoutedCommand OpenStats = new RoutedCommand();
        public static RoutedCommand OpenSettings = new RoutedCommand();
        public static RoutedCommand OpenChangelog = new RoutedCommand();
        public static RoutedCommand OpenAboutApp = new RoutedCommand();
        public static RoutedCommand CloseApp = new RoutedCommand();

        public static RoutedCommand NewTask = new RoutedCommand();
        public static RoutedCommand ShowTask = new RoutedCommand();
        public static RoutedCommand EditTask = new RoutedCommand();
        public static RoutedCommand OpenTask = new RoutedCommand();
        public static RoutedCommand PostponeTask = new RoutedCommand();
        public static RoutedCommand CloseTask = new RoutedCommand();
        public static RoutedCommand RemoveTask = new RoutedCommand();
        public static RoutedCommand OpenTemplates = new RoutedCommand();
        public static RoutedCommand OpenSearch = new RoutedCommand();

        public static RoutedCommand OpenNotes = new RoutedCommand();
        public static RoutedCommand OpenPasswords = new RoutedCommand();
        public static RoutedCommand OpenLinks = new RoutedCommand();
    }
}
