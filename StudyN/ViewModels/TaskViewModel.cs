using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        private CalendarTasksData data;

        bool longPressMenuEnabled = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CalendarTask> CalendarTasks { get => data.CalendarTasks; }
        public Command ShowLongPressOptions { get; set; }
        public TaskDataViewModel()
        {
            data = new CalendarTasksData();

            ShowLongPressOptions = new Command(() =>
            {
                // Execute Logic Here
            },
            () => isLongPressMenuEnabled);
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public bool isLongPressMenuEnabled
        {
            get { return longPressMenuEnabled; }
            set
            {
                isLongPressMenuEnabled = value;
                ShowLongPressOptions.ChangeCanExecute();
            }
        }
    }
}