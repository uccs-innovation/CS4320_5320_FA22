using DevExpress.Maui.DataGrid;
using Java.Nio.FileNio;
using Microsoft.Maui.Controls;
using StudyN.Models;
using StudyN.ViewModels;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        ToolbarItem addTaskToolbarItem;

        public TaskPage()
        {
            InitializeComponent();

            foreach(ToolbarItem item in ToolbarItems)
            {
                if(item.Text == "Add")
                {
                    addTaskToolbarItem = item;
                }
            }

            ShowLongPressMenu(false);
        }
        private async void AddButtonClicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync(nameof(AddEventPage));
        }
        private void RowLongPressed(object sender, DataGridGestureEventArgs e)
        {
            //DataGridView gridView = sender as DataGridView;
            //TaskDataViewModel model = gridView.BindingContext as TaskDataViewModel;
            //model.isLongPressMenuEnabled = true;
            ShowLongPressMenu(true);
        }

        private void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            if(e.Item != null && e.FieldName == "Completed")
            {
                DataGridView gridView = (DataGridView)sender;
                gridView.BeginUpdate();
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task);
                gridView.EndUpdate();
            }
        }

        void ShowLongPressMenu(bool setVisible)
        {
            if(setVisible)
            {
                ToolbarItems.Clear();
                ToolbarItems.Add(addTaskToolbarItem);
            }
            else
            {
                ToolbarItems.Clear();
            }
        }
    }
}