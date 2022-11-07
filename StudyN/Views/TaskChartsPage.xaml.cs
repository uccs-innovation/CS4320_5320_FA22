using StudyN.ViewModels;

namespace StudyN.Views;

public partial class TaskChartsPage : ContentPage
{

    public TaskChartsPage()
    {
        InitializeComponent();
        BindingContext = new TaskChartsViewModel();
    }
}
