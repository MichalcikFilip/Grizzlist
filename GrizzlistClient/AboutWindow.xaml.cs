using System.Reflection;
using System.Windows;

namespace Grizzlist.Client
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public string Version { get { return "test"; } }

        public AboutWindow(Window owner)
        {
            Owner = owner;

            InitializeComponent();

            tbVersion.Text += Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void Changelog_Click(object sender, RoutedEventArgs e)
        {
            new ChangelogWindow(this).ShowDialog();
        }
    }
}
