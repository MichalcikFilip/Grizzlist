using Grizzlist.Persistent;
using System;

namespace Grizzlist.FileSystem.Persistent
{
    [Serializable]
    abstract class PersistentEntity<T> : IPersistentEntity<T>
    {
        public T ID { get; set; }
    }
}
