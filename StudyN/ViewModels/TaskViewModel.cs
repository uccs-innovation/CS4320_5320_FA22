using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        private ListTaskData data;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ListTask> CalendarTasks { get => data.CalendarTasks; }

        public TaskDataViewModel()
        {
            data = new ListTaskData();
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