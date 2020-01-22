using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Notification
{
    public class NotificationSettings
    {
        public enum NotificationType
        {
            FCM = 1,
            APPLE = 2,
            WND = 3
        }
        public static class NotificationTemplate
        {
            public static string FcmNotificationContent = "{\"data\":{\"message\": \"{0}\" }}";
            public static string AppleNotificationContent = "{\"aps\":{\"alert\":\"{0}\"}}";
            public static string WnsNotification = "<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"{0}\">{1}</text></binding></visual></toast>";
        }
        public static class NotificationMessage
        {
            public static string SetNotification(NotificationType type, string message, int? windowsNotificationId = null)
            {
                switch (type)
                {
                    case NotificationType.FCM:
                        return NotificationTemplate.FcmNotificationContent.Replace("{0}", message);
                    case NotificationType.APPLE:
                        return NotificationTemplate.AppleNotificationContent.Replace("{0}", message);
                    case NotificationType.WND:
                        return NotificationTemplate.WnsNotification.Replace("{0}", (windowsNotificationId ?? 1).ToString()).Replace("{1}", message);
                    default:
                        return NotificationTemplate.FcmNotificationContent.Replace("{0}", message);
                }
            }
        }
    }
}
