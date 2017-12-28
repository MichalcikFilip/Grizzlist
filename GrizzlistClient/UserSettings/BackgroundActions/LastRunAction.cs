using Grizzlist.Client.BackgroundActions;
using Grizzlist.Client.Persistent;
using Grizzlist.Logger;
using Grizzlist.Persistent;
using System;

namespace Grizzlist.Client.UserSettings.BackgroundActions
{
    class LastRunAction : BaseAction
    {
        public LastRunAction()
            : base(600)
        { }

        protected override void RunAction()
        {
            using (IRepository<Settings, long> repository = PersistentFactory.GetContext().GetRepository<Settings, long>())
            {
                Settings settings = SettingsFactory.GetSettings(repository);
                settings.LastRun = DateTime.Now;
                repository.Update(settings);
            }

            Log.Debug("Last run in user settings was updated", this);
        }
    }
}
