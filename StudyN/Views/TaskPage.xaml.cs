using System;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views
{
    public partial class TaskPage : ContentPage
    {
        // Flag to prevent multiple child pages opening
        bool isChildPageOpening = false;

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

            Console.WriteLine("TaskPage initialized");

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

            //Ensuring that the long press menu is not yet viable
            ShowLongPressMenu(false);

        }

        protected override void OnAppearing()
        {
            isChildPageOpening = false;
        }

        //This function will by the cancel button to reset the selection menu to its
        //default state
        private void CancelButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();
                
                //Clearing the selected tasks and resetting the menu to its default
                //setting
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

        //This function will be used by the trash button to delete selected tasks
        private async void TrashButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();

                //We will first create a list of tasks before populating it with all
                //of the selected tasks
                List<Guid> taskIds = new List<Guid>();
                foreach (TaskItem task in selectedTasks)
                {
                    //If a task is being timed
                    if(GlobalTaskTimeData.TaskTimeManager.BeingTimed)
                    {
                        //If task being timed has the same id as the task in this list
                        if(task.TaskId == GlobalTaskTimeData.TaskTimeManager.TaskidBeingTimed)
                        {
                            //Alert Check
                            String alertstr = "Would you like to stop timing "
                            + GlobalTaskTimeData.TaskTimeManager.TaskName +
                            " and remove task?";
                            bool endtrack = await DisplayAlert("A Task You Selected Is Currently Being Timed", alertstr, "Yes", "No");
                            //if user wants to stop tracking old and start tracking new
                            if (endtrack)
                            {
                                GlobalTaskTimeData.TaskTimeManager.StopCurrent(DateTime.Now);
                                AlertUserOfTimeSpent();
                                taskIds.Add(task.TaskId);
                            }

                        } else {
                            taskIds.Add(task.TaskId);
                        }
                    } else {
                        taskIds.Add(task.TaskId);
                    }
                }

                //Sending the created list to TaskManager's DeleteListOfTasks function
                //for deletion
                GlobalTaskData.TaskManager.DeleteListOfTasks(taskIds);

                //Clearing the selected tasks and resetting the menu to its default
                //setting
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

        //This function will be used by the complete task button to "complete" a task
        private async void CompleteButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ToolbarItem toolbar = sender as ToolbarItem;
                ContentPage contentPage = toolbar.Parent as ContentPage;
                DataGridView gridView = contentPage.Content as DataGridView;

                gridView.BeginUpdate();

                // For each of our selected tasks, we will "complete" them using
                // TaskManager's CompleteTask function
                foreach (TaskItem task in selectedTasks)
                {
                    //If a task is being timed
                    if (GlobalTaskTimeData.TaskTimeManager.BeingTimed)
                    {
                        //If task being timed has the same id as the task in this list
                        if (task.TaskId == GlobalTaskTimeData.TaskTimeManager.TaskidBeingTimed)
                        {
                            //Alert Check
                            String alertstr = "Would you like to stop timing "
                            + GlobalTaskTimeData.TaskTimeManager.TaskName +
                            " and remove task?";
                            bool endtrack = await DisplayAlert("A Task You Selected Is Currently Being Timed", alertstr, "Yes", "No");
                            //if user wants to stop tracking old and start tracking new
                            if (endtrack)
                            {
                                GlobalTaskTimeData.TaskTimeManager.StopCurrent(DateTime.Now);
                                AlertUserOfTimeSpent();
                                GlobalTaskData.TaskManager.CompleteTask(task.TaskId);
                            }
                        }
                        else
                        {
                            GlobalTaskData.TaskManager.CompleteTask(task.TaskId);
                        }
                    } else {
                        GlobalTaskData.TaskManager.CompleteTask(task.TaskId);
                    }
                }

                //Clear the selected tasks and reset the menu back to the default
                //setting
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
            if (!isChildPageOpening)
            {
                isChildPageOpening = true;
                await Shell.Current.GoToAsync(nameof(TaskChartsPage));
            }
        }




        async void AlertUserOfTimeSpent()
        {
            //gets name of task being timed and time spent on task
            String taskname = GlobalTaskTimeData.TaskTimeManager.TaskName;
            int timespent = GlobalTaskTimeData.TaskTimeManager.taskitemtime.span.Minutes;
            //Informational alert and alert string.
            String alertstr = "You spent " + timespent + " minutes on task " + taskname;
            await DisplayAlert("Great Job!", alertstr, "OK");
        }

        //This function will be used to preform certain actions when a row is pressed
        //for a long amount of time
        private void RowLongPressed(object sender, DataGridGestureEventArgs e)
        {
            //First checking to ensure the the item we have selected is neither null
            //nor 
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

        //This function will be used to intiate editing a task upon touching a given
        //task
        private async void CellClicked(object sender, DataGridGestureEventArgs e)
        {
            
            if (e.Item != null && e.FieldName != "DueTime")
            {
                if (!isLongPressMenuVisible && !isChildPageOpening)
                {
                    isChildPageOpening = true;
                    // TaskItem we need to edit...
                    TaskItem task = (TaskItem)e.Item;
                    GlobalTaskData.ToEdit = task;
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
            if (!isChildPageOpening)
            {
                isChildPageOpening = true;
                GlobalTaskData.ToEdit = null;
                await Shell.Current.GoToAsync(nameof(AddTaskPage));
            }
        }

        //This function will be used to bring up more options on a long press
        void ShowLongPressMenu(bool setVisible)
        {
            //If the menu is already what we are trying to make it (ie we want it to
            //be viable, but it already is), return
            if (isLongPressMenuVisible == setVisible)
            {
                return;
            }

            //Set the menu to either inviable or visable
            isLongPressMenuVisible = setVisible;

            //Setting all of the visablility acorrding to what we want
            if (setVisible)
            {
                //If we want the tools to be available, clear the tool bar and add
                //the trash, complete, and cancel buttons
                ToolbarItems.Clear();
                ToolbarItems.Add(trashToolbarItem);
                ToolbarItems.Add(completeToolbarItem);
                ToolbarItems.Add(cancelToolbarItem);
            }
            else
            {
                //If we want to remove the tools, clear the tool bar and add back the
                //add task button
                ToolbarItems.Clear();
                ToolbarItems.Add(addToolbarItem);
            }
            ToolbarItems.Add(chartToolbarItem);
        }

        //This function will be used to change the color of a selected task
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

        // Method to calculate percent completion for all tasks in the Data Grid
        private void CalculateTotalPercent(object sender, CustomSummaryEventArgs e)
        {
            // Gets the data grid as an object
            DataGridView dataGrid = this.Content as DataGridView;

            // Gets the number of rows in dataGrid
            int rowCount = dataGrid.RowCount;

            // Variables to store data in
            double totalHoursWorked = 0;
            double totalHoursNeeded = 0;

            // Runs for each row in dataGrid
            for (int i = 0; i < rowCount; i++)
            {
                // Gets hours worked and needed for TaskItem in row
                TaskItem row = (TaskItem)dataGrid.GetItem(i);
                totalHoursWorked = totalHoursWorked + row.CompletionProgress;
                totalHoursNeeded = totalHoursNeeded + row.TotalTimeNeeded;
            }

            // Runs if there are assignments with hours needed to complete
            if (totalHoursNeeded > 0)
            {
                // Calculates total percent completion
                double totalPercentComplete = (totalHoursWorked / totalHoursNeeded) * 100;
                e.TotalValue = Math.Round(totalPercentComplete);

            }
            else
            {
                e.TotalValue = 0;
            }
        }
    }
}