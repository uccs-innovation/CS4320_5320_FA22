using StudyN.Models;
using StudyN.Utilities;
using StudyN.ViewModels;

namespace StudyN.Views
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            GlobalTaskTimeData.TaskTimeManager = new TaskTimeDataManager();
            GlobalTaskData.TaskManager = new TaskDataManager();
            GlobalTaskData.TaskManager.LoadFilesIntoLists();

            GlobalAppointmentData.CalendarManager = new CalendarManager();
            GlobalAppointmentData.CalendarManager.LoadFilesIntoAppointCategories();
            GlobalAppointmentData.CalendarManager.LoadSleepTime();
            GlobalAutoScheduler.AutoScheduler = new AutoScheduler();

            GlobalAppointmentData.CalendarManager.LoadFilesIntoLists();

            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}