using DevExpress.Maui.DataGrid;
using StudyN.Models;

namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        public TaskPage()
        {
            InitializeComponent();
        }

        private void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            if(e.Item != null && e.FieldName == "Completed")
            {
                DataGridView gridView = (DataGridView)sender;
                gridView.BeginUpdate();

                // Update task
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task.TaskId);

                gridView.EndUpdate();
            }
        }

        //Function for the add task button to bring to new task page
        
        private async void AddButtonClicked(object sender, EventArgs e) {
            await Shell.Current.GoToAsync(nameof(AddTaskPage));
        }

    }
}