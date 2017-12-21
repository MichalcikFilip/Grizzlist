using System;

namespace Grizzlist.FileSystem.Persistent
{
    [Serializable]
    abstract class LongIncrementalKey : PersistentEntity<long>, IIncrementalKey<long>
    {
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
