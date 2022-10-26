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
        public ObservableCollection<AppointmentCategory> AppointmentCategories { get; private set; }

        void SetCategoriesList()
        {
            // make sure uncatigorized doesn't get into Categories page
            for(int i = 0; i < GlobalAppointmentData.CalendarManager.AppointmentCategories.Count - 1; i++)
            {
                AppointmentCategories.Add(GlobalAppointmentData.CalendarManager.AppointmentCategories[i]);
            }
        }
        public CategoriesViewModel()
        {
            AppointmentCategories = new ObservableCollection<AppointmentCategory>();
            SetCategoriesList();
        }
    }
}
