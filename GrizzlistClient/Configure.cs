using Grizzlist.Client.Properties;
using Grizzlist.Logger;
using System;
using System.Windows;

namespace Grizzlist.Client
{
    static class Configure
    {
        public static void AppStart()
        {
            try
            {
                Log.Configure((LogLevel)Enum.Parse(typeof(LogLevel), Settings.Default.LogLevel), Settings.Default.LogDirectory, Settings.Default.LogFile);
            }
            catch
            {
                MessageBox.Show("Grizzlist application cannot start. There is a problem with configuration.", "Grizzlist startup error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            Log.Info("Grizzlist started", null);
        }
    }
}
