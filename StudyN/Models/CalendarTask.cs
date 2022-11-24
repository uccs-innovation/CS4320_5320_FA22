using StudyN.Views;
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
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TimeNeeded { get; set; }
        public int EstimateTime { get; set; }

        public CalendarTasksData Parent { get; set; }
    }

    public class CalendarTasksData
    {
        public string text = AddIcsPage.Result;
        public string eventName = "";
        public int id = 1;
        public string eventType = "";
        public string startDate = "";
        public string endDate = "";
        void GenerateCalendarTaskss()
        {
           

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
