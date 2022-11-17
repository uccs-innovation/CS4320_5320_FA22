using StudyN.ViewModels;
using StudyN.Resources;
using StudyN.Utilities;
using StudyN.Models;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        string dirString = "";
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }

        private void Button_ClickedDark(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new Dictionary2());
            }
        }

        private void Entry_DirPath(object sender, TextChangedEventArgs e)
        {
            dirString = e.NewTextValue;
        }

        private void Button_ClickedLoad(object sender, EventArgs e)
        {
            GlobalTaskData.TaskManager.LoadFilesIntoListsTest(dirString);
        }

        private void Button_ClickedLight(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new Dictionary1());
            }
        }

        private async void Button_ClickedIcs(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddIcsPage));
        }

        private async void Button_ClickedSleep(object sender, EventArgs e)
        {
            Routing.RegisterRoute(nameof(Views.SleepTimePage), typeof(Views.SleepTimePage));
            await Shell.Current.GoToAsync(nameof(Views.SleepTimePage));
        }
    }
}