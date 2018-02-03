using Grizzlist.Logger;
using System.Collections;
using System.Collections.Generic;

namespace Grizzlist.Client.BackgroundActions
{
    sealed class ActionsCollection : IEnumerable<IAction>, IEnumerable
    {
        private static object singleton = new object();
        private static volatile ActionsCollection instance = null;

        public static ActionsCollection Instance
        {
            get
            {
                if (instance == null)
                    lock (singleton)
                        if (instance == null)
                            instance = new ActionsCollection();

                return instance;
            }
        }

        private ActionsCollection()
        {
            Actions = new List<IAction>();
        }

        public List<IAction> Actions { get; private set; }

        public void Add(IAction action)
        {
            if (action != null)
            {
                Actions.Add(action);

                Log.Debug($"Background action {action.GetType().Name} was added", this);
            }
        }

        public void Remove(IAction action)
        {
            if (action != null)
            {
                Actions.Remove(action);

                Log.Debug($"Background action {action.GetType().Name} was removed", this);
            }
        }

        public IEnumerator<IAction> GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Actions.GetEnumerator();
        }
    }
}
