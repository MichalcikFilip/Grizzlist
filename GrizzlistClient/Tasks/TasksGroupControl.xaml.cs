using Grizzlist.Client.Tasks.Selectors;
using Grizzlist.Logger;
using Grizzlist.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Grizzlist.Client.Tasks
{
    /// <summary>
    /// Interaction logic for TasksGroupControl.xaml
    /// </summary>
    public partial class TasksGroupControl : UserControl, ITaskControlOrderSelectors
    {
        private bool collapsed = true;
        private Dictionary<Task, TaskControl> tasks = new Dictionary<Task, TaskControl>();
        private List<KeyValuePair<Func<TaskControl, object>, bool>> orderSelectors = new List<KeyValuePair<Func<TaskControl, object>, bool>>();

        public Task SelectedTask { get { return tasks.Values.FirstOrDefault(x => x.IsSelected)?.Task; } }
        public IEnumerable<Task> Tasks { get { return tasks.Where(x => x.Value.Visibility == Visibility.Visible).Select(x => x.Key); } }
        public int Count { get { return tasks.Values.Count(x => x.Visibility == Visibility.Visible); } }
        public IStringSelector DateSelector { get; set; }
        public IBoolSelector ActivityVisibilitySelector { get; set; }
        public IBackgroundSelector BackgroundSelector { get; set; }
        public IGroupIconsSelector GroupIconsSelector { get; set; }

        public event Action<Task> OnTaskSelect;
        public event Action<Task> OnTaskSelected;
        public event Action<Task> TaskDoubleClick;
        public event Action<TaskControl> OnTaskControlCreated;
        public event Action<TaskControl> OnTaskControlRemove;

        public TasksGroupControl(string name)
        {
            InitializeComponent();

            tbName.Text = name;
            Collapse();
        }

        public void AddTask(Task task)
        {
            if (task != null && !tasks.ContainsKey(task))
            {
                tasks.Add(task, new TaskControl(task) { BackgroundSelector = BackgroundSelector, DateSelector = DateSelector, ActivityVisibilitySelector = ActivityVisibilitySelector });
                pnlTasks.Children.Add(tasks[task]);

                OnTaskControlCreated?.Invoke(tasks[task]);

                tasks[task].OnSelect += OnSelect;
                tasks[task].OnSelected += OnSelected;
                tasks[task].DoubleClick += DoubleClick;
                tasks[task].Update();

                Sort();
                Collapse();
                UpdateCount();

                Log.Debug($"Task {task.Name} was added to {tbName.Text} group", this);
            }
        }

        public void UpdateTask(Task task)
        {
            if (tasks.ContainsKey(task))
            {
                tasks[task].Update();

                Sort();
                RefreshIcons();
            }
        }

        public void RemoveTask(Task task)
        {
            if (task != null && tasks.ContainsKey(task))
            {
                OnTaskControlRemove?.Invoke(tasks[task]);

                tasks[task].OnSelect -= OnSelect;
                tasks[task].OnSelected -= OnSelected;
                tasks[task].DoubleClick -= DoubleClick;

                pnlTasks.Children.Remove(tasks[task]);
                tasks.Remove(task);

                Collapse();
                UpdateCount();

                Log.Debug($"Task {task.Name} was removed from {tbName.Text} group", this);
            }
        }

        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                foreach (TaskControl task in tasks.Values)
                    task.Visibility = Visibility.Visible;

                Log.Debug($"Search was cleared for {tbName.Text} group", this);
            }
            else
            {
                text = text.Trim().ToLower();

                foreach (TaskControl task in tasks.Values)
                    task.Visibility = task.Task.Tags.Any(x => x.Value.ToLower().Contains(text)) || task.Task.Name.ToLower().Contains(text) || task.Task.Description.ToLower().Contains(text) || task.Task.SubTasks.Any(x => x.Name.ToLower().Contains(text) || x.Description.ToLower().Contains(text)) ? Visibility.Visible : Visibility.Collapsed;

                Log.Debug($"Text {text} was searched in {tbName.Text} group", this);
            }

            UpdateCount();
            RefreshIcons();
            Collapse();
            Deselect();
        }

        public ITaskControlOrderSelectors AddOrderSelector(Func<TaskControl, object> keySelector, bool descending)
        {
            orderSelectors.Add(new KeyValuePair<Func<TaskControl, object>, bool>(keySelector, descending));

            return this;
        }

        public void ClearOrderSelectors()
        {
            orderSelectors.Clear();
        }

        private void Sort()
        {
            IOrderedEnumerable<TaskControl> orderedTasks = null;

            bool firstOrder = true;
            foreach (KeyValuePair<Func<TaskControl, object>, bool> orderSelector in orderSelectors)
            {
                if (orderSelector.Value)
                    orderedTasks = (firstOrder) ? tasks.Values.OrderByDescending(orderSelector.Key) : orderedTasks.ThenByDescending(orderSelector.Key);
                else
                    orderedTasks = (firstOrder) ? tasks.Values.OrderBy(orderSelector.Key) : orderedTasks.ThenBy(orderSelector.Key);

                firstOrder = false;
            }

            if (orderedTasks != null)
            {
                pnlTasks.Children.Clear();

                foreach (TaskControl task in orderedTasks)
                    pnlTasks.Children.Add(task);
            }
        }

        public void Collapse(bool collapse)
        {
            collapsed = collapse;
            Collapse();
        }

        private void Collapse()
        {
            if (Count > 0)
            {
                Deselect();

                if (!collapsed)
                {
                    rowContent.Height = new GridLength(1, GridUnitType.Star);
                    imgArrow.Source = new BitmapImage(new Uri("/Resources/arrowBottom.png", UriKind.Relative));
                }
                else
                {
                    rowContent.Height = new GridLength(0);
                    imgArrow.Source = new BitmapImage(new Uri("/Resources/arrowRight.png", UriKind.Relative));
                }
            }
            else
            {
                rowContent.Height = new GridLength(0);
                imgArrow.Source = new BitmapImage(new Uri("/Resources/arrowRight.png", UriKind.Relative));
            }
        }

        public void Deselect()
        {
            foreach (TaskControl taskControl in tasks.Values)
                taskControl.Deselect();
        }

        public void RefreshIcons()
        {
            GroupIconsSelector?.Select(this, pnlIcons);
        }

        private void UpdateCount()
        {
            tbCount.Text = Count.ToString();
            RefreshIcons();
        }

        private void Arrow_Click(object sender, RoutedEventArgs e)
        {
            if (Count > 0)
                Collapse(!collapsed);
        }

        private void OnSelect(Task task)
        {
            OnTaskSelect?.Invoke(task);
        }

        private void OnSelected(Task task)
        {
            OnTaskSelected?.Invoke(task);
        }

        private void DoubleClick(Task task)
        {
            TaskDoubleClick?.Invoke(task);
        }
    }
}
