﻿using Grizzlist.Client.Persistent;
using Grizzlist.Logger;
using Grizzlist.Notifications;
using Grizzlist.Persistent;
using System.Collections.Generic;
using System.Windows.Media;

namespace Grizzlist.Client.Notifications
{
    static class NotificationHelper
    {
        public static Dictionary<NotificationType, string> Messages { get; private set; }
        public static Dictionary<NotificationType, Color> Colors { get; private set; }

        static NotificationHelper()
        {
            Messages = new Dictionary<NotificationType, string>();
            Messages.Add(NotificationType.TaskCreated, "Task {0} was created");
            Messages.Add(NotificationType.TaskCreatedFromTemplate, "Task {0} was created from template");
            Messages.Add(NotificationType.TaskUpdated, "Task {0} was updated");
            Messages.Add(NotificationType.TaskOpened, "Task {0} was opened");
            Messages.Add(NotificationType.TaskDeferred, "Task {0} was postponed");
            Messages.Add(NotificationType.TaskClosed, "Task {0} was closed");
            Messages.Add(NotificationType.TaskArchived, "Task {0} was archived");
            Messages.Add(NotificationType.TaskRemoved, "Task {0} was removed");
            Messages.Add(NotificationType.TaskActivityStarted, "Activity on task {0} started");
            Messages.Add(NotificationType.TaskActivityStopped, "Activity on task {0} stopped");
            Messages.Add(NotificationType.TaskDeadlineEnds, "Task {0} deadline ends today");
            Messages.Add(NotificationType.TaskDeadlineExceeded, "Task {0} deadline exceeded");
            Messages.Add(NotificationType.RecordCreated, "Record {0} ({1}) was created");
            Messages.Add(NotificationType.RecordUpdated, "Record {0} ({1}) was updated");
            Messages.Add(NotificationType.RecordRemoved, "Record {0} ({1}) was removed");

            Colors = new Dictionary<NotificationType, Color>();
            Colors.Add(NotificationType.TaskCreated, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskCreatedFromTemplate, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskUpdated, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskOpened, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskDeferred, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskClosed, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskArchived, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskRemoved, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskActivityStarted, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskActivityStopped, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.TaskDeadlineEnds, Color.FromRgb(255, 218, 68));
            Colors.Add(NotificationType.TaskDeadlineExceeded, Color.FromRgb(216, 0, 39));
            Colors.Add(NotificationType.RecordCreated, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.RecordUpdated, Color.FromRgb(178, 221, 255));
            Colors.Add(NotificationType.RecordRemoved, Color.FromRgb(178, 221, 255));
        }

        public static void Notify(NotificationType type, params string[] parameters)
        {
            Notification notification = NotificationManager.Instance.Notify(type, parameters);

            using (IRepository<Notification, long> repository = PersistentFactory.GetContext().GetRepository<Notification, long>())
                repository.Add(notification);

            Log.Debug($"Notification {type} was created.", null);
        }
    }
}
