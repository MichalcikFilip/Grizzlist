using Grizzlist.Notifications;
using System;

namespace Grizzlist.FileSystem.Persistent.Notifications
{
    [Serializable]
    class PersistentNotification : LongIncrementalKey, IConvertible<Notification, PersistentNotification, long>
    {
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
        public string[] Parameters { get; set; }

        public Notification Convert()
        {
            return new Notification(Type, Parameters) { ID = ID, Created = Created };
        }

        public PersistentNotification Convert(Notification entity)
        {
            if (entity != null)
            {
                ID = entity.ID;
                Created = entity.Created;
                Type = entity.Type;
                Parameters = entity.Parameters;
            }

            return this;
        }
    }
}
