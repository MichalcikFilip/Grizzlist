using Grizzlist.FileSystem.Persistent.Tasks.Types;
using Grizzlist.Tasks;
using System;
using System.Linq;

namespace Grizzlist.FileSystem.Persistent.Tasks
{
    [Serializable]
    class PersistentSubTask : PersistentBaseTask, IConvertible<SubTask, PersistentSubTask, long>
    {
        public bool Completed { get; set; }

        public SubTask Convert()
        {
            SubTask subTask = new SubTask() { ID = ID, Name = Name, Description = Description, Created = Created, Updated = Updated, Closed = Closed, Completed = Completed };

            subTask.Activities.AddRange(Activities.Select(x => x.Convert()));

            return subTask;
        }

        public PersistentSubTask Convert(SubTask entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Description = entity.Description;
                Created = entity.Created;
                Updated = entity.Updated;
                Closed = entity.Closed;
                Completed = entity.Completed;
                Activities = entity.Activities.Select(x => new PersistentActivity().Convert(x)).ToList();
            }

            return this;
        }
    }
}
