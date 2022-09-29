using StudyN.ViewModels;

namespace StudyN.Views;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventDataGridPage : ContentPage
{
	public EventDataGridPage()
	{
		InitializeComponent();
		BindingContext = ViewModel = new EventDataGridViewModel();
	}

	EventDataGridViewModel ViewModel { get; }

	protected override void OnAppearing()
	{
		base.OnAppearing();
		ViewModel.OnAppearing();
	}
}