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

        public TaskDataViewModel()
        {
        }

        protected void RaisePropertyChanged(string name)
        {
            Console.WriteLine("TaskViewModel property changed, name = " + name);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}