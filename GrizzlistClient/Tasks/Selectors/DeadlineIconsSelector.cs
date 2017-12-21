using Grizzlist.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks.Selectors
{
    public class DeadlineIconsSelector : IGroupIconsSelector
    {
        private TextBlock tbWarning = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(4, 3, 4, 3), FontWeight = FontWeights.Bold };
        private TextBlock tbCritical = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(4, 3, 4, 3), FontWeight = FontWeights.Bold };
        private Image imgWarning = new Image() { Margin = new Thickness(2), Source = new BitmapImage(new Uri("/Resources/warning_24_yellow.png", UriKind.Relative)) };
        private Image imgCritical = new Image() { Margin = new Thickness(2), Source = new BitmapImage(new Uri("/Resources/warning_24_red.png", UriKind.Relative)) };

        public void Select(TasksGroupControl control, StackPanel panel)
        {
            DateTime today = DateTime.Today;
            int warning = 0, critical = 0;

            foreach (Task task in control.Tasks)
            {
                if (task.Deadline < today)
                    critical++;
                else if (task.Deadline == today)
                    warning++;
            }

            panel.Children.Remove(tbCritical);
            panel.Children.Remove(imgCritical);
            panel.Children.Remove(tbWarning);
            panel.Children.Remove(imgWarning);

            if (critical > 0)
            {
                tbCritical.Text = critical.ToString();

                panel.Children.Add(tbCritical);
                panel.Children.Add(imgCritical);
            }

            if (warning > 0)
            {
                tbWarning.Text = warning.ToString();

                panel.Children.Add(tbWarning);
                panel.Children.Add(imgWarning);
            }
        }
    }
}
