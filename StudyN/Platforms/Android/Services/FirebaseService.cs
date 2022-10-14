using Android.App;
using Android.Content;
using Firebase.Messaging;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Notification = FirebaseAdmin.Messaging.Notification;
using FirebaseMessaging = FirebaseAdmin.Messaging.FirebaseMessaging;
using System.Reflection;

namespace StudyN.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : FirebaseMessagingService
    {
        static readonly string _fileName = "StudyN.private_key.json";

        /// <summary>
        /// Service to send messages to Firebase Cloud Messaging service
        /// for push notifications
        /// </summary>
        public FirebaseService()
        {
            CreateFirebaseApp();
        }

        /// <summary>
        /// Push a message to FCM
        /// </summary>
        /// <param name="subject">The title of the message<IsRequired>true</IsRequired></param>
        /// <param name="description">The body of the message<IsRequired>false</IsRequired></param>
        /// <param name="data">A collection of keyvalue pairs to indicate how the application should handle notification<IsRequired>false</IsRequired></param>
        /// <returns></returns>
        public static bool Push(string subject, string description, IDictionary<string, string> data)
        {
            try
            {
                // this registration token comes from the client FCM SDKs
                // and is stored in the application's preferences.
                var registrationToken = Preferences.Get("DeviceToken", "");

                var message = new Message
                {
                    Token = registrationToken,
                    Notification = new Notification
                    {
                        Body = description,
                        Title = subject
                    }
                };

                if (data != null && data.Any())
                {
                    message.Data = (IReadOnlyDictionary<string, string>)data;
                }

                // Send a message to the device corresponding to the provided 
                // registration token.
                var response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Successfully sent message: {response}");// TODO: Remove from DEBUG if, and add to logging
#endif
                return response != null;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);// TODO: Remove from DEBUG if, and add to logging
                return false;
            }
        }

        /// <summary>
        /// Set new registration token in application preferences
        /// </summary>
        /// <param name="token">this registration token comes from the client FCM SDKs.<IsRequired>true</IsRequired></param>
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            if (Preferences.ContainsKey("DeviceToken"))
            {
                Preferences.Remove("DeviceToken");
            }
            Preferences.Set("DeviceToken", token);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Token: {token}");
#endif
        }

        /// <summary>
        /// Create default FirebaseApp
        /// </summary>
        private static void CreateFirebaseApp()
        {
            try
            {
                // Read from embedded resource file to obtain credentials to pass to 
                // FirebaseApp
                var assembly = Assembly.GetExecutingAssembly();
                using Stream stream = assembly.GetManifestResourceStream(_fileName);
                using StreamReader reader = new(stream);
                var result = reader.ReadToEnd();


                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(result)
                });
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);// TODO: Remove from DEBUG if, and add to logging
            }
        }
    }
}
