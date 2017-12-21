using Grizzlist.Client.BackgroundActions;
using Grizzlist.Logger;
using System;

namespace Grizzlist.Client.Tasks.BackgroundActions
{
    class TaskControlBackgroundAction : BaseAction
    {
        public TaskControl TaskControl { get; private set; }

        public TaskControlBackgroundAction(TaskControl taskControl)
            : base(60)
        {
            TaskControl = taskControl;
        }

        protected override void RunAction()
        {
            if (TaskControl != null && TaskControl.Task.Deadline <= DateTime.Today)
            {
                TaskControl.Dispatcher.Invoke(() => TaskControl.UpdateBackground());
                Log.Debug($"Background action refreshed task {TaskControl.Task.Name} background", this);
            }
        }
    }
}
