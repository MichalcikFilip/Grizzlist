using Grizzlist.Persistent;

namespace Grizzlist.FileSystem.Persistent
{
    public interface IConvertible<T, D, K> where T : IPersistentEntity<K> where D : IPersistentEntity<K>
    {
        T Convert();
        D Convert(T entity);
    }
}
