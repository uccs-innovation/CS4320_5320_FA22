using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class TaskItem
    {
        public TaskItem(string name,
                        string description,
                        DateTime dueTime,
                        int priority,
                        double timeWorked,
                        double timeEstimated,
                        DateTime dateNow,
                        Guid recurId)
        {
            this.Name = name;
            this.Description = description;
            this.DueTime = dueTime;
            this.Priority = priority;
            this.TimeWorked = timeWorked;
            this.TimeEstimated = timeEstimated;
            this.DateNow = dateNow; //for timespan to check latest task added
            this.RecurId = recurId;
        }

        public bool Completed { get; set; } = false;
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public DateTime DueTime { get; set; }
        public double TimeWorked { get; set; } = 0;
        public double TimeEstimated { get; set; } = 0;
        public int Priority { get; set; } = 3;
        public bool BeingTimed { get; set; } = false;
        public DateTime DateNow { get;} 
        public List<TaskItemTime> TimeList { get; set; } = new List<TaskItemTime>();


        public Guid RecurId { get; set; } = Guid.Empty;

        public bool IsRecur { get; set; } = false;

        public double? weight { get; set; } = null;

        public bool hasBeenAutoScheduled { get; set; } = false;


        //Method for each task %
        public double? Percent
        {
            get
            {
                if (TimeEstimated != 0)
                {
                    double percentage = TimeWorked / TimeEstimated;
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
        public int GetMinutesWorked()
        {
            double minutesRemain = TimeWorked % 1;
            minutesRemain *= 60;
            return (int)minutesRemain;
        }

        /// <summary>
        /// Gets the minutes from Total Time Needed
        /// </summary>
        /// <returns></returns>
        public int GetMinutesEstimated()
        {
            double minutesRemain = TimeEstimated % 1;
            minutesRemain *= 60;
            return (int)minutesRemain;
        }
    }
}


