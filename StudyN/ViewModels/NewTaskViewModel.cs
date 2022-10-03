using System.Globalization;

namespace StudyN.ViewModels
{
    public class NewTaskViewModel : BaseViewModel
    {
        // for priority
        public enum Priority { Urgent, High, Normal, Low}
        //fields
        private string name;
        private string description;
        private int timeNeeded;
        private DateTime dueDate;
        private DateTime dueTime;
        private Priority priority;

        

        // intialize values
        public NewTaskViewModel()
        {
            name = null;
            description = null;
            timeNeeded = 0;
            dueDate = DateTime.Today.AddHours(24);
            dueTime = DateTime.Today.AddHours(24);
            priority = Priority.Normal;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public int TimeNeeded
        {
            get => timeNeeded;
            set => SetProperty(ref timeNeeded, value);
        }

        public DateTime DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }

        public DateTime DueTime
        {
            get => dueTime;
            set => SetProperty(ref dueTime, value);
        }

        public virtual Priority NewPriority
        {
            get => priority;
            set => SetProperty(ref priority, value);
        }
    }
}