﻿using System;
using System.Threading.Tasks;
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

        HashSet<ListTask> selectedTasks;
        HashSet<int> rowHandleList;
        public TaskPage()
        {
            InitializeComponent();

            selectedTasks = new HashSet<ListTask>();
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
                foreach (ListTask task in selectedTasks)
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
                foreach (ListTask task in selectedTasks)
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


        private void RowLongPressed(object sender, DataGridGestureEventArgs e)
        {
            ListTask task = e.Item as ListTask;
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

        private async void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            // Task we need to edit...
            ListTask task = (ListTask)e.Item;
            UIGlobal.ToEdit = task;
            // Get it in here
            await Shell.Current.GoToAsync(nameof(AddTaskPage));
            task.Parent.RemoveTask(task.TaskId);
        }

        //Function for the add task button to bring to new task page
        
        private async void AddButtonClicked(object sender, EventArgs e) {
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