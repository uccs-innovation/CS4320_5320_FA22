namespace StudyN.Views;

using Java.Security;
using Microsoft.Maui.Animations;
using StudyN.Models;
using StudyN.Utilities;
using StudyN.ViewModels;
using static Android.Util.EventLogTags;

public partial class AddTaskPage : ContentPage
{
    bool editingExistingTask;
    AutoScheduler autoScheduler;

    //initialize add task page
	public AddTaskPage()
	{
		InitializeComponent();
        autoScheduler = new AutoScheduler(GlobalTaskData.TaskManager.TaskList);

        //This will check if we are editing an existing task or making a new one. We will know this based on if ToEdit is null or not
        if (GlobalTaskData.ToEdit != null)
        {
            //If we are editing, we need to set the title and load in the values of the task
            Title = "Edit Task";
            LoadValues();
            BindingContext = new EditTaskViewModel();
            editingExistingTask = true;
        }
        else
        {
            //If we are just creating a new task, we need to set the title and set the time and date so they are not null
            Title = "Add Task";
            editingExistingTask = false;
            SetValues();
        }

        //If we are editing a task, the delete and edit buttons will be visable. If not, then invisable
        DeleteTaskButton.IsVisible = editingExistingTask;
        CompleteTaskButton.IsVisible = editingExistingTask;

        //Makes timer button visible
        TimerButton.IsVisible = editingExistingTask;
        //checks text of timer button. If it's not being tracked we want to see 
        //track task. Otherwise we want to see stop tracking
        if (editingExistingTask)
        {
            Guid currenttaskid = GlobalTaskData.ToEdit.TaskId;
            Guid taskbeingtimed = GlobalTaskTimeData.TaskTimeManager.TheTaskidBeingTimed;
            Console.WriteLine("ALERT ALERT ALERT ");
            //if task isn't being tracked or task is not task being tracked
            if (GlobalTaskData.ToEdit.BeingTimed || currenttaskid != taskbeingtimed)
            {
                TimerButton.Text = "Track Task";
                Console.WriteLine("ALERT Setting text to track task");
            }
            //if a task is being tracked and this is the task being tracked
            else
            {
                Console.WriteLine("ALTERT setting text to stop tracking");
                TimerButton.Text = "Stop Tracking";
            }
        }
    }   


    void HandleTimerOnOff(object sender, EventArgs args)
    {
        //gets guid of the current task.
        Guid currenttaskid = GlobalTaskData.ToEdit.TaskId;
        //gets the current time
        DateTime gettime = DateTime.Now;
        //Checks if other task is being timed. If it is we want to send an alert to turn off
        //timing of the other task May make popup window have buttons that does this for user
        if (GlobalTaskTimeData.TaskTimeManager.TaskIsBeingTimed)
        {
            //if the task is being time and the current task id matches the task being timed
            if(currenttaskid == GlobalTaskTimeData.TaskTimeManager.TheTaskidBeingTimed)
            {
                TimerButton.Text = "Track Task";
                GlobalTaskTimeData.TaskTimeManager.UpdateTaskItemTime(gettime, currenttaskid);
            } 
            //send alert to user that a different task is being tracked
            else
            {
                int x = 2 + 1;
            }
        } else {
            //checks if current task is not being timed on button click
            //if it is not being timed we want to change the text and change the property
            if (!GlobalTaskData.ToEdit.BeingTimed)
            {
                //update button text
                TimerButton.Text = "Stop Tracking";
                //flag for which task is being timed
                GlobalTaskTimeData.TaskTimeManager.UpdateTaskItemTime(gettime, currenttaskid);
            }
            else
            {
                GlobalTaskTimeData.TaskTimeManager.TaskIsBeingTimed = false;
            }
        }


    }

    //This function will be used by the delete task button to delete the given task
    private async void HandleDeleteTaskClicked(object sender, EventArgs args)
    {
        //The task manager will be told to delete this task, after which we will set ToEdit to null and return to the previous page
        GlobalTaskData.TaskManager.DeleteTask(GlobalTaskData.ToEdit.TaskId);
        GlobalTaskData.ToEdit = null;
        await Shell.Current.GoToAsync("..");
    }

    //This function will be used by the complete task button to "complete" a given task
    private async void HandleCompleteTaskClicked(object sender, EventArgs args)
    {
        //The task manager will be told to "complete" this task, after which we will set ToEdit to null and return to the previous page
        GlobalTaskData.TaskManager.CompleteTask(GlobalTaskData.ToEdit.TaskId);
        GlobalTaskData.ToEdit = null;
        await Shell.Current.GoToAsync("..");
        runAutoScheduler();
    }

    //This function will be used by the priority slider when its value has changed to set and keep track of the new value
    void HandleSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        //Stroring the new value and setting the sliders label correctly
        double value = args.NewValue;
        displayLabel.Text = String.Format("Priority");
    }

    //This function will be used by the add task button to either create a new task or save the changes to an existing one
    private async void HandleAddTaskButton(object sender, EventArgs e)
    {
        // Make sure we aren't storing nulls
        this.name.Text = this.name.Text == null ? "No Name" : this.name.Text;
        this.description.Text = this.description.Text == null ? "" : this.description.Text;
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;

        DateTime dateTime = new DateTime(this.date.Date.Value.Year, this.date.Date.Value.Month, this.date.Date.Value.Day,
            this.time.Time.Value.Hour, this.time.Time.Value.Minute, this.time.Time.Value.Second);
    
        //Check to see if we are currently editing or adding a task
        if(editingExistingTask)
        {
            //If we are editing, we will use the TaskManager's EditTask function to save the changes
            GlobalTaskData.TaskManager.EditTask(
                GlobalTaskData.ToEdit.TaskId,
                this.name.Text,
                this.description.Text,
                dateTime,
                (int)this.priority.Value,
                timeLogged,
                totalTime);

            GlobalTaskData.ToEdit = null;
        }
        else
        {
            //If we are not editing, use TaskManager's AddTask function to create and save the task
            GlobalTaskData.TaskManager.AddTask(
                this.name.Text,
                this.description.Text,
                dateTime,
                (int)this.priority.Value,
                timeLogged,
                totalTime);
        }
        
        //Returning to the previous page
        await Shell.Current.GoToAsync("..");
        runAutoScheduler();
    }

    //This function will load the values held in each field of a task into the respective forms
    void LoadValues()
    {
        this.name.Text = GlobalTaskData.ToEdit.Name;
        this.description.Text = GlobalTaskData.ToEdit.Description;
        this.date.Date = (GlobalTaskData.ToEdit.DueTime.Date);
        this.time.Time = GlobalTaskData.ToEdit.DueTime;
        this.priority.Value = (GlobalTaskData.ToEdit.Priority);
        this.tComplete.Value = GlobalTaskData.ToEdit.TotalTimeNeeded;
        this.tSpent.Value = GlobalTaskData.ToEdit.CompletionProgress;
    }

    //This function will set the date and time forms to the current time
    void SetValues()
    {
        this.date.Date = DateTime.Now;
        this.time.Time = DateTime.Now;
    }

    void runAutoScheduler()
    {
        autoScheduler.run();
        if (autoScheduler.taskPastDue)
        {
            string tasksString = "";
            foreach (TaskItem task in autoScheduler.pastDueTasks)
            {
                tasksString += task.Name + ", ";
            } 
            DisplayAlert("The following tasks cannot be completed on-time!", tasksString, "OK");
        }
    }
}