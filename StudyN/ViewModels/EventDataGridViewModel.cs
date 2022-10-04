using DevExpress.Maui.DataForm;
using Ical.Net.CalendarComponents;
using StudyN.Models;
using StudyN.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.ViewModels
{
    public class EventDataGridViewModel : BaseViewModel
    {
        [DataFormDisplayOptions(IsVisible = false)]
        public IcalViewModel Model { get; }

        [DataFormDisplayOptions(IsVisible = false)]
        public ObservableCollection<StudyNCalendarEvent> Events { get; }

        IDictionary<string, StudyNCalendarEvent> eventDictionary { get; }
        public EventDataGridViewModel()
        {
            Title = "Imported Events";
            Model = new IcalViewModel();
            Events = new ObservableCollection<StudyNCalendarEvent>();
            eventDictionary = new Dictionary<string, StudyNCalendarEvent>();
        }

        public void OnAppearing()
        {
            Model.OnLoadEvents();
            if (Model.Events != null && Model.Events.Any())
            {
                foreach (var eve in Model.Events)
                {
                    Events.Add(MapRecurrenceForDisplay(new StudyNCalendarEvent(eve)));
                }
            }
        }

        private static StudyNCalendarEvent MapRecurrenceForDisplay(StudyNCalendarEvent cEvent)
        {
            if (cEvent != null && cEvent.Recurrence != null)
            {
                var rDetail = string.Empty;
                if(cEvent.Recurrence.BySecond.Any())
                {
                    rDetail = "By Second: ";
                    foreach(var sec in cEvent.Recurrence.BySecond)
                    {
                        rDetail += $"{sec}, ";
                    }
                }
                else if (cEvent.Recurrence.ByMinute.Any())
                {
                    rDetail = "By Minute: ";
                    foreach (var minu in cEvent.Recurrence.ByMinute)
                    {
                        rDetail += $"{minu}, ";
                    }
                }
                else if (cEvent.Recurrence.ByHour.Any())
                {
                    rDetail = "By Hour: ";
                    foreach (var hour in cEvent.Recurrence.ByHour)
                    {
                        rDetail += $"{hour}, ";
                    }
                }
                else if (cEvent.Recurrence.ByDay.Any())
                {
                    rDetail = "By Day: ";
                    foreach (var day in cEvent.Recurrence.ByDay)
                    {
                        rDetail += $"{day}, ";
                    }
                }
                else if (cEvent.Recurrence.ByMonth.Any())
                {
                    rDetail = "By Month: ";
                    foreach (var month in cEvent.Recurrence.ByMonth)
                    {
                        rDetail += $"{month}, ";
                    }
                }
                cEvent.RecurrenceRulesBy = rDetail.TrimEnd(',');
                cEvent.RecurrenceRulesCountUntil = cEvent.Recurrence.Count > 0?$"Count: {cEvent.Recurrence.Count}":$"Until: {cEvent.Recurrence.Until}";
            }
            return cEvent;
        }
    }
}
