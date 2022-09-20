using DevExpress.Maui.DataGrid;
using StudyN.ViewModels;

namespace StudyN.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        TaskDataViewModel viewModel;
        public TaskPage()
        {
            InitializeComponent();
            viewModel = new TaskDataViewModel();
            BindingContext = viewModel.data;
        }

    }
}