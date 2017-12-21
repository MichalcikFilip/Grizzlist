using System;

namespace Grizzlist.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetDate(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
