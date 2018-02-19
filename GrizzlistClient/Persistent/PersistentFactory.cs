using Grizzlist.Client.UserSettings;
using Grizzlist.FileSystem;
using Grizzlist.FileSystem.Repositories;
using Grizzlist.FileSystem.Serialization;
using Grizzlist.Logger;
using Grizzlist.Persistent;
using System.Collections.Generic;

namespace Grizzlist.Client.Persistent
{
    static class PersistentFactory
    {
        public static IPersistentContext GetContext()
        {
            return new FileSystemContext();
        }

        public static void ConfigureContext()
        {
            FileSystemContext.RepositoriesMapping.Add(typeof(Settings), () => new DefaultRepository<Settings, Settings.Persistent, long>(new DefaultSerializer<List<Settings.Persistent>>(@"Settings\user.dat")));
            Log.Debug("User settings repository added", null);
        }
    }
}
