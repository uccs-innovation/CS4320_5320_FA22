using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class DisplayViewModel : INotifyPropertyChanged
    {
        private CalendarTasksData data;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CalendarTask> CalendarTasks { get => data.CalendarTasks; }

        public DisplayViewModel()
        {
            data = new CalendarTasksData();
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}