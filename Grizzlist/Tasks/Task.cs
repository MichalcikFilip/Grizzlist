using Grizzlist.Tasks.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Tasks
{
    public class Task : BaseTask
    {
        private List<Tag> tags = new List<Tag>();
        private List<Attachment> attachments = new List<Attachment>();
        private List<Drawing> drawings = new List<Drawing>();
        private List<SubTask> subTasks = new List<SubTask>();

        public string Note { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskState State { get; set; }
        public DateTime Deadline { get; set; }
        public List<Tag> Tags { get { return tags; } }
        public List<Attachment> Attachments { get { return attachments; } }
        public List<Drawing> Drawings { get { return drawings; } }
        public List<SubTask> SubTasks { get { return subTasks; } }

        public Task()
            : base()
        {
            Priority = TaskPriority.Normal;
            State = TaskState.Open;
        }

        public Task(string name, string description, string note, TaskPriority priority, TaskState state, DateTime deadline)
            : base(name, description)
        {
            Note = note;
            Priority = priority;
            State = state;
            Deadline = deadline;
        }

        public void Close()
        {
            foreach (SubTask subtask in subTasks.Where(x => !x.Completed))
                subtask.Close();

            CloseActivity();
            State = TaskState.Closed;
            Closed = DateTime.Now;
        }

        public static Task FromCopy(Task task)
        {
            Task newTask = new Task(task.Name, task.Description, task.Note, task.Priority, task.State, task.Deadline);

            newTask.Created = task.Created;
            newTask.Updated = task.Updated;
            newTask.Closed = task.Closed;
            newTask.Activities.AddRange(task.Activities);
            newTask.Tags.AddRange(task.Tags);
            newTask.Attachments.AddRange(task.Attachments);
            newTask.Drawings.AddRange(task.Drawings);
            newTask.SubTasks.AddRange(task.SubTasks.Select(x => SubTask.FromCopy(x)));

            return newTask;
        }
    }
}
