using Android.App;
using Plugin.FirebasePushNotification;
using StudyN.Views;
using StudyN.Utilities;
using Application = Microsoft.Maui.Controls.Application;

namespace StudyN
{
    public partial class App : Application
    {
        //private string _deviceToken;
        private NotificationService.NotificationService _notificaitonService;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            // Set the EventBus to asyncronously wait for task events
            Task.Run(async () => await EventBus.WaitForStudynEvent());

            // This will subscribe to event bus and live on that way
            new FileManager();

            DependencyService.Register<NotificationService.NotificationService>();
            Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
            Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(TaskPage), typeof(TaskPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
            Preferences.Set("DeviceToken", e.Token);
        }
    }
}
