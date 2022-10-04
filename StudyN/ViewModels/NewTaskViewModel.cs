using System.Globalization;
using System.ComponentModel.DataAnnotations;
using DevExpress.Maui.DataForm;

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
        private DateTime finalDueDate;
        private Priority priority;
        

        

        // intialize values
        public NewTaskViewModel()
        {
            name = null;
            priority = Priority.Normal;
            description = null;
            timeNeeded = 0;
            dueDate = DateTime.Today.AddHours(24);
            dueTime = DateTime.Today.AddHours(24);
            BackCommand = new Command(OnClickBack);
            SaveCommand = new Command(OnClickSave);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public virtual Priority NewPriority
        {
            get => priority;
            set => SetProperty(ref priority, value);
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
        
        public Command BackCommand { get; }
        public Command SaveCommand { get; }
        
        private void MapDueDate()
        {
            finalDueDate = DateTime.Parse(dueDate.ToString("yyyy-MM-dd") + " " + dueTime.TimeOfDay.ToString());
        }

        private bool Validation()
        {
            if(name == null)
            {
                return false;
            }else if (description == null)
            {
                return false;
            }else if (timeNeeded <= 0)
            {
                return false;
            }
            else
                return true;
        }

        // when data is able to get saved to a database, this will, and bring back to task page
        async void OnClickSave(object sender)
        {
            // validate before saving
            if (Validation() == true)
            {
                /*// save data
                MapDueDate();
                var newTask = new CalendarTask()
                {
                    Id = CalendarTask.LastId() + 1,
                    Completed = false,
                    Name = Name,
                    Priority = NewPriority,
                    Description = Description,
                    TimeNeeded = TimeNeeded,
                    DueDate = finalDueDate
                };
                */
                // go back to task page
                Routing.RegisterRoute(nameof(Views.TaskPage), typeof(Views.TaskPage));
                await Shell.Current.GoToAsync(nameof(Views.TaskPage));
            }
        }
        
        async void OnClickBack(object sender)
        {
            Routing.RegisterRoute(nameof(Views.TaskPage), typeof(Views.TaskPage));
            await Shell.Current.GoToAsync(nameof(Views.TaskPage));
        }
    }
}