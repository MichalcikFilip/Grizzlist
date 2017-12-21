using Grizzlist.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.FileSystem.Persistent.Records
{
    [Serializable]
    class PersistentGroup<T, D> : LongIncrementalKey, IConvertible<Group<T>, PersistentGroup<T, D>, long> where T : BaseRecord where D : PersistentBaseRecord, IConvertible<T, D, long>, new ()
    {
        public string Name { get; set; }
        public List<D> Records { get; set; }

        public Group<T> Convert()
        {
            Group<T> group = new Group<T>() { ID = ID, Name = Name };

            group.Records.AddRange(Records.Select(x => x.Convert()));

            return group;
        }

        public PersistentGroup<T, D> Convert(Group<T> entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Records = entity.Records.Select(x => new D().Convert(x)).ToList();
            }

            return this;
        }
    }
}
