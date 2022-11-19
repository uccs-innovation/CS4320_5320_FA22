using DevExpress.Maui.Scheduler;
using DevExpress.Maui.Editors;
using StudyN.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class Appointment : AppointmentItem
    {        
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public Guid ParentTaskId { get; set; }
        public string ReminderInfo { get; set; }
        public string Notes { get; set; }

        // properties for StudyN_Time category
        public bool IsGeneratedStudyNTime { get; set; }
        
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

        public DateTime LastEdited { get; set; }

        public string From { get; set; } //IE From "autoScheduler", from "userInput", from "ICS", etc...

        protected void ApptChanged(object sender, PropertyChangedEventArgs e)
        {
            // Publish Appointment Edit
            // Checking if the last edit was within the last second
            // prevents us from sending an event for each property
            if ((DateTime.Now - LastEdited).TotalSeconds > 1)
            {
                LastEdited = DateTime.Now;
                // Publish task delete appointment
                EventBus.PublishEvent(
                            new StudynEvent(UniqueId,
                            StudynEvent.StudynEventType.AppointmentEdit));
            }
        }

        public Appointment() : base()
        {
            //UniqueId = new Guid();
            LastEdited = DateTime.Now;
            PropertyChanged += new PropertyChangedEventHandler(ApptChanged);
        }
    }

    // Built-in Scheduler DataStorage DataSource AppointmentLabelsSource
    public class AppointmentLabel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Color Color { get; set; }
    }

    /// <summary>
    /// Used to serialize categories into json files
    /// Colors can't be serialized
    /// </summary>
    public class SerializedAppointmentCategory
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public string Color { get; set; }
        public double PickerXPosition { get; set; }
        public double PickerYPosition { get; set; }
    }

    public class AppointmentStatus
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public Color Color { get; set; }
    }

    public class SleepTime
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
  
    // Custom Categories
    public class AppointmentCategory
    {
        public Guid Id { get; set; }  // custom id        
        public string Caption { get; set; }
        public Color Color { get; set; }
        public double PickerXPosition { get; set; }
        public double PickerYPosition { get; set; }
    }
}
