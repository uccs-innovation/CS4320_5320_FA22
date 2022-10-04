using StudyN.Models;

namespace StudyN.Views;

//[QueryProperty(nameof(task), "task")]

public partial class TaskModifyPage : ContentPage
{
	CalendarTask Taskmod { get; set; }

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

		BindingContext = this;

		//var taskName = new Label();
		//taskName.SetBinding (Label.TextProperty, "Name");
		//taskName.BindingContext = taskMod;
    }

	private void saveChanges(object sender, EventArgs e)
	{

	}

	
}