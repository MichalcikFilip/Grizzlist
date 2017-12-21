using Grizzlist.FileSystem.Persistent.Tasks.Types;
using Grizzlist.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.FileSystem.Persistent.Tasks
{
    [Serializable]
    class PersistentTask : PersistentBaseTask, IConvertible<Task, PersistentTask, long>
    {
        public string Note { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskState State { get; set; }
        public DateTime Deadline { get; set; }
        public List<PersistentTag> Tags { get; set; }
        public List<PersistentAttachment> Attachments { get; set; }
        public List<PersistentDrawing> Drawings { get; set; }
        public List<PersistentSubTask> SubTasks { get; set; }

        public Task Convert()
        {
            Task task = new Task(Name, Description, Note, Priority, State, Deadline) { ID = ID, Created = Created, Updated = Updated, Closed = Closed };

            task.Activities.AddRange(Activities.Select(x => x.Convert()));
            task.Tags.AddRange(Tags.Select(x => x.Convert()));
            task.Attachments.AddRange(Attachments.Select(x => x.Convert()));
            task.Drawings.AddRange(Drawings.Select(x => x.Convert()));
            task.SubTasks.AddRange(SubTasks.Select(x => x.Convert()));

            return task;
        }

        public PersistentTask Convert(Task entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Name = entity.Name;
                Description = entity.Description;
                Created = entity.Created;
                Updated = entity.Updated;
                Closed = entity.Closed;
                Note = entity.Note;
                Priority = entity.Priority;
                State = entity.State;
                Deadline = entity.Deadline;
                Activities = entity.Activities.Select(x => new PersistentActivity().Convert(x)).ToList();
                Tags = entity.Tags.Select(x => new PersistentTag().Convert(x)).ToList();
                Attachments = entity.Attachments.Select(x => new PersistentAttachment().Convert(x)).ToList();
                Drawings = entity.Drawings.Select(x => new PersistentDrawing().Convert(x)).ToList();
                SubTasks = entity.SubTasks.Select(x => new PersistentSubTask().Convert(x)).ToList();
            }

            return this;
        }
    }
}
