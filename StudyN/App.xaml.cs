using StudyN.Views;
using Application = Microsoft.Maui.Controls.Application;

namespace StudyN
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Routing.RegisterRoute(nameof(Views.TaskPage), typeof(Views.TaskPage));
        }
    }
}
