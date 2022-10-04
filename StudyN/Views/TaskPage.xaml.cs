using Android.Widget;
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
            //DataGridView gridView = (DataGridView)sender;
            //gridView.BeginUpdate();
            //gridView.EndUpdate();
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
                int id = task.Id;
                CalendarTask taskmod = new CalendarTask(id);

                var navigationParameter = new Dictionary<string, object>
                {
                   { "taskmod", taskmod }
                };

                Routing.RegisterRoute(nameof(Views.TaskModifyPage), typeof(Views.TaskModifyPage));
                //Navigation.PushAsync(TaskModifyPage(task));
                await Shell.Current.GoToAsync($"TaskModifyPage", navigationParameter);
                //this.Frame.Navigate(typeof(TaskModifyPage), task);
                //await Shell.Current.GoToAsync(nameof(Views.TaskModifyPage));
                //Navigation.PushAsync(new TaskModifyPage(task));
            }
        }
    }
}