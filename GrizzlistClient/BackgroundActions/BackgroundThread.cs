using Grizzlist.Logger;
using System.Linq;
using System.Threading;

namespace Grizzlist.Client.BackgroundActions
{
    class BackgroundThread
    {
        private bool running = false;
        private ActionsCollection actions = ActionsCollection.Instance;

        public void Start()
        {
            if (!running)
            {
                running = true;
                new Thread(Run).Start();

                Log.Debug("Background thread started", this);
            }
        }

        public void Stop()
        {
            running = false;
        }

        private void Run()
        {
            while (running)
            {
                foreach (IAction action in actions.ToList())
                    if (action.CanRun())
                        action.Run();

                Thread.Sleep(500);
            }

            Log.Debug("Background thread stopped", this);
        }
    }
}
