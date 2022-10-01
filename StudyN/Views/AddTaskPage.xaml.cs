namespace StudyN.Views;
using StudyN.Models;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage()
	{
		InitializeComponent();
	}

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        displayLabel.Text = String.Format("Priority");
    }

    private async void AddTask(object sender, EventArgs e)
    {
        
        Routing.RegisterRoute(nameof(Views.AddTaskPage), typeof(Views.AddTaskPage));
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }
}