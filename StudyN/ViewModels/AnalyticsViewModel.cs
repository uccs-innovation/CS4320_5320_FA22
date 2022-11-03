using DevExpress.XtraRichEdit.Model;
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
                string caption = data.GetAppointmentCategory((Guid)item.LabelId).Caption;
                if (!tempDict.ContainsKey(caption))
                {
                    tempDict.Add(caption, 1);
                }
                else
                {
                    tempDict[caption] += 1;
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
