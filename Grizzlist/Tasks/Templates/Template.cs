using Grizzlist.Extensions;
using Grizzlist.Persistent;
using System;

namespace Grizzlist.Tasks.Templates
{
    public class Template : PersistentEntity<long>
    {
        private DateTime lastCreated;

        public DateTime LastCreated { get { return lastCreated; } }
        public Task Task { get; set; }
        public ICondition Condition { get; set; }
        public int DaysToDeadline { get; set; }

        public Template()
            : this(default(DateTime))
        { }

        public Template(DateTime lastCreated)
        {
            this.lastCreated = lastCreated;
        }

        public bool SatisfiesCondition()
        {
            return SatisfiesCondition(DateTime.Now);
        }

        public bool SatisfiesCondition(DateTime date)
        {
            if (Condition != null && date > lastCreated)
                return Condition.Satisfies(date) && !Condition.Created(date, lastCreated);
            return false;
        }

        public Task CreateTask()
        {
            return CreateTask(DateTime.Now);
        }

        public Task CreateTask(DateTime date)
        {
            if (date > lastCreated)
            {
                lastCreated = date;

                Task task = Task.FromCopy(Task);

                task.Created = task.Updated = date;
                task.Deadline = date.GetDate().AddDays(DaysToDeadline);
                task.State = TaskState.Open;

                return task;
            }

            return null;
        }
    }
}
