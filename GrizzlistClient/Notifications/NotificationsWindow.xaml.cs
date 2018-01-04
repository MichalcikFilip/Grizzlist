using Grizzlist.Client.Extensions;
using Grizzlist.Client.Persistent;
using Grizzlist.Notifications;
using Grizzlist.Persistent;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Grizzlist.Client.Notifications
{
    /// <summary>
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window, INotifiable
    {
        public const int VISIBLE_NOTIFICATIONS_NUMBER = 20;
        public const byte NOTIFICATION_BACKGROUND_ALPHA = 25;

        public NotificationsWindow(Window owner)
        {
            Owner = owner;

            InitializeComponent();

            NotificationManager.Instance.Targets.Add(this);
            Closing += (sender, e) => NotificationManager.Instance.Targets.Remove(this);

            using (IRepository<Notification, long> repository = PersistentFactory.GetContext().GetRepository<Notification, long>())
                foreach (Notification notification in repository.GetAll().OrderByDescending(x => x.Created).Take(VISIBLE_NOTIFICATIONS_NUMBER).Reverse())
                    AddNotification(notification);
        }

        public void Notify(Notification notification)
        {
            Dispatcher.Invoke(() => AddNotification(notification));
        }

        private void AddNotification(Notification notification)
        {
            pnlNotifications.Children.Insert(0, new Border() { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(NotificationHelper.Colors[notification.Type]), Background = new SolidColorBrush(NotificationHelper.Colors[notification.Type].SetAlpha(NOTIFICATION_BACKGROUND_ALPHA)), Margin = new Thickness(10, 5, 10, 5), Padding = new Thickness(6), Child = new TextBlock() { TextWrapping = TextWrapping.Wrap, Text = $"{notification.Created}{Environment.NewLine}{notification.FillMessage(NotificationHelper.Messages[notification.Type])}" } });
        }
    }
}
