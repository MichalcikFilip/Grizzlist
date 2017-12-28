using Grizzlist.FileSystem.Persistent;
using Grizzlist.Persistent;
using System;

namespace Grizzlist.Client.UserSettings
{
    class Settings : PersistentEntity<long>
    {
        public DateTime LastRun { get; set; }

        [Serializable]
        public class Persistent : IPersistentEntity<long>, IConvertible<Settings, Persistent, long>, IIncrementalKey<long>
        {
            public long ID { get; set; }
            public DateTime LastRun { get; set; }

            public Settings Convert()
            {
                return new Settings() { ID = ID, LastRun = LastRun };
            }

            public Persistent Convert(Settings entity)
            {
                return new Persistent() { ID = entity.ID, LastRun = entity.LastRun };
            }

            public virtual bool CanAutoIncrement()
            {
                return true;
            }

            public virtual long GetNewKey(long lastKey)
            {
                return ++lastKey;
            }
        }
    }
}
