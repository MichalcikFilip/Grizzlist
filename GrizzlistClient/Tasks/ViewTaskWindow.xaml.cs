using Grizzlist.Client.Extensions;
using Grizzlist.Client.Tasks.Attachments;
using Grizzlist.Client.Tasks.Drawings;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Types;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for ViewTaskWindow.xaml
    /// </summary>
    public partial class ViewTaskWindow : Window
    {
        public ViewTaskWindow(Window owner, Task task)
        {
            Owner = owner;

            InitializeComponent();

            if (task != null)
            {
                TimeSpan totalActivity = new TimeSpan();

                if (string.IsNullOrEmpty(task.Note))
                    rowNote.Height = new GridLength(0);

                if (task.State != TaskState.Closed)
                    rowClosed.Height = new GridLength(0);

                if (task.SubTasks.Count == 0)
                    rowSubtasks.Height = new GridLength(0);

                if (task.Tags.Count == 0)
                    rowTags.Height = new GridLength(0);

                Title = task.Name;
                tbName.Text = task.Name;
                tbDescription.Text = task.Description;
                tbNote.Text = task.Note;
                tbPriority.Text = task.Priority.ToString();
                tbDeadline.Text = task.Deadline.ToShortDateString();
                tbState.Text = task.State.ToString();
                tbCreated.Text = task.Created.ToString();
                tbUpdated.Text = task.Updated.ToString();
                tbClosed.Text = task.Closed.ToString();

                task.Activities.ForEach(x => totalActivity += x.Length);
                CreateActivity("Task", task);

                foreach (SubTask subtask in task.SubTasks)
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

                    subtask.Activities.ForEach(x => totalActivity += x.Length);
                    CreateActivity(subtask.Name, subtask);
                }

                foreach (Tag tag in task.Tags)
                    pnlTags.Children.Add(new Border() { Margin = new Thickness(4), Padding = new Thickness(4, 2, 4, 2), CornerRadius = new CornerRadius(4), Background = new SolidColorBrush(Color.FromRgb(255, 244, 204)), BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Color.FromRgb(255, 216, 0)), Child = new TextBlock() { Text = tag.Value } });

                foreach (Attachment attachment in task.Attachments)
                    CreateAttachmentNode(tvAttachments, attachment.Path.Split('\\'), 0, attachment);

                foreach (Grizzlist.Tasks.Types.Drawing drawing in task.Drawings)
                    AddDrawing(drawing);

                tbActivity.Text = FormatActivity(totalActivity);
            }
        }

        private void CreateActivity(string header, BaseTask task)
        {
            gridActivity.RowDefinitions.Add(new RowDefinition());

            TextBlock tbHeader = new TextBlock() { Margin = new Thickness(10, 5, 10, 5), FontWeight = FontWeights.Bold, Text = header };

            gridActivity.Children.Add(tbHeader);

            Grid.SetColumn(tbHeader, 0);
            Grid.SetRow(tbHeader, gridActivity.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(tbHeader, 2);

            gridActivity.RowDefinitions.Add(new RowDefinition());

            StackPanel pnlActivity = new StackPanel();

            gridActivity.Children.Add(pnlActivity);

            Grid.SetColumn(pnlActivity, 0);
            Grid.SetRow(pnlActivity, gridActivity.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(pnlActivity, 2);

            if (task.Activities.Count == 0)
                pnlActivity.Children.Add(new Border() { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Color.FromArgb(80, 102, 186, 255)), Background = new SolidColorBrush(Color.FromArgb(80, 216, 237, 255)), Margin = new Thickness(10, 5, 10, 5), Padding = new Thickness(6), Child = new TextBlock() { Text = "No activity" } });

            foreach (Activity activity in task.Activities)
                pnlActivity.Children.Add(new Border() { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Color.FromArgb(120, 102, 186, 255)), Background = new SolidColorBrush(Color.FromArgb(120, 216, 237, 255)), Margin = new Thickness(10, 5, 10, 5), Padding = new Thickness(6), Child = new TextBlock() { Text = $"{activity.Start} - {(activity.IsActive ? DateTime.Now : activity.End)}{Environment.NewLine}Activity: {FormatActivity(activity.Length)}" } });
        }

        private void CreateAttachmentNode(ItemsControl parentNode, string[] path, int level, Attachment attachment)
        {
            if (path.Length > level)
            {
                if (parentNode.Items.SortDescriptions.Count == 0)
                {
                    parentNode.Items.SortDescriptions.Add(new SortDescription("IsFile", ListSortDirection.Ascending));
                    parentNode.Items.SortDescriptions.Add(new SortDescription("NodeName", ListSortDirection.Ascending));
                }

                string nodeName = path[level];
                AttachmentNode node = parentNode.Items.OfType<AttachmentNode>().FirstOrDefault(x => x.NodeName == nodeName);

                if (node == null)
                {
                    node = new AttachmentNode(attachment, nodeName, string.Join(@"\", path.Take(level + 1)), path.Length == level + 1);
                    parentNode.Items.Add(node);
                    parentNode.Items.Refresh();
                }

                CreateAttachmentNode(node, path, level + 1, attachment);
                node.ExpandSubtree();
            }
        }

        private void AddDrawing(Grizzlist.Tasks.Types.Drawing drawing)
        {
            DrawingEditorControl editor = new DrawingEditorControl(drawing, true);
            TabItem tabItem = new TabItem() { Header = drawing.Name };

            tabItem.Content = editor;
            tcTask.Items.Add(tabItem);
        }

        private string FormatActivity(TimeSpan activity)
        {
            return $"{(int)activity.TotalHours}:{activity.ToString(@"mm\:ss")}";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
