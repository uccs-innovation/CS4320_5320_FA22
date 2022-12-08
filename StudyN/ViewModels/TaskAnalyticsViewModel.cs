using System.Collections.ObjectModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskAnalyticsViewModel
    {
        public ObservableCollection<TaskData> TasksTimeWorked { get; }
        public ObservableCollection<TaskData> TasksTimeNeeded { get; }

        public TaskAnalyticsViewModel()
        {
            TasksTimeWorked = new ObservableCollection<TaskData>();
            TasksTimeNeeded = new ObservableCollection<TaskData>();

            ObservableCollection<TaskItem> TaskList = GlobalTaskData.TaskManager.TaskList;

            foreach (TaskItem task in TaskList)
            {
                String taskName = task.Name;
                double timeWorked = task.TimeWorked;
                double timeNeeded = task.TimeEstimated;
                
                TasksTimeWorked.Add(new TaskData(taskName, (int)timeWorked));
                if (timeNeeded - timeWorked > 0)
                {
                    TasksTimeNeeded.Add(new TaskData(taskName, (int)timeNeeded - (int)timeWorked));
                }
            }
        }
    }

    public class TaskData
    {
        public String TaskName { get; }
        public int Time { get; }

        public TaskData(String taskName, int time)
        {
            this.TaskName = taskName;
            this.Time = time;
        }
    }
}
