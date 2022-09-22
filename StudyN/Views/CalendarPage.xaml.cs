using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new CalendarViewModel();
        }

        CalendarViewModel ViewModel { get; }

        void OnDailyClicked(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("test1");
        }

        void OnWeeklyClicked(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("test2");
        }

        void OnMonthlyClicked(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("test3");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}