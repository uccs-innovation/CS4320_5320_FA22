using StudyN.ViewModels;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTaskPage : ContentPage
    {
        public NewTaskPage()
        {
            InitializeComponent();
            BindingContext = new NewTaskViewModel();
        }

        private async void OnClickBack(object sender, EventArgs e)
        {
            Routing.RegisterRoute(nameof(Views.TaskPage), typeof(Views.TaskPage));
            await Shell.Current.GoToAsync(nameof(Views.TaskPage));
        }
    }
}