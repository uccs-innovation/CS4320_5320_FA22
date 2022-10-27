using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TaskItem> TaskList { get => GlobalTaskData.TaskManager.TaskList; }
        public List<TaskItemTime> TimeList { get => GlobalTaskData.TaskManager.GetTask(GlobalTaskTimeData.TaskTimeManager.TheTaskidBeingTimed).TimeList; }

        public TaskDataViewModel()
        {
        }

        protected void RaisePropertyChanged(string name)
        {
            Console.WriteLine("property changed");
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}