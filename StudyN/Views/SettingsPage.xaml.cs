using StudyN.ViewModels;
using StudyN.Resources;
using StudyN.Models;

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

        private void Button_ClickedDebug(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new Dictionary3());
            }
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

        private void Button_ClickedLight(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new Dictionary1());
            }
        }

        //This button sends the user to the About page
        private async void Button_ClickedAbout(object sender, EventArgs e)
        {
            GlobalTaskData.ToEdit = null;
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }
        
    }
}