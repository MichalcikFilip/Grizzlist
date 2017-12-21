using System;

namespace Grizzlist.FileSystem.Persistent.Records
{
    [Serializable]
    abstract class PersistentBaseRecord : LongIncrementalKey
    {
        public string Name { get; set; }
        public bool Removed { get; set; }
    }
}
