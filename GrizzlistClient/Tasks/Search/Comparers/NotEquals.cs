namespace Grizzlist.Client.Tasks.Search.Comparers
{
    class NotEquals : IComparer
    {
        public bool Compare<T>(T value, T other)
        {
            return !value.Equals(other);
        }
    }
}
