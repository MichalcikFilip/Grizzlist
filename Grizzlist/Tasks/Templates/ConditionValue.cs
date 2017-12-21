using System;

namespace Grizzlist.Tasks.Templates
{
    public class ConditionValue : Condition
    {
        public ConditionValueType Type { get; set; }
        public short Value { get; set; }

        public override bool Satisfies(DateTime date)
        {
            switch (Type)
            {
                case ConditionValueType.DayInWeek:
                    return (short)date.DayOfWeek == (Value != 7 ? Value : 0);
                case ConditionValueType.DayInMonth:
                    DateTime nextDay = date.AddDays(1);
                    if (nextDay.Month > date.Month && Value > date.Day)
                        return true;
                    return date.Day == Value;
                case  ConditionValueType.MonthInYear:
                    return date.Month == Value;
                default:
                    return false;
            }
        }

        public override bool Created(DateTime date, DateTime last)
        {
            if (last == default(DateTime)) return false;

            switch (Type)
            {
                case ConditionValueType.DayInWeek:
                    return date.DayOfWeek == last.DayOfWeek && date.Day == date.Day && date.Month == last.Month && date.Year == last.Year;
                case ConditionValueType.DayInMonth:
                    return date.Day == date.Day && date.Month == last.Month && date.Year == last.Year;
                case ConditionValueType.MonthInYear:
                    return date.Month == last.Month && date.Year == last.Year;
                default:
                    return false;
            }
        }
    }
}
