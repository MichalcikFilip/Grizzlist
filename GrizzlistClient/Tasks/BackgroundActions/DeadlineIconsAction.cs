using Grizzlist.Client.BackgroundActions;
using Grizzlist.Logger;
using System;
using System.Linq;

namespace Grizzlist.Client.Tasks.BackgroundActions
{
    class DeadlineIconsAction : BaseAction
    {
        public TasksGroupControl GroupControl { get; private set; }

        public DeadlineIconsAction(TasksGroupControl groupControl)
            : base(60)
        {
            GroupControl = groupControl;
        }

        protected override void RunAction()
        {
            if (GroupControl != null && GroupControl.Tasks.Any(x => x.Deadline <= DateTime.Today))
            {
                GroupControl.Dispatcher.Invoke(() => GroupControl.RefreshIcons());
                Log.Debug("Background action refreshed tasks group icons", this);
            }
        }
    }
}
