using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace StudyN.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; }        
        public int StatusId { get; set; }
        public string RecurrenceInfo { get; set; }
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
        public static Color[] AppointmentCategoryColors = { Color.FromHex("#a8d5ff"),   // periwinkle blue
                                                        Color.FromHex("#c2f49d"),   // lime                                                        
                                                        Color.FromHex("#FCC7FF"),   // orchid
                                                        Color.FromHex("#FDFDB1"),   // yellow
                                                        Color.FromHex("#8de8df"),   // aqua                                                        
                                                        Color.FromHex("FDD5B1"),    // apricot
                                                        Color.FromHex("#FFC7D8"),   // salmon                                                         
                                                        Color.FromHex("#dfcfe9") };   // lavendar
                                                                                      //Color.FromHex("#c8f4ff") }; // cornflower blue //save color if needed later

        public static string[] AppointmentStatusTitles = { "Free", "Busy", "Blocked", "Tentative", "Flexible" };
        public static Color[] AppointmentStatusColors = { Color.FromHex("00FF80"),   // light green
                                                          Color.FromHex("#FF3333"),  // red                                                        
                                                          Color.FromHex("FF33FF"),   // magenta
                                                          Color.FromHex("#FFFF00"),  // yellow
                                                          Color.FromHex("#00FFFF") };// cyan
                                                                                                                 
        //Calls to the things that we are going to do
        public static string[] AppointmentList = { "Soccer ", "Math Class", "CS Class",
                                                "going out to hike", "Sleeping", "English",
                                                "Professor Office", "Going to work", "Going to conert",
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
                    result.Add(CreateAppointment(appointmentId, AppointmentList[appointmentListIndex],
                                                      start, duration, room));
                    appointmentId++;
                    appointmentListIndex++;
                    if (appointmentListIndex >= AppointmentList.Length - 1)
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
                AppointmentCategory appointmentType = new AppointmentCategory();
                appointmentType.Id = i;
                appointmentType.Caption = AppointmentCategoryTitles[i];
                appointmentType.Color = AppointmentCategoryColors[i];
                result.Add(appointmentType);
            }
            AppointmentCategories = result;
        }

        void CreateAppointmentStatuses()
        {
            ObservableCollection<AppointmentStatus> result = new ObservableCollection<AppointmentStatus>();
            int count = AppointmentStatusTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentStatus appointmentStatus = new AppointmentStatus();
                appointmentStatus.Id = i;
                appointmentStatus.Caption = AppointmentStatusTitles[i];
                appointmentStatus.Color = AppointmentStatusColors[i];
                result.Add(appointmentStatus);
            }
            AppointmentStatuses = result;
        }

        Appointment CreateAppointment(int appointmentId, string patientName,
                                                    DateTime start, TimeSpan duration, int room)
        {
            Appointment appt = new Appointment();
            appt.Id = appointmentId;
            appt.StartTime = start;
            appt.EndTime = start.Add(duration);
            appt.Subject = patientName;
            appt.CategoryId = AppointmentCategories[rnd.Next(0, 5)].Id;
            appt.StatusId = AppointmentStatuses[rnd.Next(0, 5)].Id;
            appt.Location = string.Format("{0}", room);
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