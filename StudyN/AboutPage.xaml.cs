namespace MauiApp1;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

	private async void LearnMore_Clicked(object sender, EventArgs e)
	{
		// Navitgate to the specified URL in the system browser.
		await Launcher.Default.OpenAsync("https://aka.ms/maui");
	}
}