using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class TaskItem
    {
        public TaskItem(string name, string description, DateTime dueTime, int priority, int completionProgress, int totalTimeNeeded)
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
        public TaskDataManager Parent { get; set; } = null;
    }

    public class TaskDataManager
    {
        public TaskItem AddTask(string name, string description, DateTime dueTime, int priority, int CompletionProgress, int TotalTimeNeeded)
        {
            TaskItem newTask = new TaskItem(name, description, dueTime, priority, CompletionProgress, TotalTimeNeeded);
            newTask.Parent = this;
            TaskList.Add(newTask);
            return newTask;
        }

        public TaskItem AddTask(TaskItem task)
        {
            TaskItem newTask = new TaskItem(task.Name, task.Description, task.DueTime, task.Priority, task.CompletionProgress, task.TotalTimeNeeded);
            newTask.Parent = this;
            TaskList.Add(newTask);
            return newTask;
        }

        public void RemoveTask(Guid taskId)
        {
            foreach(TaskItem task in TaskList)
            {
                if(task.TaskId == taskId)
                {
                    TaskList.Remove(task);
                    return;
                }
            }
        }
        public void CompleteTask(Guid taskId)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    TaskList.Remove(task);
                    return;
                }
            }
        }

        public void DeleteTask(Guid taskId)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    TaskList.Remove(task);
                    return;
                }
            }
        }

        public ObservableCollection<TaskItem> TaskList { get; private set; }
        private ObservableCollection<TaskItem> CompletedTasks { get; set; }

        public TaskDataManager()
        {
            TaskList = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();
            UIGlobal.MainData = this;
        }
    }

    public static class UIGlobal
    {
        public static TaskDataManager MainData { get; set; }
        public static TaskItem ToEdit { get; set; }

    }
}


