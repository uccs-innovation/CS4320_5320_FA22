using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage: ContentPage
    {
        public AddEventPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new AddEventViewModel();
            ViewModel.OnAppearing();
        }

        AddEventViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}