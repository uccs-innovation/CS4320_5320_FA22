using StudyN.ViewModels;

namespace StudyN.Views;

public partial class SleepTimePage : ContentPage
{
	public SleepTimePage()
	{
		InitializeComponent();
		BindingContext = new SleepTimeViewModel();
	}
}