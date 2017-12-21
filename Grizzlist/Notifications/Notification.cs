using Grizzlist.Persistent;
using System;

namespace Grizzlist.Notifications
{
    public class Notification : PersistentEntity<long>
    {
        public DateTime Created { get; set; }
        public NotificationType Type { get; set; }
        public string[] Parameters { get; set; }

        public Notification(NotificationType type, params string[] parameters)
        {
            Created = DateTime.Now;
            Type = type;
            Parameters = parameters;
        }

        public string FillMessage(string messsageTemplate)
        {
            return string.Format(messsageTemplate, Parameters);
        }
    }
}
