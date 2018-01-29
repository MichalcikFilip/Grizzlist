using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.Notifications;
using Grizzlist.Logger;
using Grizzlist.Notifications;
using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.BackgroundActions
{
    class TaskDeadlineAction : BaseAction
    {
        private DeadlineState lastState = DeadlineState.None;

        public Task Task { get; private set; }

        public TaskDeadlineAction(Task task)
            : base(1800)
        {
            Task = task;
        }

        protected override void RunAction()
        {
            if (Task != null)
            {
                if (Task.Deadline == DateTime.Today && lastState != DeadlineState.DeadlineEnds)
                {
                    NotificationHelper.Notify(NotificationType.TaskDeadlineEnds, Task.Name);
                    lastState = DeadlineState.DeadlineEnds;

                    Log.Debug($"Background action created deadline notification for task {Task.Name}", this);
                }
                else if (Task.Deadline < DateTime.Today && lastState != DeadlineState.DeadlineExceeded)
                {
                    NotificationHelper.Notify(NotificationType.TaskDeadlineExceeded, Task.Name);
                    lastState = DeadlineState.DeadlineExceeded;

                    Log.Debug($"Background action created deadline notification for task {Task.Name}", this);
                }
            }
        }

        private enum DeadlineState { None, DeadlineEnds, DeadlineExceeded }
    }
}
