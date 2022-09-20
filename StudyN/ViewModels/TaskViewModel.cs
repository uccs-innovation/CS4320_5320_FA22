using StudyN.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static Android.Icu.Text.CaseMap;
namespace StudyN.ViewModels
{
    public class TaskDataViewModel : INotifyPropertyChanged
    {
        readonly Task data;

        public event PropertyChangedEventHandler PropertyChanged;
        public IReadOnlyList<Task> Employees { get => data.Tasks; }

        public TaskDataViewModel()
        {
            Title = "Tasks"
            data = new TaskData();
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