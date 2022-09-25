using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class CalendarTask
    {
        public CalendarTask(string Text, DateTime DueTime)
        {
            this.Name = Text;
            this.DueTime = DueTime;
        }

        public bool Completed { get; set; } = false;
        public Guid TaskId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public DateTime DueTime { get; set; }
        public int CompletionProgress { get; set; } = 0;
        public int TotalTimeNeeded { get; set; } = 0;
    }

    public class CalendarTasksData
    {
        void GenerateCalendarTasks()
        {
            CalendarTasks.Add(
                new CalendarTask("HW: Pitch your Application Idea", DateTime.Today)
                {
                    //Parent = this,
                    //Completed = false,
                    //Id = 1,
                    //Description = "Pitch your appilcation idea...",
                    //DueDate = DateTime.Today,
                    //TimeNeeded = 3
                }
            ); ;
            CalendarTasks.Add(
                new CalendarTask("HW: Technology Proof of Concept", DateTime.Today)
                {
                    //Parent = this,
                    //Completed = false,
                    //Id = 2,
                    //Description = "Prove your technology works...",
                    //DueDate = DateTime.Today,
                    //TimeNeeded = 7
                }
            );
            CalendarTasks.Add(
                new CalendarTask("HW: Prototype of Key Features", DateTime.Today)
                {
                    //Parent = this,
                    //Completed = false,
                    //Id = 3,
                    //Description = "Build a prototype of the feature...",
                    //DueDate = DateTime.Today.AddHours(24),
                    //TimeNeeded = 5
                }
            );
        }

        public void TaskComplete(CalendarTask task)
        {
            task.Completed = !task.Completed;
            CompletedTasks.Add(task);
            CalendarTasks.Remove(task);
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
