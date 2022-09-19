using System.Windows.Input;

namespace StudyN.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public const string ViewName = "SettingsPage";
        public SettingsViewModel()
        {
            Title = "Settings";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.devexpress.com/xamarin/"));
        }

        public ICommand OpenWebCommand { get; }
    }
}