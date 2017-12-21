using Grizzlist.Client.Tasks.Validators;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Extensions;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : ValidatableWindow
    {
        private List<SubTask> subtasks = new List<SubTask>();

        public Task EditedTask { get; private set; }

        public EditTaskWindow(Window owner, Task task = null)
        {
            Owner = owner;

            InitializeComponent();

            foreach (TaskPriority priority in Enum.GetValues(typeof(TaskPriority)))
                cbPriority.Items.Insert(0, priority);

            cbPriority.SelectedItem = TaskPriority.Normal;

            AddValidator(new EmptyStringValidator(tbName));
            AddValidator(new EmptyStringValidator(tbDescription));
            AddValidator(new DeadlineValidator(dpDeadline, task != null && task.Deadline < DateTime.Today ? task.Deadline : DateTime.Today));

            if (task != null)
            {
                Title = task.Name;
                tbName.Text = task.Name;
                tbDescription.Text = task.Description;
                tbNote.Text = task.Note;
                cbPriority.SelectedItem = task.Priority;
                dpDeadline.SelectedDate = task.Deadline;
                tbTags.Text = string.Join(", ", task.Tags.Select(x => x.Value));

                foreach (SubTask subtask in task.SubTasks)
                    AddSubtask(subtask);

                EditedTask = task;
            }
        }

        private void AddSubtask(SubTask subtask)
        {
            subtasks.Add(subtask);

            RowDefinition rowDefinition = new RowDefinition() { Height = new GridLength(30) };
            rowSubtasks.Height = new GridLength(rowSubtasks.Height.Value + rowDefinition.Height.Value);
            gridSubtasks.RowDefinitions.Add(rowDefinition);

            Label lblSubtask = new Label() { Margin = new Thickness(50, 1, 0, 0), FontWeight = FontWeights.Bold, Content = subtask.Name, ToolTip = !string.IsNullOrEmpty(subtask.Description) ? subtask.Description : null };
            Button btnEdit = new Button() { Margin = new Thickness(5), Style = FindResource("ImageButton") as Style, ToolTip = "Update Subtask", Content = new Image() { Source = new BitmapImage(new Uri("/Resources/edit_24.png", UriKind.Relative)) } };
            Button btnRemove = new Button() { Margin = new Thickness(5), Style = FindResource("ImageButton") as Style, ToolTip = "Remove Subtask", Content = new Image() { Source = new BitmapImage(new Uri("/Resources/remove_24_red.png", UriKind.Relative)) } };

            int row = gridSubtasks.RowDefinitions.Count - 1;

            gridSubtasks.Children.Add(lblSubtask);
            Grid.SetColumn(lblSubtask, 0);
            Grid.SetRow(lblSubtask, row);

            gridSubtasks.Children.Add(btnEdit);
            Grid.SetColumn(btnEdit, 1);
            Grid.SetRow(btnEdit, row);

            gridSubtasks.Children.Add(btnRemove);
            Grid.SetColumn(btnRemove, 2);
            Grid.SetRow(btnRemove, row);

            btnEdit.Click += (sender, e) => {
                EditSubTaskWindow window = new EditSubTaskWindow(this, subtask);

                if (window.ShowDialog() ?? false)
                {
                    lblSubtask.Content = subtask.Name;
                    lblSubtask.ToolTip = !string.IsNullOrEmpty(subtask.Description) ? subtask.Description : null;
                }
            };

            btnRemove.Click += (sender, e) => {
                subtasks.Remove(subtask);

                gridSubtasks.Children.Remove(lblSubtask);
                gridSubtasks.Children.Remove(btnEdit);
                gridSubtasks.Children.Remove(btnRemove);

                int rowIndex = gridSubtasks.RowDefinitions.IndexOf(rowDefinition);
                foreach (UIElement child in gridSubtasks.Children)
                    if (Grid.GetRow(child) > rowIndex)
                        Grid.SetRow(child, Grid.GetRow(child) - 1);

                gridSubtasks.RowDefinitions.Remove(rowDefinition);
                rowSubtasks.Height = new GridLength(rowSubtasks.Height.Value - rowDefinition.Height.Value);
            };
        }

        private void AddSubtask_Click(object sender, RoutedEventArgs e)
        {
            EditSubTaskWindow window = new EditSubTaskWindow(this);

            if (window.ShowDialog() ?? false)
                AddSubtask(window.EditedTask);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (EditedTask != null)
                {
                    EditedTask.Name = tbName.Text;
                    EditedTask.Description = tbDescription.Text;
                    EditedTask.Note = tbNote.Text;
                    EditedTask.Priority = (TaskPriority)cbPriority.SelectedItem;
                    EditedTask.Deadline = dpDeadline.SelectedDate ?? DateTime.Now.GetDate();
                    EditedTask.Updated = DateTime.Now;
                    EditedTask.SubTasks.Clear();
                    EditedTask.Tags.Clear();
                }
                else
                {
                    EditedTask = new Task(tbName.Text, tbDescription.Text, tbNote.Text, (TaskPriority)cbPriority.SelectedItem, TaskState.Open, dpDeadline.SelectedDate ?? DateTime.Now.GetDate());
                }

                if (subtasks.Count > 0)
                    EditedTask.SubTasks.AddRange(subtasks);

                if (!string.IsNullOrEmpty(tbTags.Text))
                    EditedTask.Tags.AddRange(tbTags.Text.Split(',').Select(x => new Tag() { Value = x.Trim() }));

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
