using Grizzlist.Persistent;
using System.Drawing;

namespace Grizzlist.Tasks.Types
{
    public struct Drawing : IPersistentEntity<long>
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Bitmap Image { get; set; }
    }
}
