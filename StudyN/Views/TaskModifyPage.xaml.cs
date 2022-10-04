using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views;

//[QueryProperty(nameof(task), "task")]

public partial class TaskModifyPage : ContentPage
{
	public CalendarTask Taskmod = new CalendarTask("Joe");

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Taskmod = query["taskmod"] as CalendarTask;
        OnPropertyChanged("taskmod");
    }

	

    //public CalendarTask GetCalendarTask()
	//{

	//}
	
	public TaskModifyPage()
	{
		InitializeComponent();

		//var taskName = new Label();
		//taskName.SetBinding (Label.TextProperty, "Name");
		//taskName.BindingContext = taskMod;
    }

	async private void saveChanges(object sender, EventArgs e)
	{
        Routing.RegisterRoute(nameof(Views.TaskPage), typeof(Views.TaskPage));
        await Shell.Current.GoToAsync(nameof(Views.TaskPage));
    }

	
}