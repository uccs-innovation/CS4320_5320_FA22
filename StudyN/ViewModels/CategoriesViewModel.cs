using StudyN.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.ViewModels
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get => GlobalAppointmentData.CalendarManager.AppointmentCategories; }
        public CategoriesViewModel()
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
