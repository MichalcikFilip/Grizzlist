namespace Grizzlist.FileSystem.Persistent
{
    public interface IIncrementalKey<T>
    {
        bool CanAutoIncrement();
        T GetNewKey(T lastKey);
    }
}
