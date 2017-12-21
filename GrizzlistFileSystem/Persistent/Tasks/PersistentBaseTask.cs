using Grizzlist.FileSystem.Persistent.Tasks.Types;
using System;
using System.Collections.Generic;

namespace Grizzlist.FileSystem.Persistent.Tasks
{
    [Serializable]
    abstract class PersistentBaseTask : LongIncrementalKey
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Closed { get; set; }
        public List<PersistentActivity> Activities { get; set; }
    }
}
