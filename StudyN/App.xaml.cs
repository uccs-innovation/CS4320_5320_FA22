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
            MainPage = new MainPage();

        }
    }
}
