using System;
using System.Collections.ObjectModel;
using DevExpress.Maui.Scheduler;
using DevExpress.Maui.Scheduler.Internal;
using Microsoft.Maui.Controls;

namespace StudyN.Models
{
    public class Appointment : AppointmentItem
    {    
        public string ReminderInfo { get; set; }
        public string Notes { get; set; }

        // properties for StudyN_Time category
        public bool IsGeneratedStudyNTime { get; set; }
        public int ParentTaskId { get; set; }
        public int StudyNBlock_Minutes { get; set; }         
        public bool WasEdited { get; set; }
        public bool IsOrphan { get; set; }

        // properties for Assignment category
        public int EstimatedCompletionTime_Hours { get; set; }

        // properties for Exam category
        public bool IsExamTakehome { get; set; }
        public int ExamTime_Minutes { get; set; }

        // StudyN Time Algorithm properties
        public int BeforePadding_Minutes { get; set; }
        public int AfterPadding_Minutes { get; set; }
        public int MaxBlockTime_Minutes { get; set; }
        public int MinBlockTime_Minutes { get; set; }
        public int BreakTime_Minutes { get; set; }
        public bool AllowBackToBackStudyNSessions { get; set; }
        public bool UseFreeTimeBlocks { get; set; }

        // properties for data import
        public bool IsCanvasImport { get; set; }
        public bool IsExternalCalendarImport { get; set; }
    }


    public class AppointmentCategory
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Color Color { get; set; }
    }

    public class AppointmentStatus
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Color Color { get; set; }
    }


    public class AppData
    {
        public static DateTime BaseDate = DateTime.Today;

        public static string[] AppointmentCategoryTitles = { "StudyN Time", "Class", "Appointment", "Assignment", "Free Time", "Exam", "Office Hours", "Work"};
        public static Color[] AppointmentCategoryColors = { Color.FromArgb("#3333FF"),   // dark blue
                                                        Color.FromArgb("#008A00"),   // green                                                        
                                                        Color.FromArgb("#D80073"),   // dark pink
                                                        Color.FromArgb("#FFCB21"),   // mustard
                                                        Color.FromArgb("#1BA1E2"),   // medium blue                                                        
                                                        Color.FromArgb("FF8000"),    // orange
                                                        Color.FromArgb("#A20025"),   // burgundy                                                         
                                                        Color.FromArgb("#6A00FF") };   // purple
                                                                                      
        public static string[] AppointmentStatusTitles = { "Free", "Busy", "Blocked", "Tentative", "Flexible" };
        public static Color[] AppointmentStatusColors = { Color.FromArgb("00FF80"),   // light green
                                                          Color.FromArgb("#FF3333"),  // red                                                        
                                                          Color.FromArgb("FF33FF"),   // magenta
                                                          Color.FromArgb("#FFFF00"),  // yellow
                                                          Color.FromArgb("#00FFFF") };// cyan
                                                                                                                 
        //Calls to the things that we are going to do
        public static string[] AppointmentTitles = { "Soccer", "Math Class", "CS Class",
                                                "Hike", "Sleep", "English Class",
                                                "Professor Office", "Work", "Concert",
                                                "Homework", "Project", "GYM",
                                                "Going to get Food"};

        static Random rnd = new Random();

        void CreateAppointments()
        {
            int appointmentId = 1;
            int appointmentListIndex = 0;
            DateTime start;
            TimeSpan duration;
            ObservableCollection<Appointment> result = new ObservableCollection<Appointment>();
            for (int i = -20; i < 20; i++)
                for (int j = 0; j < 15; j++)
                {
                    int room = rnd.Next(1, 100);
                    start = BaseDate.AddDays(i).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));
                    duration = TimeSpan.FromMinutes(rnd.Next(20, 30));
                    result.Add(CreateAppointment(appointmentId, AppointmentTitles[appointmentListIndex],
                                                      start, duration, room));
                    appointmentId++;
                    appointmentListIndex++;
                    if (appointmentListIndex >= AppointmentTitles.Length - 1)
                        appointmentListIndex = 1;
                }
            Appointments = result;
        }

        void CreateAppointmentCategories()
        {
            ObservableCollection<AppointmentCategory> result = new ObservableCollection<AppointmentCategory>();
            int count = AppointmentCategoryTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentCategory cat = new AppointmentCategory();
                cat.Id = i;
                cat.Caption = AppointmentCategoryTitles[i];
                cat.Color = AppointmentCategoryColors[i];
                result.Add(cat);
            }
            AppointmentCategories = result;
        }

        void CreateAppointmentStatuses()
        {
            ObservableCollection<AppointmentStatus> result = new ObservableCollection<AppointmentStatus>();
            int count = AppointmentStatusTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentStatus stat = new AppointmentStatus();
                stat.Id = i;
                stat.Caption = AppointmentStatusTitles[i];
                stat.Color = AppointmentStatusColors[i];
                result.Add(stat);
            }
            AppointmentStatuses = result;
        }

        Appointment CreateAppointment(int appointmentId, string appointmentTitle,
                                                    DateTime start, TimeSpan duration, int room)
        {
            Appointment appt = new()
            {
                Id = appointmentId,
                Start = start,
                End = start.Add(duration),
                Subject = appointmentTitle,
                LabelId = AppointmentCategories[rnd.Next(0, 5)].Id,
                StatusId = AppointmentStatuses[rnd.Next(0, 5)].Id,
                Location = string.Format("{0}", room)
            };

            return appt;
        }

        public ObservableCollection<Appointment> Appointments { get; private set; }
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get; private set; }
        public ObservableCollection<AppointmentStatus> AppointmentStatuses { get; private set; }


        public AppData()
        {
            CreateAppointmentCategories();
            CreateAppointmentStatuses();
            CreateAppointments();
        }
    }
}