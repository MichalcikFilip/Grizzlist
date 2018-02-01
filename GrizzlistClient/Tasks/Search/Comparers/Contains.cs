namespace Grizzlist.Client.Tasks.Search.Comparers
{
    class Contains : IComparer
    {
        public bool Compare<T>(T value, T other)
        {
            if (other is string)
                return (other as string).Contains(value as string);

            return false;
        }
    }
}
