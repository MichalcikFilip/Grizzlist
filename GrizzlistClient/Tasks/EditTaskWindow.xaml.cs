using Grizzlist.Client.Tasks.Attachments;
using Grizzlist.Client.Tasks.Drawings;
using Grizzlist.Client.Tasks.Validators;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Extensions;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Types;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : ValidatableWindow
    {
        private List<SubTask> subtasks = new List<SubTask>();
        private List<Attachment> attachments = new List<Attachment>();
        private List<DrawingEditorControl> drawings = new List<DrawingEditorControl>();

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

                foreach (Attachment attachment in task.Attachments)
                    AddAttachment(attachment);

                foreach (Drawing drawing in task.Drawings)
                    AddDrawing(drawing);

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

        private void AddAttachment(Attachment attachment)
        {
            if (!attachments.Any(x => x.Path == attachment.Path))
            {
                attachments.Add(attachment);
                CreateAttachmentNode(tvAttachments, attachment.Path.Split('\\'), 0, attachment);
            }
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

        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog() { Multiselect = true };

            if (dlg.ShowDialog(this) ?? false)
                foreach (string fileName in dlg.FileNames)
                    AddAttachment(new Attachment() { Path = fileName });
        }

        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (tvAttachments.SelectedItem != null)
            {
                AttachmentNode selectedNode = (AttachmentNode)tvAttachments.SelectedItem;
                ((ItemsControl)selectedNode.Parent).Items.Remove(selectedNode);
                attachments.RemoveAll(x => x.Path.StartsWith(selectedNode.FullPath));
            }
        }

        private void UpdateAttachmentNode_Click(object sender, RoutedEventArgs e)
        {
            if (tvAttachments.SelectedItem != null)
            {
                AttachmentNode selectedNode = (AttachmentNode)tvAttachments.SelectedItem;

                if (selectedNode.IsFile)
                {
                    AttachmentNoteWindow dlg = new AttachmentNoteWindow(this) { Note = selectedNode.Attachment.Note };

                    if (dlg.ShowDialog() ?? false)
                    {
                        selectedNode.UpdateNote(dlg.Note);
                        attachments.RemoveAll(x => x.Path == selectedNode.Attachment.Path);
                        attachments.Add(new Attachment() { Path = selectedNode.Attachment.Path, Note = dlg.Note });
                    }
                }
            }
        }

        private TabItem AddDrawing(Drawing drawing)
        {
            DrawingEditorControl editor = new DrawingEditorControl(drawing);
            TabItem tabItem = new TabItem() { Header = drawing.Name };

            editor.OnRename += name => tabItem.Header = name;
            editor.OnValidation += result => tabItem.BorderBrush = result ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(172, 172, 172)) : System.Windows.Media.Brushes.Red;
            editor.OnDelete += () =>
            {
                drawings.Remove(editor);
                tcTask.Items.Remove(tabItem);
            };

            tabItem.Content = editor;
            tcTask.Items.Add(tabItem);
            drawings.Add(editor);

            return tabItem;
        }

        private void AddDrawing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddDrawing(new Drawing() { Name = "New drawing", Image = new System.Drawing.Bitmap(250, 250) }).IsSelected = true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                foreach (DrawingEditorControl editor in drawings)
                    if (!editor.IsValid())
                        return;

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
                    EditedTask.Attachments.Clear();
                    EditedTask.Drawings.Clear();
                }
                else
                {
                    EditedTask = new Task(tbName.Text, tbDescription.Text, tbNote.Text, (TaskPriority)cbPriority.SelectedItem, TaskState.Open, dpDeadline.SelectedDate ?? DateTime.Now.GetDate());
                }

                if (subtasks.Count > 0)
                    EditedTask.SubTasks.AddRange(subtasks);

                if (!string.IsNullOrEmpty(tbTags.Text))
                    EditedTask.Tags.AddRange(tbTags.Text.Split(',').Select(x => new Tag() { Value = x.Trim() }));

                if (attachments.Count > 0)
                    EditedTask.Attachments.AddRange(attachments);

                if (drawings.Count > 0)
                    EditedTask.Drawings.AddRange(drawings.Select(x => x.Drawing));

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
