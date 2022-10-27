using DevExpress.Maui.Charts;
using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalyticsPage : ContentPage
    {
        public AnalyticsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new AnalyticsViewModel();
        }

        AnalyticsViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}