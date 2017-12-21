using Grizzlist.Client.Persistent;
using Grizzlist.Persistent;
using Grizzlist.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grizzlist.Client.Stats
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public StatsWindow(Window owner)
        {
            Owner = owner;

            InitializeComponent();
            cbPeriod.SelectionChanged += RefreshStats;

            DateTime now = DateTime.Today;
            cbPeriod.Items.Add(new Period("Total", DateTime.MinValue));
            cbPeriod.Items.Add(new Period("Last year", now.AddYears(-1)));
            cbPeriod.Items.Add(new Period("Last 30 days", now.AddDays(-30)));
            cbPeriod.Items.Add(new Period("Last 7 days", now.AddDays(-7)));
            cbPeriod.SelectedIndex = 0;
        }

        private void RefreshStats(object sender, SelectionChangedEventArgs e)
        {
            using (IRepository<StatsManager, long> repository = PersistentFactory.GetContext().GetRepository<StatsManager, long>())
            {
                StatsManager manager = repository.GetAll().FirstOrDefault();

                if (manager != null)
                {
                    StatsData result = new StatsData();

                    foreach (StatsData data in manager.GetData(((Period)cbPeriod.SelectedItem).Start, DateTime.Today))
                        result += data;

                    tbCreated.Text = result.TasksCreated.ToString();
                    tbFromTemplate.Text = $"{result.TasksCreatedFromTemplate} ({(result.TasksCreated > 0 ? Math.Round((double)result.TasksCreatedFromTemplate / result.TasksCreated, 2) * 100 : 0)}%)";
                    tbClosed.Text = $"{result.TasksClosed} ({(result.TasksCreated > 0 ? Math.Round((double)result.TasksClosed / result.TasksCreated, 2) * 100 : 0)}%)";
                    tbArchived.Text = $"{result.TasksArchived} ({(result.TasksCreated > 0 ? Math.Round((double)result.TasksArchived / result.TasksCreated, 2) * 100 : 0)}%)";
                    tbRemoved.Text = $"{result.TasksRemoved} ({(result.TasksCreated > 0 ? Math.Round((double)result.TasksRemoved / result.TasksCreated, 2) * 100 : 0)}%)";
                    tbVeryHigh.Text = $"{result.TasksVeryHighPriority} ({(result.TasksClosed > 0 ? Math.Round((double)result.TasksVeryHighPriority / result.TasksClosed, 2) * 100 : 0)}%)";
                    tbHigh.Text = $"{result.TasksHighPriority} ({(result.TasksClosed > 0 ? Math.Round((double)result.TasksHighPriority / result.TasksClosed, 2) * 100 : 0)}%)";
                    tbNormal.Text = $"{result.TasksNormalPriority} ({(result.TasksClosed > 0 ? Math.Round((double)result.TasksNormalPriority / result.TasksClosed, 2) * 100 : 0)}%)";
                    tbLow.Text = $"{result.TasksLowPriority} ({(result.TasksClosed > 0 ? Math.Round((double)result.TasksLowPriority / result.TasksClosed, 2) * 100 : 0)}%)";
                    tbVeryLow.Text = $"{result.TasksVeryLowPriority} ({(result.TasksClosed > 0 ? Math.Round((double)result.TasksVeryLowPriority / result.TasksClosed, 2) * 100 : 0)}%)";
                }
            }
        }

        private struct Period
        {
            public string Name { get; private set; }
            public DateTime Start { get; private set; }

            public Period(string name, DateTime start)
            {
                Name = name;
                Start = start;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
