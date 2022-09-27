using DevExpress.Maui.DataForm;

namespace StudyN.ViewModels
{
	public class TaskDetailViewModel : TaskDataViewModel
	{
        public const string ViewName = "Task Details";

        // fields
        private string title;
        private string description;
        private List<DateTime> startTimes;
        private List<DateTime> endTimes;
        private DateTime dueDate;
        private bool complete = false;

        public int Id { get; set; }

        public string Title0
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public List<DateTime> StartTimes
        {
            get => startTimes;
            set => SetProperty(ref startTimes, value);
        }

        public List<DateTime> EndTimes
        {
            get => endTimes;
            set => SetProperty(ref endTimes, value);
        }

        public DateTime DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }

        public bool Complete
        {
            get => complete;
            set => SetProperty(ref complete, value);
        }

        public TaskDetailViewModel()
		{
			
		}
	}
}