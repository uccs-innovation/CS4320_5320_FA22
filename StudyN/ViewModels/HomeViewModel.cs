using DevExpress.XtraRichEdit.Fields;
using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        const int COMPLETE_INDEX = 0;
        const int INCOMPLETE_INDEX = 1;
        const int MIN_PERCENTAGE = 0;
        const int MAX_PERCENTAGE = 100;

        public ObservableCollection<DonutChartItem> TaskPieChartData { get; set; }
        public ObservableCollection<DonutChartItem> HoursPieChartData { get; set; }
        public ObservableCollection<Appointment> CalendarEvents { get => GlobalAppointmentData.CalendarManager.Appointments; }
        public HomeViewModel()
        {
            TaskPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", MIN_PERCENTAGE),
                new DonutChartItem("Incomplete", MAX_PERCENTAGE)
            };

            HoursPieChartData = new ObservableCollection<DonutChartItem>() {
                new DonutChartItem("Complete", MIN_PERCENTAGE),
                new DonutChartItem("Incomplete", MAX_PERCENTAGE)
            };
        }

        public void SetTaskPercentage(double percent)
        {
            TaskPieChartData[COMPLETE_INDEX].Percentage = (int)percent;
            TaskPieChartData[INCOMPLETE_INDEX].Percentage = MAX_PERCENTAGE - (int)percent;
            RaisePropertyChanged(nameof(TaskPieChartData));
        }

        public void SetHourPercentage(double percent)
        {
            HoursPieChartData[COMPLETE_INDEX].Percentage = (int)percent;
            HoursPieChartData[INCOMPLETE_INDEX].Percentage = MAX_PERCENTAGE - (int)percent;
            RaisePropertyChanged(nameof(HoursPieChartData));
        }

        protected void RaisePropertyChanged(string name)
        {
            Console.WriteLine("HomeViewModel property changed, name = " + name);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}