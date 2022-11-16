using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views;

public partial class SleepTimePage : ContentPage
{
	public SleepTimePage()
	{
		InitializeComponent();
		BindingContext = new SleepTimeViewModel();
		LoadSleepTime();
	}

	/// <summary>
	/// When save button is clicked, save the input and go back to calendar page
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void OnSaveButtonTap(object sender, EventArgs e)
	{
		if(this.startTime != null && this.endTime != null)
		{
			// if the start time and end time are inputed save them
			DateTime fullStartTime = new DateTime(0000, 00, 00, this.startTime.Time.Value.Hour, 
				this.startTime.Time.Value.Minute, this.startTime.Time.Value.Second);
			DateTime fullEndTime = new DateTime(0000, 00, 00, this.endTime.Time.Value.Hour,
				this.endTime.Time.Value.Minute,this.endTime.Time.Value.Second);
			GlobalAppointmentData.CalendarManager.SaveSleepTime(fullStartTime, fullEndTime);
		}
		// get out of Sleep Time Page
		Routing.RegisterRoute(nameof(Views.CalendarPage), typeof(Views.CalendarPage));
		await Shell.Current.GoToAsync("..");
	}
	
	/// <summary>
	/// Loads the sleep time object if it isn't null
	/// </summary>
	void LoadSleepTime()
	{
		// load sleep time start and stop times if they exist
		if(File.Exists(FileSystem.AppDataDirectory + "/sleepTime.json")) {
			this.startTime.Time = GlobalAppointmentData.CalendarManager.SleepTime.StartTime;
			this.endTime.Time = GlobalAppointmentData.CalendarManager.SleepTime.EndTime;
		}
	}
}