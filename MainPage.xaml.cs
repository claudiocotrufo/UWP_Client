using System;
using System.Collections.Generic;
using System.Web.Http;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static App1.Notification.NotificationSettings;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<NotificationType> notificationTypeSelected = new List<NotificationType>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        public HttpConfiguration Configuration { get; private set; }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            HUB.InitNotificationsAsync();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            HUB.DeleteRegistration(id_registration.Text);
        }

        private void NotificationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((e.AddedItems[0] as ComboBoxItem).Content)
            {
                case "All":
                    notificationTypeSelected = new List<NotificationType> {
                    Notification.NotificationSettings.NotificationType.FCM,
                    Notification.NotificationSettings.NotificationType.WND
                    };
                    break;
                case "Android":
                    notificationTypeSelected = new List<NotificationType> {
                    Notification.NotificationSettings.NotificationType.FCM
                    };
                    break;
                case "Windows":
                    notificationTypeSelected = new List<NotificationType> {
                    Notification.NotificationSettings.NotificationType.WND
                    };
                    break;
                default:
                    notificationTypeSelected = new List<NotificationType> {
                    Notification.NotificationSettings.NotificationType.FCM,
                    Notification.NotificationSettings.NotificationType.WND
                    };
                    break;
            }
        }

        private async void Button_clickAsync(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(message.Text) && !string.IsNullOrWhiteSpace(message.Text))
            {
                HUB.SendNotification(message.Text, notificationTypeSelected);
            }
            else
            {
                var dialog = new MessageDialog("message can't be emplty");
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}
