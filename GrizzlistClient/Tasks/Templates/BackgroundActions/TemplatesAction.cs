using Grizzlist.Client.BackgroundActions;
using Grizzlist.Tasks;
using System;

namespace Grizzlist.Client.Tasks.Templates.BackgroundActions
{
    class TemplatesAction : BaseAction
    {
        public event Action<Task> TaskCreated;

        public TemplatesAction()
            : base(3600)
        { }

        protected override void RunAction()
        {

        }
    }
}
