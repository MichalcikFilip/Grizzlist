using Grizzlist.Client.Persistent;
using Grizzlist.Client.Tasks.Selectors;
using Grizzlist.Persistent;
using Grizzlist.Tasks;
using Grizzlist.Tasks.Types;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        public Task Task { get; private set; }
        public bool IsSelected { get; private set; }

        public IBackgroundSelector BackgroundSelector { get; set; }
        public IStringSelector DateSelector { get; set; }
        public IBoolSelector ActivityVisibilitySelector { get; set; }

        public event Action<Task> OnSelect;
        public event Action<Task> OnSelected;
        public event Action<Task> DoubleClick;

        public TaskControl(Task task)
        {
            Task = task;

            InitializeComponent();
            HideNote();
        }

        public void Update()
        {
            btnActivity.Visibility = ActivityVisibilitySelector?.Select() ?? false ? Visibility.Visible : Visibility.Collapsed;
            btnActivity.Source = new BitmapImage(new Uri(Task.IsActive ? "/Resources/recActive.png" : "/Resources/rec.png", UriKind.Relative));
            btnActivity.MouseLeftButtonDown += (sender, e) => ActivityClick(Task, btnActivity);

            pnlPriority.Fill = new SolidColorBrush(TaskPriorityColors.GetColor(Task.Priority));
            tbName.Text = Task.Name;
            tbDate.Text = DateSelector?.Select(Task) ?? string.Empty;
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

                imgCompleted.MouseLeftButtonDown += (sender, e) => {
                    if (!subtask.Completed)
                    {
                        subtask.Close();
                        Update();

                        using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                            repository.Update(Task);
                    }
                };

                if (!subtask.Completed && (ActivityVisibilitySelector?.Select() ?? false))
                {
                    Image imgActivity = new Image() { Source = new BitmapImage(new Uri(subtask.IsActive ? "/Resources/recActive.png" : "/Resources/rec.png", UriKind.Relative)), Margin = new Thickness(2, 2, 2, string.IsNullOrEmpty(subtask.Description) ? 10 : 2) };

                    gridSubtasks.Children.Add(imgActivity);

                    Grid.SetColumn(imgActivity, 2);
                    Grid.SetRow(imgActivity, gridSubtasks.RowDefinitions.Count - 1);

                    imgActivity.MouseLeftButtonDown += (sender, e) => ActivityClick(subtask, imgActivity);
                }

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

            if (string.IsNullOrEmpty(Task.Note) && Task.Attachments.Count == 0 && Task.Drawings.Count == 0)
                rowAttachments.Height = new GridLength(0);
            else
                rowAttachments.Height = new GridLength(1, GridUnitType.Star);

            imgNote.Visibility = string.IsNullOrEmpty(Task.Note) ? Visibility.Collapsed : Visibility.Visible;
            imgDrawing.Visibility = Task.Drawings.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            imgFile.Visibility = Task.Attachments.Count == 0 ? Visibility.Collapsed : Visibility.Visible;

            if (string.IsNullOrEmpty(Task.Note))
                HideNote();

            tbNote.Text = Task.Note;

            if (Task.Tags.Count == 0)
                rowTags.Height = new GridLength(0);
            else
                rowTags.Height = new GridLength(1, GridUnitType.Star);

            pnlTags.Children.Clear();
            foreach (Tag tag in Task.Tags)
                pnlTags.Children.Add(new Border() { Margin = new Thickness(6), Padding = new Thickness(4, 2, 4, 2), CornerRadius = new CornerRadius(4), Background = new SolidColorBrush(Color.FromRgb(255, 244, 204)), BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Color.FromRgb(255, 216, 0)), Child = new TextBlock() { Text = tag.Value } });

            UpdateBackground();
        }

        public void UpdateBackground()
        {
            mainGrid.Background = new SolidColorBrush(BackgroundSelector?.Select(Task, IsSelected) ?? Colors.White);
        }

        private void ActivityClick(BaseTask task, Image image)
        {
            if (task.IsActive)
            {
                task.CloseActivity();
                image.Source = new BitmapImage(new Uri("/Resources/rec.png", UriKind.Relative));
            }
            else
            {
                task.StartActivity();
                image.Source = new BitmapImage(new Uri("/Resources/recActive.png", UriKind.Relative));
            }

            using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                repository.Update(Task);
        }

        private void ShowNote()
        {
            rowNote.Height = new GridLength(1, GridUnitType.Star);
            imgNote.Source = new BitmapImage(new Uri("/Resources/noteOpen.png", UriKind.Relative));
        }

        private void HideNote()
        {
            rowNote.Height = new GridLength(0);
            imgNote.Source = new BitmapImage(new Uri("/Resources/note.png", UriKind.Relative));
        }

        private void Note_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (rowNote.Height.Value == 0)
                ShowNote();
            else
                HideNote();
        }

        public void Deselect()
        {
            IsSelected = false;
            UpdateBackground();
        }

        private void Select(object sender, MouseButtonEventArgs e)
        {
            OnSelect?.Invoke(Task);

            IsSelected = true;
            UpdateBackground();

            OnSelected?.Invoke(Task);
            e.Handled = true;

            if (e.ClickCount == 2)
                DoubleClick?.Invoke(Task);
        }

        private void SelectUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
