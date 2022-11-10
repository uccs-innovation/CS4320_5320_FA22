using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class TaskItem
    {
        public TaskItem(string name,
                        string description,
                        DateTime dueTime,
                        int priority,
                        double completionProgress,
                        double totalTimeNeeded,
                        string recur)
        {
            this.Name = name;
            this.Description = description;
            this.DueTime = dueTime;
            this.Priority = priority;
            this.CompletionProgress = completionProgress;
            this.TotalTimeNeeded = totalTimeNeeded;
            this.Recur = recur;
        }

        public bool Completed { get; set; } = false;
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public DateTime DueTime { get; set; }
        public double CompletionProgress { get; set; } = 0;
        public double TotalTimeNeeded { get; set; } = 0;
        public int Priority { get; set; } = 3;
        public bool BeingTimed { get; set; } = false;
        public List<TaskItemTime> TimeList { get; set; } = new List<TaskItemTime>();


        public string Recur { get; set; } = "";

        public bool IsRecur { get; set; } = false;

        public double? weight { get; set; } = null;


        //Method for each task %
        public double? Percent
        {
            get
            {
                if (TotalTimeNeeded != 0)
                {
                    double percentage = CompletionProgress / TotalTimeNeeded;
                    if (percentage == Double.NaN)
                        return 0;
                    else
                        return percentage;
                }
                else
                    //If the total time is empty then it does not display %
                    return null;
            }
        }

        /// <summary>
        /// Gets the minutes from Completion Progress
        /// </summary>
        /// <returns></returns>
        public int GetCompletionProgressMinutes()
        {
            double minutesRemain = CompletionProgress % 1;
            minutesRemain *= 60;
            return (int)minutesRemain;
        }

        /// <summary>
        /// Gets the minutes from Total Time Needed
        /// </summary>
        /// <returns></returns>
        public int GetTotalMinutesNeeded()
        {
            double minutesRemain = TotalTimeNeeded % 1;
            minutesRemain *= 60;
            return (int)minutesRemain;
        }
    }
}


