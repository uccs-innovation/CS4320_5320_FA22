using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TaskItem> TaskList { get => GlobalTaskData.TaskManager.TaskList; }

        public HomeViewModel()
        {
            Title = "Dashboard";
        }


        async public void OnAppearing()
        {
            await FileManager.WaitForFileOp();
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