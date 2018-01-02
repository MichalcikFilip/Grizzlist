using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.Extensions;
using Grizzlist.Client.Notifications;
using Grizzlist.Client.Persistent;
using Grizzlist.Client.Stats;
using Grizzlist.Client.Tasks;
using Grizzlist.Client.Tasks.BackgroundActions;
using Grizzlist.Client.Tasks.Selectors;
using Grizzlist.Client.Tasks.Templates.BackgroundActions;
using Grizzlist.Client.UserSettings.BackgroundActions;
using Grizzlist.Logger;
using Grizzlist.Notifications;
using Grizzlist.Persistent;
using Grizzlist.Stats;
using Grizzlist.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Grizzlist.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifiable
    {
        private List<TasksGroupControl> groups = new List<TasksGroupControl>();

        private TasksGroupControl groupOpen;
        private TasksGroupControl groupPostponed;
        private TasksGroupControl groupClosed;

        private BackgroundThread backgroundThread = new BackgroundThread();
        private bool deselect = true;

        public MainWindow()
        {
            InitializeComponent();

            NotificationManager.Instance.Targets.Add(this);
            Closing += (sender, e) => NotificationManager.Instance.Targets.Remove(this);

            groupOpen = new TasksGroupControl("Open tasks");
            groupPostponed = new TasksGroupControl("Postponed tasks");
            groupClosed = new TasksGroupControl("Closed tasks");

            groups.Add(groupOpen);
            groups.Add(groupPostponed);
            groups.Add(groupClosed);

            foreach (TasksGroupControl group in groups)
            {
                pnlGroups.Children.Add(group);
                group.OnTaskSelect += OnTaskSelect;
                group.OnTaskSelected += OnTaskSelected;
            }

            groupOpen.BackgroundSelector = new OpenTaskBackgroundSelector();
            groupPostponed.BackgroundSelector = new OpenTaskBackgroundSelector();
            groupClosed.BackgroundSelector = new TaskBackgroundSelector();
            groupOpen.DateSelector = new DeadlineSelector();
            groupPostponed.DateSelector = new DeadlineSelector();
            groupClosed.DateSelector = new ClosedSelector();
            groupOpen.ActivityVisibilitySelector = new SimpleBoolSelector(true);
            groupPostponed.ActivityVisibilitySelector = new SimpleBoolSelector(false);
            groupClosed.ActivityVisibilitySelector = new SimpleBoolSelector(false);
            groupOpen.GroupIconsSelector = new DeadlineIconsSelector();
            groupPostponed.GroupIconsSelector = new DeadlineIconsSelector();
            groupOpen.TaskDoubleClick += t => Command_EditTask(null, null);
            groupPostponed.TaskDoubleClick += t => Command_EditTask(null, null);
            groupClosed.TaskDoubleClick += t => Command_ShowTask(null, null);
            groupOpen.AddOrderSelector(x => x.Task.Deadline, false).AddOrderSelector(x => x.Task.Priority, true);
            groupPostponed.AddOrderSelector(x => x.Task.Deadline, false).AddOrderSelector(x => x.Task.Priority, true);
            groupClosed.AddOrderSelector(x => x.Task.Closed, true);
            groupOpen.OnTaskControlCreated += t => ActionsCollection.Instance.Add(new TaskControlBackgroundAction(t));
            groupPostponed.OnTaskControlCreated += t => ActionsCollection.Instance.Add(new TaskControlBackgroundAction(t));
            groupOpen.OnTaskControlRemove += t => ActionsCollection.Instance.Remove(ActionsCollection.Instance.Actions.OfType<TaskControlBackgroundAction>().FirstOrDefault(x => x.TaskControl == t));
            groupPostponed.OnTaskControlRemove += t => ActionsCollection.Instance.Remove(ActionsCollection.Instance.Actions.OfType<TaskControlBackgroundAction>().FirstOrDefault(x => x.TaskControl == t));
            groupOpen.Collapse(false);

            ActionsCollection.Instance.Add(new DeadlineIconsAction(groupOpen));
            ActionsCollection.Instance.Add(new DeadlineIconsAction(groupPostponed));

            using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
            {
                foreach (Task task in repository.GetAll())
                {
                    switch (task.State)
                    {
                        case TaskState.Open:
                            groupOpen.AddTask(task);
                            ActionsCollection.Instance.Add(new TaskDeadlineAction(task));
                            break;
                        case TaskState.Postponed:
                            groupPostponed.AddTask(task);
                            ActionsCollection.Instance.Add(new TaskDeadlineAction(task));
                            break;
                        case TaskState.Closed:
                            groupClosed.AddTask(task);
                            break;
                    }
                }
            }

            TemplatesAction templatesAction = new TemplatesAction();

            templatesAction.TaskCreated += task =>
            {
                if (task != null)
                {
                    groupOpen.AddTask(task);

                    using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                        repository.Add(task);

                    StatsHelper.Update(StatsData.TaskCreated);
                    StatsHelper.Update(StatsData.TaskCreatedFromTemplate);
                    NotificationHelper.Notify(NotificationType.TaskCreatedFromTemplate, task.Name);

                    ActionsCollection.Instance.Add(new TaskDeadlineAction(task));
                }
            };

            ActionsCollection.Instance.Add(templatesAction);
            ActionsCollection.Instance.Add(new LastRunAction());

            backgroundThread.Start();
            RefreshMenu();

            Log.Debug($"{groups.Sum(x => x.Count)} tasks loaded", this);
            Log.Info("Main window opened", this);
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            string text = tbSearch.Text.Trim();

            groups.ForEach(x => x.Search(text));
            RefreshMenu();

            pnlSearch.BorderBrush = new SolidColorBrush(string.IsNullOrEmpty(text) ? Color.FromRgb(171, 173, 179) : Colors.Green);
        }

        private void SearchClearClick(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            SearchClick(sender, e);
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SearchClick(sender, e);
        }

        public void Notify(Notification notification)
        {
            Border notificationControl = new Border() { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(NotificationHelper.Colors[notification.Type]), Background = new SolidColorBrush(Colors.White), Margin = new Thickness(15, 5, 15, 5), Child = new TextBlock() { Background = new SolidColorBrush(NotificationHelper.Colors[notification.Type].SetAlpha(NotificationsWindow.NOTIFICATION_BACKGROUND_ALPHA)), Padding = new Thickness(6), Text = $"{notification.Created}{Environment.NewLine}{notification.FillMessage(NotificationHelper.Messages[notification.Type])}" } };
            Timer hideNotification = new Timer(5000);

            pnlNotifications.Children.Add(notificationControl);
            hideNotification.Elapsed += (sender, e) => pnlNotifications.Dispatcher.BeginInvoke(new Action(() => pnlNotifications.Children.Remove(notificationControl)));

            hideNotification.Start();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("cmd");
        }

        private void Command_OpenNotifications(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            new NotificationsWindow(this).ShowDialog();
        }

        private void Command_OpenStats(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            new StatsWindow(this).ShowDialog();
        }

        private void Command_OpenChangelog(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            new ChangelogWindow(this).ShowDialog();
        }

        private void Command_AboutGrizzlist(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            new AboutWindow(this).ShowDialog();
        }

        private void Command_CloseApp(object sender, ExecutedRoutedEventArgs e)
        {
            Log.Info("Main window closing", this);
            Application.Current.Shutdown();
        }

        private void Command_NewTask(object sender, ExecutedRoutedEventArgs e)
        {
            EditTaskWindow window = new EditTaskWindow(this);

            if (window.ShowDialog() ?? false)
            {
                groupOpen.AddTask(window.EditedTask);
                ActionsCollection.Instance.Add(new TaskDeadlineAction(window.EditedTask));

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                    repository.Add(window.EditedTask);

                StatsHelper.Update(StatsData.TaskCreated);
                NotificationHelper.Notify(NotificationType.TaskCreated, window.EditedTask.Name);
            }
        }

        private void Command_ShowTask(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            Task selectedTask = groups.FirstOrDefault(x => x.SelectedTask != null)?.SelectedTask;

            if (selectedTask != null)
                new ViewTaskWindow(this, selectedTask).ShowDialog();
        }

        private void Command_EditTask(object sender, ExecutedRoutedEventArgs e)
        {
            deselect = false;
            TasksGroupControl selectedGroup = groups.FirstOrDefault(x => x.SelectedTask != null);

            if (selectedGroup != null && (selectedGroup.SelectedTask.State == TaskState.Open || selectedGroup.SelectedTask.State == TaskState.Postponed))
            {
                EditTaskWindow window = new EditTaskWindow(this, selectedGroup.SelectedTask);

                if (window.ShowDialog() ?? false)
                {
                    selectedGroup.UpdateTask(window.EditedTask);

                    using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                        repository.Update(window.EditedTask);

                    NotificationHelper.Notify(NotificationType.TaskUpdated, window.EditedTask.Name);
                }
            }
        }

        private void Command_OpenTask(object sender, ExecutedRoutedEventArgs e)
        {
            if (groupPostponed.SelectedTask != null)
            {
                Task selectedTask = groupPostponed.SelectedTask;
                selectedTask.State = TaskState.Open;
                selectedTask.Updated = DateTime.Now;

                groupPostponed.RemoveTask(selectedTask);
                groupOpen.AddTask(selectedTask);

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                    repository.Update(selectedTask);

                NotificationHelper.Notify(NotificationType.TaskOpened, selectedTask.Name);
            }
        }

        private void Command_PostponeTask(object sender, ExecutedRoutedEventArgs e)
        {
            if (groupOpen.SelectedTask != null)
            {
                Task selectedTask = groupOpen.SelectedTask;
                selectedTask.State = TaskState.Postponed;
                selectedTask.Updated = DateTime.Now;

                groupOpen.RemoveTask(selectedTask);
                groupPostponed.AddTask(selectedTask);

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                    repository.Update(selectedTask);

                NotificationHelper.Notify(NotificationType.TaskDeferred, selectedTask.Name);
            }
        }

        private void Command_CloseTask(object sender, ExecutedRoutedEventArgs e)
        {
            TasksGroupControl selectedGroup = groups.FirstOrDefault(x => x.SelectedTask != null);

            if (selectedGroup != null && (selectedGroup.SelectedTask.State == TaskState.Open || selectedGroup.SelectedTask.State == TaskState.Postponed))
            {
                Task selectedTask = selectedGroup.SelectedTask;
                selectedTask.Close();

                selectedGroup.RemoveTask(selectedTask);
                groupClosed.AddTask(selectedTask);
                ActionsCollection.Instance.Remove(ActionsCollection.Instance.Actions.OfType<TaskDeadlineAction>().FirstOrDefault(x => x.Task == selectedTask));

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                    repository.Update(selectedTask);

                StatsHelper.Update(StatsData.TaskClosed);
                StatsHelper.Update(StatsHelper.PriorityUpdate(selectedTask.Priority));
                NotificationHelper.Notify(NotificationType.TaskClosed, selectedTask.Name);
            }
        }

        private void Command_RemoveTask(object sender, ExecutedRoutedEventArgs e)
        {
            TasksGroupControl selectedGroup = groups.FirstOrDefault(x => x.SelectedTask != null);

            if (selectedGroup != null)
            {
                Task selectedTask = selectedGroup.SelectedTask;
                selectedTask.State = TaskState.Removed;

                selectedGroup.RemoveTask(selectedTask);
                ActionsCollection.Instance.Remove(ActionsCollection.Instance.Actions.OfType<TaskDeadlineAction>().FirstOrDefault(x => x.Task == selectedTask));

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                    repository.Update(selectedTask);

                StatsHelper.Update(StatsData.TaskRemoved);
                NotificationHelper.Notify(NotificationType.TaskRemoved, selectedTask.Name);
            }
        }

        private void RefreshMenu()
        {
            btnShowTask.IsEnabled = groups.Any(x => x.SelectedTask != null);
            btnEditTask.IsEnabled = groupOpen.SelectedTask != null || groupPostponed.SelectedTask != null;
            btnOpenTask.IsEnabled = groupPostponed.SelectedTask != null;
            btnPostponeTask.IsEnabled = groupOpen.SelectedTask != null;
            btnCloseTask.IsEnabled = groupOpen.SelectedTask != null || groupPostponed.SelectedTask != null;
            btnRemoveTask.IsEnabled = groups.Any(x => x.SelectedTask != null);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (deselect)
                DeselectAll();
            else
                deselect = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            backgroundThread.Stop();
        }

        private void Groups_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DeselectAll();
        }

        private void OnTaskSelect(Task task)
        {
            DeselectAll();
        }

        private void OnTaskSelected(Task task)
        {
            RefreshMenu();
        }

        private void DeselectAll()
        {
            groups.ForEach(x => x.Deselect());
            RefreshMenu();
        }
    }
}
