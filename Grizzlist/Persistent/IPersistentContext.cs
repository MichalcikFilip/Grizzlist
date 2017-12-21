namespace Grizzlist.Persistent
{
    public interface IPersistentContext
    {
        IRepository<T, K> GetRepository<T, K>() where T : IPersistentEntity<K>;
    }
}
