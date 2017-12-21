namespace Grizzlist.Persistent
{
    public abstract class PersistentEntity<T> : IPersistentEntity<T>
    {
        public T ID { get; set; }
    }
}
