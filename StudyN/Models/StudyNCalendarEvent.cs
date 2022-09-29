using Ical.Net.CalendarComponents;

namespace StudyN.Models
{
    public class StudyNCalendarEvent
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Created { get; set; }
        public string Uid { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string LastModified { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        public StudyNCalendarEvent() { }

        public StudyNCalendarEvent(CalendarEvent calE)
        {
            Start = calE.Start.ToString();
            End = calE.End.ToString();
            Created = calE.Created.ToString();
            Uid = calE.Uid;
            Description = calE.Description;
            Summary = calE.Summary;
            LastModified = calE.LastModified.ToString();
            Location = calE.Location;
            Status = calE.Status;
        }
    }
}
