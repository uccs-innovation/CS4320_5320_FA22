using StudyN.Services;
using StudyN.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace StudyN
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<NavigationService>();
            Routing.RegisterRoute(typeof(ImportCalPage).FullName, typeof(ImportCalPage));
            Routing.RegisterRoute(typeof(SettingsPage).FullName, typeof(SettingsPage));
            Routing.RegisterRoute(typeof(EventDataGridPage).FullName, typeof(EventDataGridPage));   
            MainPage = new MainPage();

        }
    }
}
