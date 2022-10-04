using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class ListTask
    {
        public ListTask(string name, string description, DateTime dueTime, int priority, int completionProgress, int totalTimeNeeded)
        {
            this.Name = name;
            this.Description = description;
            this.DueTime = dueTime;
            this.Priority = priority;
            this.CompletionProgress = completionProgress;
            this.TotalTimeNeeded = totalTimeNeeded;
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
           /* ListTask task = AddTask("HW: Pitch your Application Idea", DateTime.Today);
            task.Description = "Pitch your appilcation idea...";

            task = AddTask("HW: Technology Proof of Concept", DateTime.Today);
            task.Description = "Prove your technology works...";

            task = AddTask("HW: Prototype of Key Features", DateTime.Today.AddHours(24));
            task.Description = "Build a prototype of the feature...";*/
        }

        public ListTask AddTask(string name, string description, DateTime dueTime, int priority, int CompletionProgress, int TotalTimeNeeded)
        {
            ListTask newTask = new ListTask(name, description, dueTime, priority, CompletionProgress, TotalTimeNeeded);
            newTask.Parent = this;
            ListTasks.Add(newTask);
            return newTask;
        }

        public void RemoveTask(Guid taskId)
        {
            foreach(ListTask task in ListTasks)
            {
                if(task.TaskId == taskId)
                {
                    ListTasks.Remove(task);
                    return;
                }
            }
        }
        public void CompleteTask(Guid taskId)
        {
            foreach (ListTask task in ListTasks)
            {
                if (task.TaskId == taskId)
                {
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    ListTasks.Remove(task);
                    return;
                }
            }
        }

        public void DeleteTask(Guid taskId)
        {
            foreach (ListTask task in ListTasks)
            {
                if (task.TaskId == taskId)
                {
                    ListTasks.Remove(task);
                    return;
                }
            }
        }

        public ObservableCollection<ListTask> ListTasks { get; private set; }
        private ObservableCollection<ListTask> CompletedTasks { get; set; }

        public ListTaskData()
        {
            ListTasks = new ObservableCollection<ListTask>();
            CompletedTasks = new ObservableCollection<ListTask>();
            GenerateCalendarTasks();
            UIGlobal.MainData = this;
        }
    }

    public static class UIGlobal
    {
        public static ListTaskData MainData { get; set; }
        public static ListTask ToEdit { get; set; }

      
    }
}


