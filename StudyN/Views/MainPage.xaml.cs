using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();

            GlobalTaskTimeData.TaskTimeManager = new TaskTimeDataManager();
            GlobalTaskData.TaskManager = new TaskDataManager();
            GlobalTaskData.TaskManager.LoadFilesIntoLists();
        }
    }
}