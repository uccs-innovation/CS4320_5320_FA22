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
        public NotificationService()
        {
        }

        public void OnStart()
        {
            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            _timer.Enabled = true;
        }

        protected void OnStop()
        {
            _timer.Enabled = false;
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            var appts = DataAccess.GetData();
#if DEBUG
            _counter++;
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now} --> Poller Get #{_counter}");
            System.Diagnostics.Debug.WriteLine($"{appts.Count()} Appointments found");
#endif

            foreach (var appt in appts)
            {
                if(appt.Subject.Equals("Test Note"))
                {
                    System.Diagnostics.Debug.WriteLine($"Appointment found for : {appt.Subject}");
                    FirebaseService.Push(appt.Subject, appt.Description, null);
                }
                if (appt.Reminders.Any() && CheckTime(appt))
                {
                    System.Diagnostics.Debug.WriteLine($"Reminder found for : {appt.Subject}");
                }
            }
        }

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

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}
