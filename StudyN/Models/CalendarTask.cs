using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class CalendarTask
    {
        public CalendarTask(string Text)
        {
            this.Name = Text;
        }

        public bool Completed { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public DateTime DueDate { get; set; }
        public int TimeNeeded { get; set; }
        public List<DateTime> StartTimes { get; set; }
        public List<DateTime> EndTimes { get; set; }

        public CalendarTasksData Parent { get; set; }
    }

    public class CalendarTasksData
    {
        void GenerateCalendarTaskss()
        {
            CalendarTasks.Add(
                new CalendarTask("HW: Pitch your Application Idea")
                {
                    Parent = this,
                    Completed = false,
                    Id = 1,
                    Description = "Pitch your appilcation idea...",
                    Type = 0,
                    DueDate = DateTime.Today.AddHours(23).AddMinutes(59),
                    TimeNeeded = 3,
                    StartTimes = { DateTime.Today.AddHours(12) },
                    EndTimes = { DateTime.Today.AddHours(15) }
                }
            ) ;
            CalendarTasks.Add(
                new CalendarTask("HW: Technology Proof of Concept")
                {
                    Parent = this,
                    Completed = false,
                    Id = 2,
                    Description = "Prove your technology works...",
                    Type= 0,
                    DueDate = DateTime.Today.AddHours(47).AddMinutes(59),
                    TimeNeeded = 7,
                    StartTimes = { DateTime.Today.AddHours(15), DateTime.Today.AddHours(36) },
                    EndTimes = { DateTime.Today.AddHours(20), DateTime.Today.AddHours(38) }
                }
            );
            CalendarTasks.Add(
                new CalendarTask("HW: Prototype of Key Features")
                {
                    Parent = this,
                    Completed = false,
                    Id = 3,
                    Description = "Build a prototype of the feature...",
                    Type= 0,
                    DueDate = DateTime.Today.AddHours(47).AddMinutes(59),
                    TimeNeeded = 5,
                    StartTimes = { DateTime.Today.AddHours(38) },
                    EndTimes = { DateTime.Today.AddHours(43) }
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
            GenerateCalendarTaskss();
        }
    }
}
