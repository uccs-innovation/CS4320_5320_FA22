using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<TaskItem> TaskList { get => GlobalTaskData.TaskManager.TaskList; }

        public HomeViewModel()
        {
            Title = "Dashboard";
            //Items = new ObservableCollection<Item>();
        }

        //public ObservableCollection<Item> Items { get; private set; }

        async public void OnAppearing()
        {
            await FileManager.WaitForFileOp();
        }
    }
}