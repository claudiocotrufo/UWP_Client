﻿using App1.HUB_Settings;
using Microsoft.Azure.NotificationHubs;
using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static App1.Notification.NotificationSettings;

namespace App1
{
    public static class HUB
    {
        public static async void InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            NotificationHub hub = new NotificationHub(Setting.HUB_Name, Setting.HUB_Path);
            var result = await hub.RegisterNativeAsync(channel.Uri);

            // Displays the registration ID so you know it was successful
            if (result.RegistrationId != null)
            {
                var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
                dialog.Commands.Add(new UICommand("OK"));

                ((Window.Current.Content as Frame).Content as MainPage)
                    .id_registration.Text = result.RegistrationId;
                await dialog.ShowAsync();
            }
        }

        public static async void SendNotification(string message, List<NotificationType> types)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(Setting.HUB_Path, Setting.HUB_Name);

            foreach (var t in types)
            {
                var notification =
                   NotificationMessage.SetNotification(t, message);

                switch (t)
                {
                    case NotificationType.FCM:
                        await hub.SendFcmNativeNotificationAsync(notification);
                        break;
                    case NotificationType.WND:
                        await hub.SendWindowsNativeNotificationAsync(notification);
                        break;
                    default:
                        await hub.SendFcmNativeNotificationAsync(notification);
                        break;
                }
            }
        }

        public static async void DeleteRegistration(string id)
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(Setting.HUB_Path, Setting.HUB_Name);

            await hub.DeleteRegistrationAsync(id);
            ((Window.Current.Content as Frame).Content as MainPage)
                 .id_registration.Text = "no registration";

        }
    }

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            HUB.InitNotificationsAsync();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
