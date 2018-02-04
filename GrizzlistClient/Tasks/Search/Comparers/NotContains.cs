namespace Grizzlist.Client.Tasks.Search.Comparers
{
    class NotContains : IComparer
    {
        public bool Compare<T>(T value, T other)
        {
            if (other is string)
                return !(other as string).ToLower().Contains((value as string).ToLower());

            return false;
        }
    }
}
