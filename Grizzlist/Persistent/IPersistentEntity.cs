namespace Grizzlist.Persistent
{
    public interface IPersistentEntity<T>
    {
        T ID { get; set; }
    }
}
