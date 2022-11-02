using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;
using Plugin.FirebasePushNotification;

namespace StudyN
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            Platform.Init(this, savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Platform.OnResume(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intent"></param>
        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
            Platform.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}