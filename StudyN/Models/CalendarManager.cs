using System;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using AndroidX.Fragment.App.StrictMode;
using DevExpress.Maui.Scheduler;
using DevExpress.Maui.Scheduler.Internal;
using Microsoft.Maui.Controls;
using StudyN.Utilities;
using DevExpress.Data.Mask;
using Newtonsoft.Json;

namespace StudyN.Models
{
    public class CalendarManager
    {
        public static DateTime BaseDate = DateTime.Today;

        public static string[] AppointmentCategoryTitles = { "StudyN Time", "Class", "Appointment", "Assignment", "Free Time", "Exam", "Office Hours", "Work"};
        public static Color[] AppointmentCategoryColors = { Color.FromArgb("#3333FF"),   // dark blue
                                                        Color.FromArgb("#00FF00"),   // green                                                        
                                                        Color.FromArgb("#D80073"),   // dark pink
                                                        Color.FromArgb("#FFCB21"),   // mustard
                                                        Color.FromArgb("#1BA1E2"),   // medium blue                                                        
                                                        Color.FromArgb("FF8000"),    // orange
                                                        Color.FromArgb("#FF0000"),   // burgundy                                                         
                                                        Color.FromArgb("#6A00FF") };   // purple
        public static double[] AppointmentCategoryX = { 0.65f, 0.35f, 0.9f, 0.15f, 0.52f, 0.1f, 0.98f, 0.8f};
        // Uncategorized category

        public static AppointmentCategory Uncategorized = new()

        {

            Id = Guid.NewGuid(),

            Caption = "Uncategorized",

            Color = Color.FromArgb("#D9D9D9"),

            PickerXPosition = 0.5f,

            PickerYPosition = 1.0f

        };
                                                                                      
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


        public void CreateAppointmentCategories()
        {
            int count = AppointmentCategoryTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentCategory cat = new AppointmentCategory();
                cat.Id = Guid.NewGuid();
                cat.Caption = AppointmentCategoryTitles[i];
                cat.Color = AppointmentCategoryColors[i];
                cat.PickerXPosition = AppointmentCategoryX[i];
                cat.PickerYPosition = 0.5f;
                AppointmentCategories.Add(cat);
                EventBus.PublishEvent(
                            new StudynEvent(cat.Id, StudynEvent.StudynEventType.CategoryAdd));
            }
        }

        public AppointmentCategory GetAppointmentCategory(Guid id)
        {
            // go through categories
            foreach(AppointmentCategory category in AppointmentCategories)
            {
                // if the category is found return it
                if(category.Id == id)
                {
                    return category;
                }
            }
            // else return null
            return null;
        }

        void CreateAppointmentStatuses()
        {
            int count = AppointmentStatusTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentStatus stat = new AppointmentStatus();
                stat.Id = i;
                stat.Caption = AppointmentStatusTitles[i];
                stat.Color = AppointmentStatusColors[i];
                AppointmentStatuses.Add(stat);
            }
        }

        public Appointment CreateAppointment(int appointmentId,
                                            string appointmentTitle,
                                            DateTime start,
                                            TimeSpan duration,
                                            int room,
                                            Guid guid = new Guid(),
                                            String from = "")
        {
            Appointment appt = new()
            {
                Id = appointmentId,
                Start = start,
                End = start.Add(duration),
                Subject = appointmentTitle,
                LabelId = AppointmentCategories[rnd.Next(0, 5)].Id,
                StatusId = AppointmentStatuses[rnd.Next(0, 5)].Id,
                Location = string.Format("{0}", room),
                Description = string.Empty,
                UniqueId = guid,
                From = from
            };

            Appointments.Add(appt);

            // Publish appointment add event

            if (appt.From != "autoScheduler")
            {
                EventBus.PublishEvent(
                            new StudynEvent(guid, StudynEvent.StudynEventType.AppointmentAdd));
            }

            return appt;
        }

        /// <summary>
        /// Function for creating a new category and adding it to category list
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="categoryColor"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppointmentCategory CreateCategory(string categoryName, 

                                                   Color categoryColor,

                                                   double x, double y,

                                                   Guid id = new Guid())

        {

            // Makes a new category

            AppointmentCategory cat = new()

            {

                Id = id,

                Caption = categoryName,

                Color = categoryColor,

                PickerXPosition = x,

                PickerYPosition = y

            };

            // Adds category to category list

            AppointmentCategories.Add(cat);

            EventBus.PublishEvent(
                        new StudynEvent(cat.Id, StudynEvent.StudynEventType.CategoryAdd));

            return cat;

        }

        /// <summary>
        /// Edits an existing category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="categoryColor"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EditCategory(string categoryName, 
                                 Color categoryColor,
                                 double x, double y,
                                 Guid id)

        {

            // Get the category

            AppointmentCategory cat = null;

            foreach (AppointmentCategory category in AppointmentCategories)

            {

                if(category.Id == id)

                {

                    cat = category;

                }

            }

            if(cat == null)

            {

                return false;

            }

            // add new elements to category

            cat.Caption = categoryName;

            cat.Color = categoryColor;

            cat.PickerXPosition = x;

            cat.PickerYPosition = y;

            EventBus.PublishEvent(
                        new StudynEvent(id, StudynEvent.StudynEventType.CategoryEdit));

            return true;

        }

        /// <summary>
        /// Removes a category
        /// </summary>
        /// <param name="id"></param>
        public void RemoveCategory(Guid id)
        {
            // Search for category
            foreach (AppointmentCategory category in AppointmentCategories)
            {
                if(category.Id == id)
                {
                    // go through the appointments with the category
                    foreach(Appointment appointment in Appointments)
                    {
                        if(appointment.LabelId == category)
                        {
                            // Make appointment uncategorized
                            appointment.LabelId = Uncategorized;
                        }
                    }
                    // Remove category
                    AppointmentCategories.Remove(category);
                    EventBus.PublishEvent(
                                new StudynEvent(id, StudynEvent.StudynEventType.CategoryDelete));
                    return;
                }
            }
        }

        // Properly handle appointments associated with a newly completed task
        public void TaskCompleted(Guid uniqueId)
        {
            // Get list of all potentiall affected appointments
            var affectedAppointments = new List<Appointment>();
            foreach (Appointment apt in Appointments)
            {
                if (apt.UniqueId == uniqueId)
                {
                    affectedAppointments.Add(apt);
                }
            }

            // Now take the list of associated appointments and deal with
            // them as needed. Can't do this above because out iterator
            // will become invalid the second we have to remove an appoinment
            foreach (Appointment apt in affectedAppointments)
            {
                // If Start is after now, remove appointment
                if (apt.Start > DateTime.Now)
                {
                    Appointments.Remove(apt);
                }

                // If event is currently happening,
                // truncate the appointments time to now
                if (apt.Start < DateTime.Now
                    && apt.End > DateTime.Now)
                {
                    apt.End = DateTime.Now;
                }
            }
        }
        
        private void AppointmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                try
                {
                    var apptList = sender as ObservableCollection<Appointment>;

                    foreach(Appointment appt in apptList)
                    {
                        Console.WriteLine("Attempting to publishing appt from " + appt.From);
                        // Publish add appointment
                        if (appt.From != "autoScheduler")
                        {
                            EventBus.PublishEvent(
                                new StudynEvent(appt.UniqueId,
                                StudynEvent.StudynEventType.AppointmentAdd));
                        }
                    }
                }
                catch (NullReferenceException execption)
                {
                    Console.WriteLine(execption.Message);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Publish delete appointment
                EventBus.PublishEvent(
                            new StudynEvent(new Guid(),
                            StudynEvent.StudynEventType.AppointmentDelete));
            }
        }

        // Calculate the number of total hours scheduled to work on tasks today
        public int NumHoursScheduledToday()
        {
            double numMinScheduled = 0;
            foreach(Appointment appt in Appointments)
            {
                // Check if associated task exits
                if (GlobalTaskData.TaskManager.GetTask(appt.UniqueId) != null)
                {
                    numMinScheduled += (appt.End - appt.Start).TotalMinutes;
                }
            }
            // Return as int for simple UI
            return (int)(numMinScheduled/60);
        }

        // Calculate the number of hours scheduled today that have already been completed
        public int NumHoursCompletedToday()
        {
            double numMinCompleted = 0;
            foreach (Appointment appt in Appointments)
            {
                TaskItem task = GlobalTaskData.TaskManager.GetTask(appt.UniqueId);
                // Check if associated task exits
                if (task != null)
                {
                    // Look at logged times
                    foreach (TaskItemTime taskTime in task.TimeList)
                    {
                        // Add up times that finished before "now"
                        // that started sometime today
                        if(taskTime.stop < DateTime.Now
                            && taskTime.start == DateTime.Today)
                        {
                            numMinCompleted += taskTime.span.TotalMinutes;
                        }
                    }
                }
            }
            return (int)(numMinCompleted/60);
        }

        public void LoadFilesIntoAppointCategories()
        {
            string jsonFileText;
            // gets categories
            string[] categoryFileList = FileManager.LoadCategoryFileNames();
            foreach (string file in categoryFileList)
            {
                jsonFileText = File.ReadAllText(file);
                SerializedAppointmentCategory deserializer = JsonConvert.DeserializeObject<SerializedAppointmentCategory>(jsonFileText);
                AppointmentCategory category = new AppointmentCategory();
                category.Id = deserializer.Id;
                category.Caption = deserializer.Caption;
                category.Color = Color.FromArgb(deserializer.Color);
                category.PickerXPosition = deserializer.PickerXPosition;
                category.PickerYPosition = deserializer.PickerYPosition;
                AppointmentCategories.Add(category);
            }
        }

        public ObservableCollection<Appointment> Appointments { get; private set; }
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get; private set; }
        public ObservableCollection<AppointmentStatus> AppointmentStatuses { get; private set; }


        public CalendarManager()
        {
            Appointments = new ObservableCollection<Appointment>();
            AppointmentCategories = new ObservableCollection<AppointmentCategory>();
            AppointmentStatuses = new ObservableCollection<AppointmentStatus>();
            
            // Handle changes to collection
            Appointments.CollectionChanged  += new NotifyCollectionChangedEventHandler(AppointmentCollectionChanged);

            // check if pointer file doesn't exist before make default files
            if (FileManager.LoadCategoryFileNames().Length == 0)
            {
                CreateAppointmentCategories();
            }
            
            CreateAppointmentStatuses();
            //CreateAppointments();
        }
    }
}
