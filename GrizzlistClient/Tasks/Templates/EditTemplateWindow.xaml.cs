using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Templates;
using Grizzlist.Tasks.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks.Templates
{
    /// <summary>
    /// Interaction logic for EditTemplateWindow.xaml
    /// </summary>
    public partial class EditTemplateWindow : ValidatableWindow
    {
        private List<SubTask> subtasks = new List<SubTask>();

        public Template EditedTemplate { get; private set; }

        public EditTemplateWindow(Window owner, Template template = null)
        {
            Owner = owner;

            InitializeComponent();

            foreach (TaskPriority priority in Enum.GetValues(typeof(TaskPriority)))
                cbPriority.Items.Insert(0, priority);

            cbPriority.SelectedItem = TaskPriority.Normal;
            tbDaysToDeadline.Text = "0";

            AddValidator(new EmptyStringValidator(tbName));
            AddValidator(new EmptyStringValidator(tbDescription));
            AddValidator(new RangeIntegerValidator(tbDaysToDeadline, 0, 30));

            if (template != null)
            {
                Title = template.Task.Name;
                tbName.Text = template.Task.Name;
                tbDescription.Text = template.Task.Description;
                tbNote.Text = template.Task.Note;
                cbPriority.SelectedItem = template.Task.Priority;
                tbTags.Text = string.Join(", ", template.Task.Tags.Select(x => x.Value));
                tbDaysToDeadline.Text = template.DaysToDeadline.ToString();

                foreach (SubTask subtask in template.Task.SubTasks)
                    AddSubtask(subtask);

                EditedTemplate = template;
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
                if (EditedTemplate != null)
                {
                    EditedTemplate.Task.Name = tbName.Text;
                    EditedTemplate.Task.Description = tbDescription.Text;
                    EditedTemplate.Task.Note = tbNote.Text;
                    EditedTemplate.Task.Priority = (TaskPriority)cbPriority.SelectedItem;
                    EditedTemplate.Task.SubTasks.Clear();
                    EditedTemplate.Task.Tags.Clear();
                    EditedTemplate.DaysToDeadline = int.Parse(tbDaysToDeadline.Text);
                }
                else
                {
                    EditedTemplate = new Template { Task = new Task(tbName.Text, tbDescription.Text, tbNote.Text, (TaskPriority)cbPriority.SelectedItem, TaskState.Open, default(DateTime)), DaysToDeadline = int.Parse(tbDaysToDeadline.Text) };
                }

                if (subtasks.Count > 0)
                    EditedTemplate.Task.SubTasks.AddRange(subtasks);

                if (!string.IsNullOrEmpty(tbTags.Text))
                    EditedTemplate.Task.Tags.AddRange(tbTags.Text.Split(',').Select(x => new Tag() { Value = x.Trim() }));

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
