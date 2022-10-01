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

            Routing.RegisterRoute(typeof(AddEventPage).FullName, typeof(AddEventPage));
            Routing.RegisterRoute(typeof(TaskPage).FullName, typeof(TaskPage));
        }
    }
}
