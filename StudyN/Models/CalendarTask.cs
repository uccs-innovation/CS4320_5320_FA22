using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class CalendarTask
    {
        public CalendarTask(string name, DateTime dueTime)
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
        public CalendarTasksData Parent { get; set; } = null;
    }

    public class CalendarTasksData
    {
        void GenerateCalendarTasks()
        {
            CalendarTask task = AddTask("HW: Pitch your Application Idea", DateTime.Today);
            task.Description = "Pitch your appilcation idea...";

            task = AddTask("HW: Technology Proof of Concept", DateTime.Today);
            task.Description = "Prove your technology works...";

            task = AddTask("HW: Prototype of Key Features", DateTime.Today.AddHours(24));
            task.Description = "Build a prototype of the feature...";
        }

        public CalendarTask AddTask(string name, DateTime dueTime)
        {
            CalendarTask newTask = new CalendarTask(name, dueTime);
            newTask.Parent = this;
            CalendarTasks.Add(newTask);
            return newTask;
        }

        public void RemoveTask(Guid taskId)
        {
            foreach(CalendarTask task in CalendarTasks)
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
            foreach (CalendarTask task in CalendarTasks)
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

        public ObservableCollection<CalendarTask> CalendarTasks { get; private set; }
        private ObservableCollection<CalendarTask> CompletedTasks { get; set; }

        public CalendarTasksData()
        {
            CalendarTasks = new ObservableCollection<CalendarTask>();
            CompletedTasks = new ObservableCollection<CalendarTask>();
            GenerateCalendarTasks();
        }
    }
}
