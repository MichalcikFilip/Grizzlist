using Grizzlist.Persistent;
using Grizzlist.Tasks.Types;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Types
{
    [Serializable]
    struct PersistentTag : IPersistentEntity<long>, IConvertible<Tag, PersistentTag, long>, IIncrementalKey<long>
    {
        public long ID { get; set; }
        public string Value { get; set; }

        public Tag Convert()
        {
            return new Tag() { ID = ID, Value = Value };
        }

        public PersistentTag Convert(Tag entity)
        {
            ID = entity.ID;
            Value = entity.Value;

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
