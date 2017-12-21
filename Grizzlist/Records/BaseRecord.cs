using Grizzlist.Persistent;

namespace Grizzlist.Records
{
    public abstract class BaseRecord : PersistentEntity<long>
    {
        public string Name { get; set; }
        public bool Removed { get; set; }
    }
}
