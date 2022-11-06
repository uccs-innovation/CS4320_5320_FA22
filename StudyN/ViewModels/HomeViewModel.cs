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
        public ObservableCollection<DonutChartItem> HoursPieChartData { get; }
        public ObservableCollection<Appointment> CalendarEvents { get => GlobalAppointmentData.CalendarManager.Appointments; }
        public HomeViewModel()
        {
            TaskPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", 70),
                new DonutChartItem("Incomplete", 30)
            };

            HoursPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", 30),
                new DonutChartItem("Incomplete", 70)
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