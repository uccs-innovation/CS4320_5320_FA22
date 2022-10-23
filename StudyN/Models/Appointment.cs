using DevExpress.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class Appointment : AppointmentItem
    {
        public Guid UniqueId { get; set; }
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
}
