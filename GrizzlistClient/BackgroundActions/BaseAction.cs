using Grizzlist.Logger;
using System;

namespace Grizzlist.Client.BackgroundActions
{
    abstract class BaseAction : IAction
    {
        private DateTime lastRun;

        public double Interval { get; private set; }

        public BaseAction(double interval)
        {
            Interval = interval;
        }

        public bool CanRun()
        {
            return lastRun.AddSeconds(Interval) < DateTime.Now;
        }

        public void Run()
        {
            Log.Debug("Background action started", this);

            lastRun = DateTime.Now;
            RunAction();
        }

        protected abstract void RunAction();
    }
}
