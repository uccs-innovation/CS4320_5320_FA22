using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace StudyN.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExportCal : ContentPage
    {
        public ExportCal()
        {
            InitializeComponent();
        }

        private void CellClicked(object sender, DataGridGestureEventArgs e)
        {

            if (e.Item != null)
            {
                var editForm = new EditFormPage(grid, grid.GetItem(e.RowHandle));
                Navigation.PushAsync(editForm);
            }

        }

    }
}