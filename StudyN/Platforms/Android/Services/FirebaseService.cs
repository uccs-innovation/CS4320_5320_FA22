using Android.App;
using Android.Content;
using Firebase.Messaging;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Notification = FirebaseAdmin.Messaging.Notification;
using FirebaseMessaging = FirebaseAdmin.Messaging.FirebaseMessaging;

namespace StudyN.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : FirebaseMessagingService
    {
        const string PATH = "StudyN.private_key.json";
        //private const string FIREBASE_URL = @"https://fcm.googleapis.com/v1/projects/studyn-pushnotification/messages:send";
        //private const string PUBLIC_SERVER_KEY = @"=BK-Koy1xEam6DavLn1W4s3953gKe93jECus11BPiy_4MPynD30sBZTWVuuS3U4Mhd3CGbHh33Ml_ffSNyDqM20I";
        //private const string PUBLIC_SERVER_KEY = @"=BAR8oFBS50ueEeXpmxVbjzNEFpnabJX_vIgHAw0aDpVknS14yJs2AD8-XqIoDK5vYxETWvaXv9Lbvuk0kqWAgSE";

        public FirebaseService()
        {
        }

        public static void Push(string subject, string description, IDictionary<string, string> data)
        {
            try
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(PATH)
                });

                // this registration token comes from the client FCM SDKs.
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
                System.Diagnostics.Debug.WriteLine($"Successfully sent message: {response}");
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

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
    }
}
