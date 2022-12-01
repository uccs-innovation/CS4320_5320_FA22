using DevExpress.XtraRichEdit.Fields;
using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<TaskItem> TaskList { get => GlobalTaskData.TaskManager.TaskList; }
        public ObservableCollection<Appointment> ApptList { get => GlobalAppointmentData.CalendarManager.Appointments; }

        public HomePageViewModel()
        {
            
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