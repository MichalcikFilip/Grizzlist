using System;

namespace Grizzlist.Client.Tasks.Search.Comparers
{
    class GreaterThan : IComparer
    {
        public bool Compare<T>(T value, T other)
        {
            if (value is IComparable<T>)
                return ((IComparable<T>)value).CompareTo(other) < 0;

            return false;
        }
    }
}
