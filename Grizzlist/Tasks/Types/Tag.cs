using Grizzlist.Persistent;

namespace Grizzlist.Tasks.Types
{
    public struct Tag : IPersistentEntity<long>
    {
        public long ID { get; set; }
        public string Value { get; set; }
    }
}
