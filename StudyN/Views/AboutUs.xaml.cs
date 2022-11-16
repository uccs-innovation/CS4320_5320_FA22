namespace StudyN.Views;

public partial class AboutUs: ContentPage
{
    public AboutUs()
    {
        InitializeComponent();
    }
    public async void Button_clicked(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new HomePage());
    }
    public async void Button_clicked1(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new CalendarPage());
    }
    public async void Button_clicked2(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new TaskPage());
    }
    public async void Button_clicked3(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new AnalyticsPage());
    }
    public async void Button_clicked4(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new CategoriesPage());
    }
    public async void Button_clicked5(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

}