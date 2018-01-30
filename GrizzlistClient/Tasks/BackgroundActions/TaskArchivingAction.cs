using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.Notifications;
using Grizzlist.Client.Persistent;
using Grizzlist.Client.Stats;
using Grizzlist.Logger;
using Grizzlist.Notifications;
using Grizzlist.Persistent;
using Grizzlist.Stats;
using Grizzlist.Tasks;
using System;
using System.Linq;

namespace Grizzlist.Client.Tasks.BackgroundActions
{
    class TaskArchivingAction : BaseAction
    {
        private TasksGroupControl closedTasksGroup;

        public TaskArchivingAction(TasksGroupControl closedTasksGroup)
            : base(600)
        {
            this.closedTasksGroup = closedTasksGroup;
        }

        protected override void RunAction()
        {
            if (closedTasksGroup != null)
            {
                DateTime today = DateTime.Today;

                using (IRepository<Task, long> repository = PersistentFactory.GetContext().GetRepository<Task, long>())
                {
                    foreach (Task task in closedTasksGroup.AllTasks.ToList())
                    {
                        if ((today - task.Closed).TotalDays > 60)
                        {
                            task.State = TaskState.Archived;

                            closedTasksGroup.Dispatcher.Invoke(() => closedTasksGroup.RemoveTask(task));
                            repository.Update(task);

                            StatsHelper.Update(StatsData.TaskArchived);
                            NotificationHelper.Notify(NotificationType.TaskArchived, task.Name);

                            Log.Debug($"Background action archived {task.Name} task", this);
                        }
                    }
                }
            }
        }
    }
}
