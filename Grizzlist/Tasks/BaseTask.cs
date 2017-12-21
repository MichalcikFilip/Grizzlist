using Grizzlist.Persistent;
using Grizzlist.Tasks.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grizzlist.Tasks
{
    public abstract class BaseTask : PersistentEntity<long>
    {
        private List<Activity> activities = new List<Activity>();

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Closed { get; set; }
        public List<Activity> Activities { get { return activities; } }
        public bool IsActive { get { return Activities.Count > 0 && Activities.Last().IsActive; } }

        public BaseTask()
        {
            Created = Updated = DateTime.Now;
        }

        public BaseTask(string name, string description)
            : this()
        {
            Name = name;
            Description = description;
        }

        public void StartActivity()
        {
            if (!IsActive) Activities.Add(Activity.NewActivity());
        }

        public void CloseActivity()
        {
            if (IsActive) Activities[Activities.Count - 1] = Activities[Activities.Count - 1].Close();
        }
    }
}
