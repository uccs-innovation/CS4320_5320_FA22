using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        public HomePage()
        {
            InitializeComponent();
            DateFilter.AutoFilterValue = DateTime.Today;
            BindingContext = ViewModel = new HomeViewModel();
            ViewModel.OnAppearing();
        }

        HomeViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
            DateFilter.AutoFilterValue = DateTime.Today;
        }
    }
}