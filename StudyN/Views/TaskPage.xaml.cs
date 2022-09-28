using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public TaskPage()
        {
            InitializeComponent();
            BindingContext = new TaskDataViewModel();
        }

        private void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            DataGridView gridView = (DataGridView)sender;
            if (e.Item != null && e.FieldName == "Completed")
            {
                gridView.BeginUpdate();
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task);
                gridView.EndUpdate();
            }
            /*else if (e.Item != null && e.FieldName != "Completed")
            {
                //for transitioning to detail page
                await Shell.Current.GoToAsync(nameof(TaskDetailPage));
            }*/
        }
    }
}