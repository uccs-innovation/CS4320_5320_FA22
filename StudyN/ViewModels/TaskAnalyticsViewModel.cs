using System.Collections.ObjectModel;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class TaskAnalyticsViewModel
    {
        public ObservableCollection<TaskData> TasksTimeWorked { get; }
        public ObservableCollection<TaskData> TasksTimeNeeded { get; }
        public String MaxVisualTaskName { get; set; }

        public TaskAnalyticsViewModel()
        {
            TasksTimeWorked = new ObservableCollection<TaskData>();
            TasksTimeNeeded = new ObservableCollection<TaskData>();

            ObservableCollection<TaskItem> TaskList = GlobalTaskData.TaskManager.TaskList;
            int listSize = 0;

            foreach (TaskItem task in TaskList)
            {
                listSize += 1;

                String taskName = task.Name;
                double timeWorked = task.TimeWorked;
                double timeNeeded = task.TimeEstimated;
                
                TasksTimeWorked.Add(new TaskData(taskName, (int)timeWorked));
                if (timeNeeded - timeWorked > 0)
                {
                    TasksTimeNeeded.Add(new TaskData(taskName, (int)timeNeeded - (int)timeWorked));
                }

                if (listSize <= 3)
                {
                    MaxVisualTaskName = taskName;
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
