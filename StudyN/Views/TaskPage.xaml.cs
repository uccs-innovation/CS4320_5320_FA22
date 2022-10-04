using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace StudyN.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public TaskPage()
        {
            InitializeComponent();
        }

        async private void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            if(e.Item != null && e.FieldName == "Completed")
            {
                DataGridView gridView = (DataGridView)sender;
                gridView.BeginUpdate();
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task);
                gridView.EndUpdate();
            }
            else if (e.Item != null)
            {
                CalendarTask task = (CalendarTask)e.Item;

                var navigationParameter = new Dictionary<string, object>
                {
                   { "taskmod", task }
                };

                //Navigation.PushAsync(TaskModifyPage(task));
                await Shell.Current.GoToAsync($"TaskModifyPage", navigationParameter);
                //this.Frame.Navigate(typeof(TaskModifyPage), task);
                //await Shell.Current.GoToAsync(nameof(TaskModifyPage));
                //Navigation.PushAsync(new TaskModifyPage(task));
            }
        }
    }
}