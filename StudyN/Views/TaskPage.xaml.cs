using System;
using DevExpress.Maui.DataGrid;
using StudyN.Models;

namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        bool isLongPressMenuVisible = true;
        ToolbarItem cancelToolbarItem;
        ToolbarItem trashToolbarItem;
        ToolbarItem completeToolbarItem;

        HashSet<CalendarTask> selectedTasks;
        HashSet<int> rowHandleList;
        public TaskPage()
        {
            InitializeComponent();

            selectedTasks = new HashSet<CalendarTask>();
            rowHandleList = new HashSet<int>();

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
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();

                selectedTasks.Clear();
                rowHandleList.Clear();
                ShowLongPressMenu(false);

                gridView.EndUpdate();
            }
            catch(NullReferenceException execption)
            {
                Console.WriteLine(execption.Message);
            }
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
            DataGridView gridView = sender as DataGridView;

            gridView.BeginUpdate();

            // Add/Remove from list as needed
            if (selectedTasks.Contains(task))
            {
                selectedTasks.Remove(task);
                rowHandleList.Remove(e.RowHandle);
            }
            else
            {
                selectedTasks.Add(task);
                rowHandleList.Add(e.RowHandle);
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

            gridView.EndUpdate();
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

        private void DataGridView_CustomCellStyle(object sender, CustomCellStyleEventArgs e)
        {
            if(rowHandleList.Contains(e.RowHandle))
            {
                e.BackgroundColor = Color.FromArgb("#d9f0fe");
            }
            else
            {
                e.BackgroundColor = Color.FromArgb("#FFFFFF");
            }
            
        }
    }
}