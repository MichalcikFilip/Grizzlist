using Grizzlist.Persistent;
using Grizzlist.Tasks.Types;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Types
{
    [Serializable]
    struct PersistentActivity : IPersistentEntity<long>, IConvertible<Activity, PersistentActivity, long>, IIncrementalKey<long>
    {
        public long ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Activity Convert()
        {
            return new Activity() { ID = ID, Start = Start, End = End };
        }

        public PersistentActivity Convert(Activity entity)
        {
            ID = entity.ID;
            Start = entity.Start;
            End = entity.End;

            return this;
        }

        public bool CanAutoIncrement()
        {
            return true;
        }

        public long GetNewKey(long lastKey)
        {
            return ++lastKey;
        }
    }
}
