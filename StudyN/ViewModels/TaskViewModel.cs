using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        private TaskDataManager data;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TaskItem> TaskList { get => data.TaskList; }

        public TaskDataViewModel()
        {
            data = new TaskDataManager();
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