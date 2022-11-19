namespace StudyN.Views;

public partial class AboutUs: ContentPage
{
    public AboutUs()
    {
        InitializeComponent();
    }
    public async void Button_clicked(object sender, EventArgs args)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }
    
}