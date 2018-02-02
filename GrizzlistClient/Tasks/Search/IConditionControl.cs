using System;

namespace Grizzlist.Client.Tasks.Search
{
    interface IConditionControl
    {
        event Action RemoveClicked;

        ICondition GetCondition();
    }
}
