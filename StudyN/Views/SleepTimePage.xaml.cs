using StudyN.ViewModels;

namespace StudyN.Views;

public partial class SleepTimePage : ContentPage
{
	public SleepTimePage()
	{
		InitializeComponent();
		BindingContext = new SleepTimeViewModel();
	}

	/// <summary>
	/// When save button is clicked, save the input and go back to calendar page
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void OnSaveButtonTap(object sender, EventArgs e)
	{
		Routing.RegisterRoute(nameof(Views.CalendarPage), typeof(Views.CalendarPage));
		await Shell.Current.GoToAsync("..");
	}
}