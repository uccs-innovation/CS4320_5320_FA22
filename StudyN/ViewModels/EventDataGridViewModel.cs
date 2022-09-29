using DevExpress.Maui.DataForm;
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
                    Events.Add(new StudyNCalendarEvent(eve));
                }
            }
        }
    }
}
