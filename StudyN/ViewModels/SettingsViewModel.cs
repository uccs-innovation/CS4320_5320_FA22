using StudyN.Services;

namespace StudyN.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public const string ViewName = "SettingsPage";
        NavigationService NavigationService { get; set; }
        public SettingsViewModel()
        {
            Title = "Settings";
            NavigationService = new NavigationService();
        }

        async public void OnImport()
        {
            await NavigationService.NavigateToAsync<ImportCalViewModel>();
        }
    }
}