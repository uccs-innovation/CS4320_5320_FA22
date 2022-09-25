using DevExpress.Maui.DataGrid;
using Java.Util;
using Microsoft.Maui.Controls;
using StudyN.Models;
using StudyN.ViewModels;


namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        bool isLongPressMenuVisible;
        ToolbarItem cancelToolbarItem;
        ToolbarItem trashToolbarItem;
        ToolbarItem completeToolbarItem;

        HashSet<CalendarTask> selectedTasks;

        public TaskPage()
        {
            InitializeComponent();

            selectedTasks = new HashSet<CalendarTask>();

            foreach (ToolbarItem item in ToolbarItems)
            {
                if(item.Text == "Trash")
                {
                    trashToolbarItem = item;
                }
                else if(item.Text == "Cancel")
                {
                    cancelToolbarItem = item;
                }
                else if(item.Text == "Complete")
                {
                    completeToolbarItem = item;
                }
            }

            ShowLongPressMenu(false);
        }
        private void CancelButtonClicked(object sender, EventArgs e)
        {
            selectedTasks.Clear();
            ShowLongPressMenu(false);
        }

        private void TrashButtonClicked(object sender, EventArgs e)
        {
            // Hookup when available
        }
        private void CompleteButtonClicked(object sender, EventArgs e)
        {
            // Hookup when available
        }
        private void RowLongPressed(object sender, DataGridGestureEventArgs e)
        {
            CalendarTask task = e.Item as CalendarTask;

            // Add/Remove from list as needed
            if(selectedTasks.Contains(task))
            {
                selectedTasks.Remove(task);
            }
            else
            {
                selectedTasks.Add(task);
            }

            // Display based on number of items selected
            if(selectedTasks.Count > 0)
            {
                ShowLongPressMenu(true);
            }
            else
            {
                ShowLongPressMenu(false);
            }
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
            if (isLongPressMenuVisible == setVisible)
            {
                return;
            }

            isLongPressMenuVisible = setVisible;
            if (setVisible)
            {
                ToolbarItems.Clear();
                ToolbarItems.Add(trashToolbarItem);
                ToolbarItems.Add(completeToolbarItem);
                ToolbarItems.Add(cancelToolbarItem);
            }
            else
            {
                ToolbarItems.Clear();
            }
        }
    }
}