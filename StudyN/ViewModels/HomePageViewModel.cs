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

        public void GetDailyList()
        {
            foreach (Appointment app in ApptList.ToList())
            {
                Console.WriteLine(app.Subject);
                Console.WriteLine(app.End.Date.ToString());
                Console.WriteLine(DateTime.Now.Date.ToString());
                Console.WriteLine(app.End.Date.ToString() != DateTime.Now.Date.ToString());
                if (app.End.Date.ToString() != DateTime.Now.Date.ToString())
                {
                    ApptList.Remove(app);
                }
            }

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