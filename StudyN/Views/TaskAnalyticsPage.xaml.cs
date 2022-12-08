using StudyN.ViewModels;

namespace StudyN.Views;

public partial class TaskAnalyticsPage: ContentPage
{

    public TaskAnalyticsPage()
    {
        InitializeComponent();
        BindingContext = new TaskAnalyticsViewModel();
    }
}
