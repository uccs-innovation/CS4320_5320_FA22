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
            Routing.RegisterRoute(nameof(Views.AddTaskPage), typeof(Views.AddTaskPage));

            Routing.RegisterRoute(typeof(AddEventPage).FullName, typeof(AddEventPage));
        }
    }
}
