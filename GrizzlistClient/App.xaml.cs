using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.UserSettings.BackgroundActions;
using System.Windows;

namespace Grizzlist.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Configure.AppStart();
            ActionsCollection.Instance.Add(new LastRunAction());
        }
    }
}
