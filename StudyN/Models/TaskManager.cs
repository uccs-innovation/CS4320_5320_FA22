using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class TaskItem
    {
        public TaskItem(string name,
                        string description,
                        DateTime dueTime,
                        int priority,
                        int completionProgress,
                        int totalTimeNeeded,
                        double percent)
        {
            this.Name = name;
            this.Description = description;
            this.DueTime = dueTime;
            this.Priority = priority;
            this.CompletionProgress = completionProgress;
            this.TotalTimeNeeded = totalTimeNeeded;
            this.Percent = percent;
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
        private double _percentage; //to display % of completenes

        public double Percent { 
            get {
                if (TotalTimeNeeded != 0)
                {
                    _percentage = (double)CompletionProgress / (double)TotalTimeNeeded;
                    if (_percentage == Double.NaN)
                        return 0;
                    else
                        return _percentage;
                }
                else
                    return 0;
            }
            set { 
                 _percentage = value;
            }
        }

    }
    public class TaskDataManager
    {
        public TaskItem AddTask(string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                double Percent)
        {
            TaskItem newTask  = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded,
                                            Percent);
            newTask.Parent = this;
            TaskList.Add(newTask);
            return newTask;
        }

        public TaskItem AddTask(TaskItem task)
        {
            TaskItem newTask = new TaskItem(task.Name,
                                            task.Description,
                                            task.DueTime,
                                            task.Priority,
                                            task.CompletionProgress,
                                            task.TotalTimeNeeded,
                                            task.Percent);
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

            GenerateTestData_Tasks(); // estepanek:just for testing
            
        }

        void GenerateTestData_Tasks()
        {
            Random rnd = new Random();
            DateTime tmpDate;
            ObservableCollection<TaskItem> result = new ObservableCollection<TaskItem>();
           
            tmpDate = DateTime.Today.AddDays(1).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));                    
            result.Add(AddTask("Wireframes", "Wireframes for CS5320", tmpDate, 0, 0, 3, 0));

            tmpDate = DateTime.Today.AddDays(2).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));
            result.Add(AddTask("Development", "Feature development for CS5320", tmpDate, 0, 4, 6, 0));

            tmpDate = DateTime.Today.AddDays(3).AddHours(rnd.Next(8, 17)).AddMinutes(rnd.Next(0, 40));
            result.Add(AddTask("Study for Midterm", "Study for CS5320 Midterm", tmpDate, 0, 5, 10, 0));

            TaskList = result;
        }

    }

    public static class UIGlobal
    {
        public static TaskDataManager MainData { get; set; }
        public static TaskItem ToEdit { get; set; }

    }
}


