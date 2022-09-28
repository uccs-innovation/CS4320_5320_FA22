using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyN.Models;
using StudyN.Services;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : BaseViewModel
    {
        //CalendarTask _selectedTask;

        private CalendarTasksData data;

        public ObservableCollection<CalendarTask> CalendarTasks { get => data.CalendarTasks; }

        public IDataStore<CalendarTask> TaskStore => DependencyService.Get<IDataStore<CalendarTask>>();

        public Command LoadTasksCommand { get; }
        public TaskDataViewModel()
        {
            data = new CalendarTasksData();
        }
        
        /*
        public Command<CalendarTask> TaskClicked { get; }
        public CalendarTask SelectedTask
        {
            get => _selectedTask;
            set { SetProperty(ref _selectedTask, value);}
        }
        */
        
        /*
        async void OnTaskClick(CalendarTask task)
        {
            if(task == null)
            {
                return;
            }
        }
        */
    }
}