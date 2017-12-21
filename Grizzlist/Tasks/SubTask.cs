using System;

namespace Grizzlist.Tasks
{
    public class SubTask : BaseTask
    {
        public bool Completed { get; set; }

        public SubTask()
            : base()
        {
            Completed = false;
        }

        public SubTask(string name, string description)
            : base(name, description)
        {
            Completed = false;
        }

        public void Close()
        {
            CloseActivity();
            Completed = true;
            Closed = DateTime.Now;
        }

        public static SubTask FromCopy(SubTask subTask)
        {
            SubTask newSubTask = new SubTask(subTask.Name, subTask.Description);

            newSubTask.Created = subTask.Created;
            newSubTask.Updated = subTask.Updated;
            newSubTask.Closed = subTask.Closed;

            return newSubTask;
        }
    }
}
