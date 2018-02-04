using Grizzlist.Tasks;
using Grizzlist.Tasks.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks.Search
{
    /// <summary>
    /// Interaction logic for SearchItemControl.xaml
    /// </summary>
    public partial class SearchItemControl : UserControl
    {
        public Window Owner { get; private set; }
        public Task Task { get; private set; }

        public event Action<Task> OnSelect;

        public SearchItemControl(Window owner, Task task)
        {
            Owner = owner;
            Task = task;

            InitializeComponent();

            mainGrid.Background = new SolidColorBrush(Colors.White);
            pnlPriority.Fill = new SolidColorBrush(TaskPriorityColors.GetColor(Task.Priority));
            tbName.Text = Task.Name;
            tbDescription.Text = Task.Description;

            if (Task.SubTasks.Count == 0)
                rowSubtasks.Height = new GridLength(0);
            else
                rowSubtasks.Height = new GridLength(1, GridUnitType.Star);

            gridSubtasks.RowDefinitions.Clear();
            gridSubtasks.Children.Clear();

            foreach (SubTask subtask in Task.SubTasks)
            {
                gridSubtasks.RowDefinitions.Add(new RowDefinition());

                Image imgCompleted = new Image() { Source = new BitmapImage(new Uri("/Resources/completed_24_green.png", UriKind.Relative)), Margin = new Thickness(2, 2, 2, string.IsNullOrEmpty(subtask.Description) ? 10 : 2), Opacity = subtask.Completed ? 1 : 0.3 };
                TextBlock tbSubtaskName = new TextBlock() { Margin = new Thickness(4, 2, 0, string.IsNullOrEmpty(subtask.Description) ? 10 : 2), FontWeight = FontWeights.Bold, Text = subtask.Name, Opacity = subtask.Completed ? 0.3 : 1 };

                gridSubtasks.Children.Add(imgCompleted);
                gridSubtasks.Children.Add(tbSubtaskName);

                Grid.SetColumn(imgCompleted, 0);
                Grid.SetColumn(tbSubtaskName, 1);
                Grid.SetRow(imgCompleted, gridSubtasks.RowDefinitions.Count - 1);
                Grid.SetRow(tbSubtaskName, gridSubtasks.RowDefinitions.Count - 1);

                if (!string.IsNullOrEmpty(subtask.Description))
                {
                    gridSubtasks.RowDefinitions.Add(new RowDefinition());

                    TextBlock tbSubtaskDescription = new TextBlock() { Margin = new Thickness(4, 0, 0, 10), TextWrapping = TextWrapping.Wrap, Text = subtask.Description, Opacity = subtask.Completed ? 0.2 : 1 };

                    gridSubtasks.Children.Add(tbSubtaskDescription);

                    Grid.SetColumn(tbSubtaskDescription, 1);
                    Grid.SetRow(tbSubtaskDescription, gridSubtasks.RowDefinitions.Count - 1);
                    Grid.SetColumnSpan(tbSubtaskDescription, 2);
                }
            }

            if (Task.Tags.Count == 0)
                rowTags.Height = new GridLength(0);
            else
                rowTags.Height = new GridLength(1, GridUnitType.Star);

            pnlTags.Children.Clear();
            foreach (Tag tag in Task.Tags)
                pnlTags.Children.Add(new Border() { Margin = new Thickness(6), Padding = new Thickness(4, 2, 4, 2), CornerRadius = new CornerRadius(4), Background = new SolidColorBrush(Color.FromRgb(255, 244, 204)), BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Color.FromRgb(255, 216, 0)), Child = new TextBlock() { Text = tag.Value } });
        }

        public void Deselect()
        {
            mainGrid.Background = new SolidColorBrush(Colors.White);
        }

        private void Select(object sender, MouseButtonEventArgs e)
        {
            OnSelect?.Invoke(Task);

            mainGrid.Background = new SolidColorBrush(Color.FromRgb(216, 237, 255));
            e.Handled = true;

            if (e.ClickCount == 2)
                new ViewTaskWindow(Owner, Task).ShowDialog();
        }
    }
}
