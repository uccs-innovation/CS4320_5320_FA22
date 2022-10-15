using StudyN.ViewModels;

namespace StudyN.Views
{

    public partial class AddEventPage: ContentPage
    {
        public DateTime? Time { get; set; }
        public AddEventPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new AddEventViewModel();
        }

        AddEventViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}