namespace Grizzlist.FileSystem.Serialization
{
    public interface ISerializer<T>
    {
        bool Serialize(T entity);
        T Deserialize();
    }
}
