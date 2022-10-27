
namespace StudyN.ViewModels
{
    public class AnalyticsViewModel : BaseViewModel
    {
        public List<CalendarEvent> CalendarEvents { get; set; }

        public AnalyticsViewModel()
        {
            Title = "Analytics";
            CalendarEvents = InitializeEvents();
        }

        public List<CalendarEvent> InitializeEvents()
        {
            List<CalendarEvent> EventList = new();
            EventList.Add(new("Assignments",10));
            EventList.Add(new("Classes",5));
            EventList.Add(new("Work",6));
            return EventList;
        }

        public class CalendarEvent
        {
            public string EventType { get; }
            public int EventAmount{ get; }

            public CalendarEvent(string EventType, int EventAmount)
            {
                this.EventType = EventType;
                this.EventAmount = EventAmount;
            }
        }
    }
}
