using Android.App;
using Android.Content;
using Android.OS;
using DevExpress.Maui.Scheduler;
using StudyN.Common;
using StudyN.Models;
using StudyN.Platforms.Android.Services;
using System.Timers;

namespace StudyN.NotificationService
{
    public class NotificationService : Service
    {
        private int _counter;
        private System.Timers.Timer _timer;

        /// <summary>
        /// 
        /// </summary>
        public NotificationService()
        {
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnStart()
        {
            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            _timer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void OnStop()
        {
            _timer.Enabled = false;
        }

        /// <summary>
        /// Time tick that uses static class DataAccess to gather information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            var appts = DataAccess.GetData();
#if DEBUG
            // TODO: Remove from DEBUG if, and add to logging  
            _counter++;
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now} --> Poller Get #{_counter}");
            System.Diagnostics.Debug.WriteLine($"{appts.Count()} Appointments found");
#endif

            foreach (var appt in appts)
            {
#if DEBUG
                if (appt.Subject.Equals("Test Note"))
                {
                    System.Diagnostics.Debug.WriteLine($"Appointment found for : {appt.Subject}");
                    var sent = FirebaseService.Push(appt.Subject, appt.Description ?? "This is a test of the push notification service.", null);
                    if (sent)
                    {
                        DataAccess.DeleteItem(appt); //Remove event from notification store
                    }
                }
#else
                if (appt.Reminders.Any() && CheckTime(appt))
                {
                    System.Diagnostics.Debug.WriteLine($"Reminder found for : {appt.Subject}");// TODO: Remove from DEBUG if, and add to logging
                    var sent = FirebaseService.Push(appt.Subject, appt.Description, null);
                    if (sent)
                    {
                        DataAccess.DeleteItem(appt);
                    }
                }
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appt"></param>
        /// <returns></returns>
        private static bool CheckTime(AppointmentItem appt)
        {
            var reminders = false;
            foreach (var reminder in appt.Reminders)
            {
                if (reminder.Minutes > 0)
                {
                    if (DateTime.Now.Date.Equals(appt.Start.Date) && DateTime.Now.Hour.Equals(appt.Start.Hour) && DateTime.Now.AddMinutes(reminder.Minutes).Minute.Equals(appt.Start.Minute))
                    {
                        reminders = true;
                    }
                }
                else if (reminder.Hours > 0)
                {
                    if (DateTime.Now.Date.Equals(appt.Start.Date) && DateTime.Now.AddHours(reminder.Hours).Hour.Equals(appt.Start.Hour))
                    {
                        reminders = true;
                    }
                }
                else if (reminder.Days > 0)
                {
                    if (DateTime.Now.AddDays(reminder.Days).Day.Equals(appt.Start.Day))
                    {
                        reminders = true;
                    }
                }
            }
            return reminders;
        }
    }
}
