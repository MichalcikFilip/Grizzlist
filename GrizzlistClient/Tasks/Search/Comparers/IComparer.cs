namespace Grizzlist.Client.Tasks.Search.Comparers
{
    interface IComparer
    {
        bool Compare<T>(T value, T other);
    }
}
