

using StudyN.Models;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels
{
    public class AnalyticsViewModel : BaseViewModel
    {
        public ObservableCollection<Event> CalendarEvents { get; set; }

        public AnalyticsViewModel()
        {
            Title = "Analytics";
            InitializeEvents();
        }

        public void InitializeEvents()
        {
            ObservableCollection<Event> EventCollection = new();
            var data = GlobalAppointmentData.CalendarManager;
            Dictionary<string, int> tempDict = new();
            int numOfEvents = 0;
            foreach (var item in data.Appointments)
            {
                numOfEvents++;
                string category = data.AppointmentCategories[(int)item.LabelId].Caption;
                if (!tempDict.ContainsKey(category))
                {
                    tempDict.Add(category, 1);
                }
                else
                {
                    tempDict[category] += 1;
                }
            }
            foreach (var key in tempDict.Keys)
            {
                EventCollection.Add(new(key, tempDict[key]));
            }
            CalendarEvents = EventCollection;
        }

        public class Event : BindableObject
        {
            public string EventType { get; }
            public int EventAmount{ get; }

            public Event(string EventType, int EventAmount)
            {
                this.EventType = EventType;
                this.EventAmount = EventAmount;
            }
        }
    }
}
