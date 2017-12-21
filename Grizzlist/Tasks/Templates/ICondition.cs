using System;

namespace Grizzlist.Tasks.Templates
{
    public interface ICondition
    {
        bool Satisfies(DateTime date);
        bool Created(DateTime date, DateTime last);
    }
}
