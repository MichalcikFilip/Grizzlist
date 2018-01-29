using Grizzlist.Tasks.Templates;
using System;

namespace Grizzlist.Client.Tasks.Templates
{
    interface IConditionControl
    {
        event Action RemoveClicked;

        ICondition GetCondition();
    }
}
