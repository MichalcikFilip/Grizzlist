using Grizzlist.FileSystem;
using Grizzlist.Persistent;

namespace Grizzlist.Client.Persistent
{
    static class PersistentFactory
    {
        public static IPersistentContext GetContext()
        {
            return new FileSystemContext();
        }
    }
}
