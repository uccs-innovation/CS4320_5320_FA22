using StudyN.ViewModels;
using StudyN.Resources;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }

        private async void Button_ClickedThemes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ThemePage));
        }

        private async void Button_ClickedIcs(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddIcsPage));
        }
    }
}