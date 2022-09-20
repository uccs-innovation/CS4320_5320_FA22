using StudyN.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        public CalendarTasksData data;

        public event PropertyChangedEventHandler PropertyChanged;
        public IReadOnlyList<CalendarTask> CalendarTasks { get => data.CalendarTasks; }

        public TaskDataViewModel()
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