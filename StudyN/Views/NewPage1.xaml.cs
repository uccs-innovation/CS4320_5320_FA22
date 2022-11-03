namespace StudyN.Views;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}
    public async void Button_clicked(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new HomePage());
    }
}