using Grizzlist.Client.Persistent;
using Grizzlist.Notifications;
using Grizzlist.Persistent;

namespace Grizzlist.Client.Notifications
{
    static class NotificationHelper
    {
        public static void Notify(NotificationType type, params string[] parameters)
        {
            Notification notification = NotificationManager.Instance.Notify(type, parameters);

            using (IRepository<Notification, long> repository = PersistentFactory.GetContext().GetRepository<Notification, long>())
                repository.Add(notification);
        }
    }
}
