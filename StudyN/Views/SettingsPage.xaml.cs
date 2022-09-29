using StudyN.ViewModels;
using StudyN.Resources;
using StudyN.Services;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private NavigationService _navigationService;
        public SettingsPage()
        {
            InitializeComponent();
            _navigationService = new NavigationService();
            BindingContext = ViewModel = new SettingsViewModel();
        }

        SettingsViewModel ViewModel { get; }

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

        void OnImportClicked(object sender, EventArgs e)
        {
            ViewModel.OnImport();
        }
    }
}