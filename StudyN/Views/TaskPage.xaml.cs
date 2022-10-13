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

        private void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            /*if (e.Item != null && e.FieldName == "Completed")
            {
                DataGridView gridView = (DataGridView)sender;
                gridView.BeginUpdate();
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task);
                gridView.EndUpdate();
            }*/
            if (e.Item != null)
            {
                var editForm = new EditFormPage(grid, grid.GetItem(e.RowHandle));
                Navigation.PushAsync(editForm);
            }

        }

    }
}