using StudyN.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get => GlobalAppointmentData.CalendarManager.AppointmentCategories; }
        public CategoriesViewModel()
        {

        }
    }
}
