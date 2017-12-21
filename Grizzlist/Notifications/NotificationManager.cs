using System.Collections.Generic;

namespace Grizzlist.Notifications
{
    public sealed class NotificationManager
    {
        private static object singleton = new object();
        private static volatile NotificationManager instance = null;

        public static NotificationManager Instance
        {
            get
            {
                if (instance == null)
                    lock (singleton)
                        if (instance == null)
                            instance = new NotificationManager();

                return instance;
            }
        }

        private NotificationManager()
        { }

        private List<INotifiable> targets = new List<INotifiable>();

        public List<INotifiable> Targets { get { return targets; } }

        public Notification Notify(NotificationType type, params string[] parameters)
        {
            return Notify(new Notification(type, parameters));
        }

        public Notification Notify(Notification notification)
        {
            if (notification != null)
                foreach (INotifiable target in targets)
                    target?.Notify(notification);

            return notification;
        }
    }
}
