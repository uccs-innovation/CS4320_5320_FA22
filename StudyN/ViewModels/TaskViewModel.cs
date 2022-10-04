using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        private CalendarTasksData data;
        //private CalendarTask modtask;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CalendarTask> CalendarTasks { get => data.CalendarTasks; }

        public Command AddCommand { get; }

        public TaskDataViewModel()
        {
            data = new CalendarTasksData();
            AddCommand = new Command(OnClickAdd);
            //modtask = new CalendarTask();
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        async void OnClickAdd(object sender)
        {
            Routing.RegisterRoute(nameof(Views.NewTaskPage), typeof(Views.NewTaskPage));
            await Shell.Current.GoToAsync(nameof(Views.NewTaskPage));
        }
    }
}