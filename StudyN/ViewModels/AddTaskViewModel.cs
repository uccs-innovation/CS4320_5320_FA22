
using StudyN.Models;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels;

public class AddTaskViewModel : BaseViewModel
{
    public ObservableCollection<AppointmentCategory> appointmentCategories { get => GlobalAppointmentData.CalendarManager.AppointmentCategories; }
    public List<string> categoryStrings;
    public AddTaskViewModel()
	{
        //intitailize category strings with appointment category captions
        categoryStrings = new List<string>();
        foreach (AppointmentCategory category in appointmentCategories)
        {
            categoryStrings.Add(category.Caption);
        }
    }
}

public partial class EditTaskViewModel : AddTaskViewModel
{
    public EditTaskViewModel()
	{
    }
}


