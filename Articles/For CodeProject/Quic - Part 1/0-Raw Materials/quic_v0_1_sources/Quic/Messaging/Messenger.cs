using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quic.Messaging
{
    public static class Messenger
    {
        static List<INotification> notifications = new List<INotification>();

        public static void Prompt(string message, MessageType messageType)
        {
            if (prompted == null)
                prompted = new MessengerEventHandler(messageHandler);
            prompted(null, new MessengerEventArgs(message, messageType));
        }
        public static void Notify(INotification notification)
        {
            notifications.Add(notification);

            if (notified == null)
                notified = new NotifyEventHandler(notifHandler);
            notified(null, new NotifyEventArgs(notification));
        }
        public static void ClearNotifications()
        {
            notifications.Clear();

            if (notifCleared == null)
                notifCleared = new EventHandler(eventHandler);
            notifCleared(null, new EventArgs());
        }

        static void messageHandler(object sender, MessengerEventArgs e) { }
        static void notifHandler(object sender, NotifyEventArgs e) { }
        static void eventHandler(object sender, EventArgs e) { }

        public static INotification[] Notifications { get { return notifications.ToArray(); } }

        static event MessengerEventHandler prompted;
        public static event MessengerEventHandler Prompted
        {
            add
            {
                if (prompted == null)
                    prompted = new MessengerEventHandler(messageHandler);
                prompted += value;
            }
            remove
            {
                if (prompted == null)
                    prompted = new MessengerEventHandler(messageHandler);
                prompted -= value;
            }
        }

        static event NotifyEventHandler notified;
        public static event NotifyEventHandler Notified
        {
            add
            {
                if (notified == null)
                    notified = new NotifyEventHandler(notifHandler);
                notified += value;
            }
            remove
            {
                if (notified == null)
                    notified = new NotifyEventHandler(notifHandler);
                notified -= value;
            }
        }

        static event EventHandler notifCleared;
        public static event EventHandler NotificationsCleared
        {
            add
            {
                if (notifCleared == null)
                    notifCleared = new EventHandler(eventHandler);
                notifCleared += value;
            }
            remove
            {
                if (notifCleared == null)
                    notifCleared = new EventHandler(eventHandler);
                notifCleared -= value;
            }
        }

        public delegate void MessengerEventHandler(object sender, MessengerEventArgs e);
        public class MessengerEventArgs : EventArgs
        {
            public MessengerEventArgs(string message, MessageType messageType)
            {
                Message = message;
                MessageType = messageType;
            }

            /// <summary>
            /// Gets the message
            /// </summary>
            public string Message { get; private set; }
            /// <summary>
            /// Gets the message type
            /// </summary>
            public MessageType MessageType { get; private set; }
        }

        public delegate void NotifyEventHandler(object sender, NotifyEventArgs e);
        public class NotifyEventArgs : EventArgs
        {
            public NotifyEventArgs(INotification notification)
            {
                Notification = notification;
            }

            /// <summary>
            /// Gets the exception
            /// </summary>
            public INotification Notification { get; private set; }
        }
    }
}
