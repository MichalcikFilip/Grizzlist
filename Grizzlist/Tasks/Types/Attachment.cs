using Grizzlist.Persistent;

namespace Grizzlist.Tasks.Types
{
    public struct Attachment : IPersistentEntity<long>
    {
        public long ID { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
    }
}
