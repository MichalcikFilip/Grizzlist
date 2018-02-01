namespace Grizzlist.Client.Tasks.Search.Comparers
{
    class Equals : IComparer
    {
        public bool Compare<T>(T value, T other)
        {
            return value.Equals(other);
        }
    }
}
