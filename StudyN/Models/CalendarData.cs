using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace StudyN.Models
{
    public class UserEvents
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }
        public int LabelId { get; set; }
        public string Location { get; set; }
    }

    public class UserEventsType
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Color Color { get; set; }
    }

    public class AppData
    {
        public static DateTime BaseDate = DateTime.Today;

        public static string[] AppointmentTypes = { "Free Time", "Office Hours", "Class", "Work", "Bed Time" };
        public static Color[] AppointmentTypeColors = { Color.FromHex("#dfcfe9"), Color.FromHex("#c2f49d"),
                                                        Color.FromHex("#8de8df"), Color.FromHex("#a8d5ff"),
                                                        Color.FromHex("#c8f4ff") };

        //Calls to the things that we are going to do
        public static string[] User = { "Soccer ", "Math Class", "CS Class",
                                                "going out to hike", "Sleeping", "English",
                                                "Professor Office", "Going to work", "Going to conert",
                                                "Homework", "Project", "GYM",
                                                "Going to get Food"};

        static Random rnd = new Random();

        void CreateUserEvents()
        {
            int appointmentId = 1;
            int userIndex = 0;
            DateTime start;
            TimeSpan duration;
            ObservableCollection<UserEvents> result = new ObservableCollection<UserEvents>();
            for (int i = -20; i < 20; i++)
                for (int j = 0; j < 15; j++)
                {
                    int room = rnd.Next(1, 100);
                    start = BaseDate.AddDays(i).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));
                    duration = TimeSpan.FromMinutes(rnd.Next(20, 30));
                    result.Add(CreateUserAppointment(appointmentId, User[userIndex],
                                                      start, duration, room));
                    appointmentId++;
                    userIndex++;
                    if (userIndex >= User.Length - 1)
                        userIndex = 1;
                }
            UserEven = result;
        }

        void CreateLabels()
        {
            ObservableCollection<UserEventsType> result = new ObservableCollection<UserEventsType>();
            int count = AppointmentTypes.Length;
            for (int i = 0; i < count; i++)
            {
                UserEventsType appointmentType = new UserEventsType();
                appointmentType.Id = i;
                appointmentType.Caption = AppointmentTypes[i];
                appointmentType.Color = AppointmentTypeColors[i];
                result.Add(appointmentType);
            }
            Labels = result;
        }

        UserEvents CreateUserAppointment(int appointmentId, string patientName,
                                                    DateTime start, TimeSpan duration, int room)
        {
            UserEvents userAppEvent = new UserEvents();
            userAppEvent.Id = appointmentId;
            userAppEvent.StartTime = start;
            userAppEvent.EndTime = start.Add(duration);
            userAppEvent.Subject = patientName;
            userAppEvent.LabelId = Labels[rnd.Next(0, 5)].Id;
            if (userAppEvent.LabelId != 3)
                userAppEvent.Location = string.Format("{0}", room);
            return userAppEvent;
        }

        public ObservableCollection<UserEvents> UserEven { get; private set; }
        public ObservableCollection<UserEventsType> Labels { get; private set; }


        public AppData()
        {
            CreateLabels();
            CreateUserEvents();
        }
    }
}