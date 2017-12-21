namespace Grizzlist.FileSystem.Serialization
{
    public abstract class BaseSerializer<T> : ISerializer<T>
    {
        public string FilePath { get; private set; }

        public BaseSerializer(string filePath)
        {
            FilePath = filePath;
        }

        public abstract T Deserialize();
        public abstract bool Serialize(T entity);
    }
}
