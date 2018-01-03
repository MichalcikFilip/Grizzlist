using Grizzlist.Client.BackgroundActions;
using Grizzlist.Logger;
using System;

namespace Grizzlist.Client.Tasks.BackgroundActions
{
    class TaskControlAction : BaseAction
    {
        public TaskControl TaskControl { get; private set; }

        public TaskControlAction(TaskControl taskControl)
            : base(60)
        {
            TaskControl = taskControl;
        }

        protected override void RunAction()
        {
            if (TaskControl != null)
            {
                if (TaskControl.Task.Deadline <= DateTime.Today)
                {
                    TaskControl.Dispatcher.Invoke(() => TaskControl.UpdateBackground());
                    Log.Debug($"Task control action refreshed task {TaskControl.Task.Name} background", this);
                }

                TaskControl.Dispatcher.Invoke(() => TaskControl.UpdateDate());
                Log.Debug($"Task control action refreshed task {TaskControl.Task.Name} date", this);
            }
        }
    }
}
