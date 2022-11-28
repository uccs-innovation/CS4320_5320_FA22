using StudyN.Models;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels
{
    public class AnalyticsViewModel : BaseViewModel
    {
        public ObservableCollection<Event> CalendarEvents { get; set; }
        public Color[] Palette { get; set; }

        public AnalyticsViewModel()
        {
            Title = "Analytics";
            InitializeEvents();
            InitalizeColorPalette();
        }

        public void InitializeEvents()
        {
            ObservableCollection<Event> EventCollection = new();
            var data = GlobalAppointmentData.CalendarManager;
            Dictionary<string, int> tempDict = new();
            foreach (var item in data.Appointments)
            {
                Guid labelId = Guid.Parse(item.LabelId.ToString());
                AppointmentCategory category = data.GetAppointmentCategory(labelId);
                if (category == null)
                {
                    Console.WriteLine("Invalid Category Id: ");
                    Console.WriteLine("    Label Id: " + labelId.ToString());
                    continue;
                }

                string caption = data.GetAppointmentCategory(labelId).Caption;
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

        public void InitalizeColorPalette()
        {
            var data = GlobalAppointmentData.CalendarManager;
            Color[] ColorList = new Color[data.AppointmentCategories.Count];
            int i = 0;
            foreach (var item in data.Appointments)
            {
                Guid labelId = Guid.Parse(item.LabelId.ToString());
                AppointmentCategory category = data.GetAppointmentCategory(labelId);
                if (category == null)
                {
                    Console.WriteLine("Invalid Category Id: ");
                    Console.WriteLine("    Label Id: " + labelId.ToString());
                    continue;
                }

                Color color = category.Color;
                if (!ColorList.Contains(color))
                {
                    ColorList[i] = color;
                    i++;
                }
            }
            for (int j = 0; j < ColorList.Length; j++)
            {
                if (ColorList[j] == null)
                {
                    ColorList[j] = Colors.White;
                }
            }
            Palette = ColorList;
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
