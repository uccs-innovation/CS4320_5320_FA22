using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Appointment> CalendarEvents { get => GlobalAppointmentData.CalendarManager.Appointments; }
        public HomeViewModel()
        {
            Title = "Dashboard";
        }

        protected void RaisePropertyChanged(string name)
        {
            Console.WriteLine("property changed");
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}