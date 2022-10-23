﻿using System;
using System.Collections.ObjectModel;
using AndroidX.Fragment.App.StrictMode;
using DevExpress.Maui.Scheduler;
using DevExpress.Maui.Scheduler.Internal;
using Microsoft.Maui.Controls;

namespace StudyN.Models
{
    public class CalendarManager
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
            for (int i = -20; i < 20; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    int room = rnd.Next(1, 100);
                    start = BaseDate.AddDays(i).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));
                    duration = TimeSpan.FromMinutes(rnd.Next(20, 30));
                    CreateAppointment(appointmentId,
                                        AppointmentTitles[appointmentListIndex],
                                        start,
                                        duration,
                                        room);
                    appointmentId++;
                    appointmentListIndex++;
                    if (appointmentListIndex >= AppointmentTitles.Length - 1)
                        appointmentListIndex = 1;
                }
            }
        }

        void CreateAppointmentCategories()
        {
            int count = AppointmentCategoryTitles.Length;
            for (int i = 0; i < count; i++)
            {
                AppointmentCategory cat = new AppointmentCategory();
                cat.Id = i;
                cat.Caption = AppointmentCategoryTitles[i];
                cat.Color = AppointmentCategoryColors[i];
                AppointmentCategories.Add(cat);
            }
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

        Appointment CreateAppointment(int appointmentId,
                                        string appointmentTitle,
                                        DateTime start,
                                        TimeSpan duration,
                                        int room,
                                        Guid guid = new Guid())
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
                UniqueId = guid
            };

            Appointments.Add(appt);

            return appt;
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

        public ObservableCollection<Appointment> Appointments { get; private set; }
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get; private set; }
        public ObservableCollection<AppointmentStatus> AppointmentStatuses { get; private set; }


        public CalendarManager()
        {
            CreateAppointmentCategories();
            CreateAppointmentStatuses();
            CreateAppointments();
        }
    }
}