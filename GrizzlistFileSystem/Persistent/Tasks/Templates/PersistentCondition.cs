using Grizzlist.Tasks.Templates;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Templates
{
    [Serializable]
    abstract class PersistentCondition : LongIncrementalKey, IConvertible<Condition, PersistentCondition, long>
    {
        public abstract Condition Convert();
        public abstract PersistentCondition Convert(Condition entity);
    }
}
