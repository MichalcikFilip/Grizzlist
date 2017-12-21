using Grizzlist.Persistent;
using System;

namespace Grizzlist.Tasks.Templates
{
    public abstract class Condition : PersistentEntity<long>, ICondition
    {
        public abstract bool Created(DateTime date, DateTime last);
        public abstract bool Satisfies(DateTime date);
    }
}
