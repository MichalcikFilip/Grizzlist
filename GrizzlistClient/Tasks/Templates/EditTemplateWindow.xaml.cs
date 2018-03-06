using Grizzlist.Client.Tasks.Attachments;
using Grizzlist.Client.Tasks.Drawings;
using Grizzlist.Client.Validators;
using Grizzlist.Client.Validators.BasicValidators;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Templates;
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

namespace Grizzlist.Client.Tasks.Templates
{
    /// <summary>
    /// Interaction logic for EditTemplateWindow.xaml
    /// </summary>
    public partial class EditTemplateWindow : ValidatableWindow
    {
        private List<SubTask> subtasks = new List<SubTask>();
        private List<Attachment> attachments = new List<Attachment>();
        private List<DrawingEditorControl> drawings = new List<DrawingEditorControl>();

        public Template EditedTemplate { get; private set; }

        public EditTemplateWindow(Window owner, Template template = null)
        {
            Owner = owner;

            InitializeComponent();

            btnConOperator.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionOperatorControl());
            btnConValue.MouseLeftButtonDown += (s, e) => AddCondition(new ConditionValueControl());

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

                if (template.Condition != null)
                {
                    if (template.Condition is ConditionValue)
                        AddCondition(new ConditionValueControl((ConditionValue)template.Condition));
                    if (template.Condition is ConditionOperator)
                        AddCondition(new ConditionOperatorControl((ConditionOperator)template.Condition));
                }

                foreach (Attachment attachment in template.Task.Attachments)
                    AddAttachment(attachment);

                foreach (Drawing drawing in template.Task.Drawings)
                    AddDrawing(drawing);

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

        private void AddCondition(ValidatableControl conditionControl)
        {
            btnConOperator.Visibility = Visibility.Collapsed;
            btnConValue.Visibility = Visibility.Collapsed;
            conditionControl.Margin = new Thickness(4);

            if (conditionControl is IConditionControl)
            {
                ((IConditionControl)conditionControl).RemoveClicked += () =>
                {
                    btnConOperator.Visibility = Visibility.Visible;
                    btnConValue.Visibility = Visibility.Visible;

                    pnlCondition.Children.Clear();
                };
            }

            pnlCondition.Children.Add(conditionControl);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                if (pnlCondition.Children.Count == 1 && pnlCondition.Children[0] is ValidatableControl && !((ValidatableControl)pnlCondition.Children[0]).IsValid())
                    return;

                foreach (DrawingEditorControl editor in drawings)
                    if (!editor.IsValid())
                        return;

                if (EditedTemplate != null)
                {
                    EditedTemplate.Task.Name = tbName.Text;
                    EditedTemplate.Task.Description = tbDescription.Text;
                    EditedTemplate.Task.Note = tbNote.Text;
                    EditedTemplate.Task.Priority = (TaskPriority)cbPriority.SelectedItem;
                    EditedTemplate.Task.SubTasks.Clear();
                    EditedTemplate.Task.Tags.Clear();
                    EditedTemplate.Task.Attachments.Clear();
                    EditedTemplate.Task.Drawings.Clear();
                    EditedTemplate.DaysToDeadline = int.Parse(tbDaysToDeadline.Text);
                    EditedTemplate.Condition = null;
                }
                else
                {
                    EditedTemplate = new Template { Task = new Task(tbName.Text, tbDescription.Text, tbNote.Text, (TaskPriority)cbPriority.SelectedItem, TaskState.Open, default(DateTime)), DaysToDeadline = int.Parse(tbDaysToDeadline.Text) };
                }

                if (subtasks.Count > 0)
                    EditedTemplate.Task.SubTasks.AddRange(subtasks);

                if (!string.IsNullOrEmpty(tbTags.Text))
                    EditedTemplate.Task.Tags.AddRange(tbTags.Text.Split(',').Select(x => new Tag() { Value = x.Trim() }));

                if (attachments.Count > 0)
                    EditedTemplate.Task.Attachments.AddRange(attachments);

                if (drawings.Count > 0)
                    EditedTemplate.Task.Drawings.AddRange(drawings.Select(x => x.Drawing));

                if (pnlCondition.Children.Count == 1 && pnlCondition.Children[0] is IConditionControl)
                    EditedTemplate.Condition = ((IConditionControl)pnlCondition.Children[0]).GetCondition();

                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
