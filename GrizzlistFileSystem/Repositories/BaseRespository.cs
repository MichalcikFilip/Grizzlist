using Grizzlist.FileSystem.Serialization;

namespace Grizzlist.FileSystem.Repositories
{
    public abstract class BaseRespository<T>
    {
        protected ISerializer<T> serializer = null;

        public BaseRespository(ISerializer<T> serializer)
        {
            this.serializer = serializer;
        }
    }
}
