using DevExpress.Maui.DataForm;

namespace StudyN.ViewModels
{
	public class TaskDetailViewModel : TaskDataViewModel
	{
        public const string ViewName = "Task Details";

        // fields
        private string name;
        private string description;
        private List<DateTime> startTimes;
        private List<DateTime> endTimes;
        private DateTime dueDate;
        private bool complete = false;

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
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

        /// <summary>
        /// Gets the data of the selected task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task LoadTaskId(int taskId)
        {
            var task = await TaskStore.GetItemAsync(taskId);
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            StartTimes = task.StartTimes;
            EndTimes = task.EndTimes;
            DueDate = task.DueDate;
            Complete = task.Completed;
        }
	}
}