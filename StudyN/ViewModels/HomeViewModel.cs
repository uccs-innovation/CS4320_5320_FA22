using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DonutChartItem> TaskPieChartData { get; set; }
        public ObservableCollection<DonutChartItem> HoursPieChartData { get; set; }
        public ObservableCollection<Appointment> CalendarEvents { get => GlobalAppointmentData.CalendarManager.Appointments; }
        public HomeViewModel()
        {
            SetTaskPercentage(0);
            SetHourPercentage(0);
        }

        public void SetTaskPercentage(double percent)
        {
            TaskPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", (int)percent),
                new DonutChartItem("Incomplete", 100-(int)percent)
            };
            RaisePropertyChanged(nameof(TaskPieChartData));
        }

        public void SetHourPercentage(double percent)
        {
            HoursPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", (int)percent),
                new DonutChartItem("Incomplete", 100-(int)percent)
            };
            RaisePropertyChanged(nameof(HoursPieChartData));
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