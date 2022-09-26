using System;
using DevExpress.Maui.DataGrid;
using StudyN.Models;

namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        bool isLongPressMenuVisible = true;
        ToolbarItem addToolbarItem;
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
                switch(item.Text)
                {
                    case "Add":
                        addToolbarItem = item;
                        break;
                    case "Trash":
                        trashToolbarItem = item;
                        break;
                    case "Cancel":
                        cancelToolbarItem = item;
                        break;
                    case "Complete":
                        completeToolbarItem = item;
                        break;
                    default:
                        break;
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
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();

                // Delete tasks
                foreach(CalendarTask task in selectedTasks)
                {
                    task.Parent.TaskDelete(task.TaskId);
                }

                selectedTasks.Clear();
                rowHandleList.Clear();
                ShowLongPressMenu(false);

                gridView.EndUpdate();
            }
            catch (NullReferenceException execption)
            {
                Console.WriteLine(execption.Message);
            }
        }
        private void CompleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();

                // Delete tasks
                foreach (CalendarTask task in selectedTasks)
                {
                    task.Parent.TaskComplete(task.TaskId);
                }

                selectedTasks.Clear();
                rowHandleList.Clear();
                ShowLongPressMenu(false);

                gridView.EndUpdate();
            }
            catch (NullReferenceException execption)
            {
                Console.WriteLine(execption.Message);
            }
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

                // Update task
                CalendarTask task = (CalendarTask)e.Item;
                task.Parent.TaskComplete(task.TaskId);

                gridView.EndUpdate();
            }
        }

        //Function for the add task button to bring to new task page
        
        private async void AddButtonClicked(object sender, EventArgs e)
        { 
            await Shell.Current.GoToAsync(nameof(AddEventPage));
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
                ToolbarItems.Add(addToolbarItem);
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