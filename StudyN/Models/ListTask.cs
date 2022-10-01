using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class ListTask
    {
        public ListTask(string name, DateTime dueTime)
        {
            this.Name = name;
            this.DueTime = dueTime;
        }

        public bool Completed { get; set; } = false;
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public DateTime DueTime { get; set; }
        public int CompletionProgress { get; set; } = 0;
        public int TotalTimeNeeded { get; set; } = 0;
        public int Priority { get; set; } = 3;
        public ListTaskData Parent { get; set; } = null;
    }

    public class ListTaskData
    {
        void GenerateCalendarTasks()
        {
            ListTask task = AddTask("HW: Pitch your Application Idea", DateTime.Today);
            task.Description = "Pitch your appilcation idea...";

            task = AddTask("HW: Technology Proof of Concept", DateTime.Today);
            task.Description = "Prove your technology works...";

            task = AddTask("HW: Prototype of Key Features", DateTime.Today.AddHours(24));
            task.Description = "Build a prototype of the feature...";
        }

        public ListTask AddTask(string name, DateTime dueTime)
        {
            ListTask newTask = new ListTask(name, dueTime);
            newTask.Parent = this;
            CalendarTasks.Add(newTask);
            return newTask;
        }

        public void RemoveTask(Guid taskId)
        {
            foreach(ListTask task in CalendarTasks)
            {
                if(task.TaskId == taskId)
                {
                    CalendarTasks.Remove(task);
                    return;
                }
            }
        }
        public void TaskComplete(Guid taskId)
        {
            foreach (ListTask task in CalendarTasks)
            {
                if (task.TaskId == taskId)
                {
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    CalendarTasks.Remove(task);
                    return;
                }
            }
        }

        public ObservableCollection<ListTask> CalendarTasks { get; private set; }
        private ObservableCollection<ListTask> CompletedTasks { get; set; }

        public ListTaskData()
        {
            CalendarTasks = new ObservableCollection<ListTask>();
            CompletedTasks = new ObservableCollection<ListTask>();
            GenerateCalendarTasks();
        }
    }
}
