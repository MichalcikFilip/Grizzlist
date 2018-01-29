using Grizzlist.Logger;
using Grizzlist.Persistent;
using System.Linq;

namespace Grizzlist.Client.UserSettings
{
    static class SettingsFactory
    {
        public static Settings GetSettings(IRepository<Settings,long> repository)
        {
            Settings settings = repository.GetAll().FirstOrDefault();

            if (settings == null)
            {
                settings = new Settings();
                repository.Add(settings);

                Log.Debug("User settings were created", null);
            }

            return settings;
        }
    }
}
