using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new HomeViewModel();
            ViewModel.OnAppearing();
        }

        HomeViewModel ViewModel { get; }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        void OnImportClicked(object sender, EventArgs e)
        {
            ViewModel.OnImport();
        }

        void OnLoadClicked(object sender, EventArgs e)
        {
            ViewModel.OnLoad();
        }
    }
}