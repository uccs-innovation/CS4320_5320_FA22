using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DonutChartItem> TaskPieChartData { get; }
        public ObservableCollection<Appointment> CalendarEvents { get => GlobalAppointmentData.CalendarManager.Appointments; }
        public HomeViewModel()
        {
            TaskPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", 50),
                new DonutChartItem("Incomplete", 50)
            };
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