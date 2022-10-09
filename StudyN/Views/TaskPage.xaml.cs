using System;
using System.Threading.Tasks;
using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace StudyN.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        bool isLongPressMenuVisible = true;
        ToolbarItem addToolbarItem;
        ToolbarItem cancelToolbarItem;
        ToolbarItem trashToolbarItem;
        ToolbarItem completeToolbarItem;
        ToolbarItem chartToolbarItem;

        HashSet<TaskItem> selectedTasks;
        HashSet<int> rowHandleList;
        public TaskPage()
        {
            InitializeComponent();

            selectedTasks = new HashSet<TaskItem>();
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
                    case "Chart":
                        chartToolbarItem = item;
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
                foreach (TaskItem task in selectedTasks)
                { 
                    task.Parent.DeleteTask(task.TaskId);
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
                foreach (TaskItem task in selectedTasks)
                {
                    task.Parent.CompleteTask(task.TaskId);
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

        // Bringing up the Task Metrics Visualization Page
        private async void TaskChartsButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(TaskChartsPage));
        }

        private void RowLongPressed(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null && e.FieldName != "DueTime")
            {
                TaskItem task = e.Item as TaskItem;
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
                if (selectedTasks.Count > 0)
                {
                    ShowLongPressMenu(true);
                }
                else
                {
                    ShowLongPressMenu(false);
                }

                gridView.EndUpdate();
            }
        }

        private async void CellClicked(object sender, DataGridGestureEventArgs e)
        {

            if (e.Item != null && e.FieldName != "DueTime")
            {
                if (!isLongPressMenuVisible)
                {
                    // TaskItem we need to edit...
                    TaskItem task = (TaskItem)e.Item;
                    UIGlobal.ToEdit = task;
                    // Get it in here
                    await Shell.Current.GoToAsync(nameof(AddTaskPage));
                }
                else
                {
                    RowLongPressed(sender, e);
                }
            }
        }

        //Function for the add task button to bring to new task page
        private async void AddButtonClicked(object sender, EventArgs e)
        {
            UIGlobal.ToEdit = null;
            await Shell.Current.GoToAsync(nameof(AddTaskPage));
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
            ToolbarItems.Add(chartToolbarItem);
        }

        private void HighlightSelectedRows(object sender, CustomCellStyleEventArgs e)
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