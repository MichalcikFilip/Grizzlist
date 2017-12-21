using Grizzlist.Tasks.Templates;
using System;

namespace Grizzlist.FileSystem.Persistent.Tasks.Templates
{
    [Serializable]
    class PersistentTemplate : LongIncrementalKey, IConvertible<Template, PersistentTemplate, long>
    {
        public DateTime LastCreated { get; set; }
        public PersistentTask Task { get; set; }
        public PersistentCondition Condition { get; set; }
        public int DaysToDeadline { get; set; }

        public Template Convert()
        {
            return new Template(LastCreated) { ID = ID, Task = Task.Convert(), Condition = Condition.Convert(), DaysToDeadline = DaysToDeadline };
        }

        public PersistentTemplate Convert(Template entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                LastCreated = entity.LastCreated;
                Task = new PersistentTask().Convert(entity.Task);
                Condition = entity.Condition is ConditionOperator ? new PersistentConditionOperator().Convert((Condition)entity.Condition) : new PersistentConditionValue().Convert((Condition)entity.Condition);
                DaysToDeadline = entity.DaysToDeadline;
            }

            return this;
        }
    }
}
