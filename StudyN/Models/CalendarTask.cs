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
        public DateTime DueDate { get; set; }
        public int TimeNeeded { get; set; }

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
                    DueDate = DateTime.Today,
                    TimeNeeded = 3
                }
            ); ;
            CalendarTasks.Add(
                new CalendarTask("HW: Technology Proof of Concept")
                {
                    Parent = this,
                    Completed = false,
                    Id = 2,
                    Description = "Prove your technology works...",
                    DueDate = DateTime.Today,
                    TimeNeeded = 7
                }
            );
            CalendarTasks.Add(
                new CalendarTask("HW: Prototype of Key Features")
                {
                    Parent = this,
                    Completed = false,
                    Id = 3,
                    Description = "Build a prototype of the feature...",
                    DueDate = DateTime.Today.AddDays(3),
                    TimeNeeded = 5
                }
            );
        }

        public void TaskComplete(CalendarTask task)
        {
            task.Completed = !task.Completed;
            CompletedTasks.Add(task);
            CalendarTasks.Remove(task);
        }

        public void taskChange(CalendarTask task)
        {

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
