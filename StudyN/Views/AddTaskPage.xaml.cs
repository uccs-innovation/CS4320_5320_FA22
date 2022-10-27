namespace StudyN.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Animations;
using StudyN.Models;
using StudyN.Utilities;
using StudyN.ViewModels;
using static Android.Provider.Settings;

public partial class AddTaskPage : ContentPage
{
    bool editingExistingTask;
    AutoScheduler autoScheduler;
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
        if (editingExistingTask)
        {
            HandleEditRecurrence(GlobalTaskData.ToEdit.TaskId);
        }
        else
        {
            //If we are not editing, use TaskManager's AddTask function to create and save the task
            task = GlobalTaskData.TaskManager.AddTask(
                    this.name.Text,
                    this.description.Text,
                    dateTime,
                    (int)this.priority.Value,
                    timeLogged,
                    totalTime);
        }
        
        //Returning to the previous page
        await Shell.Current.GoToAsync("..");
        runAutoScheduler(task.TaskId);
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

    void runAutoScheduler(Guid taskId)
    {
        autoScheduler.run(taskId);
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

    void OnCheckBoxDueDateChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value == true)
        {
            this.date.Date = DateTime.MaxValue;
            this.time.Time = DateTime.MaxValue;
            date.IsVisible = false;
            time.IsVisible = false;
        }
        else if(e.Value == false)
        {
            this.date.Date = DateTime.Now;
            this.time.Time = DateTime.Now;
            date.IsVisible = true;
            time.IsVisible = true;
        }
    }

    //These functions will be used to add recurrence of a selected task for day/week/month
    private void HandleRecurrenceDay(object sender, EventArgs e)
    {
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;
        DateTime dateTime = new DateTime(this.date.Date.Value.Year, this.date.Date.Value.Month, this.date.Date.Value.Day,
            this.time.Time.Value.Hour, this.time.Time.Value.Minute, this.time.Time.Value.Second);
        for (int i = 1; i <= 365; i++)
        {
            dateTime = dateTime.AddDays(i); //every day
            //If we are not editing, use TaskManager's AddTask function to create and save the task
            GlobalTaskData.TaskManager.AddTask(
                this.name.Text,
                this.description.Text,
                dateTime,
                (int)this.priority.Value,
                timeLogged,
                totalTime);
            
        }

    }
    private void HandleRecurrenceWeek(object sender, EventArgs e)
    {
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;
        DateTime dateTime = new DateTime(this.date.Date.Value.Year, this.date.Date.Value.Month, this.date.Date.Value.Day,
            this.time.Time.Value.Hour, this.time.Time.Value.Minute, this.time.Time.Value.Second);

        for (int i = 1; i <= 52; i++)
        {
            dateTime = dateTime.AddDays(i*7); //every week
            //If we are not editing, use TaskManager's AddTask function to create and save the task
            GlobalTaskData.TaskManager.AddTask(
                this.name.Text,
                this.description.Text,
                dateTime,
                (int)this.priority.Value,
                timeLogged,
                totalTime);
            
        }

    }
    private void HandleRecurrenceMonth(object sender, EventArgs e)
    {
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;
        DateTime dateTime = new DateTime(this.date.Date.Value.Year, this.date.Date.Value.Month, this.date.Date.Value.Day,
            this.time.Time.Value.Hour, this.time.Time.Value.Minute, this.time.Time.Value.Second);

        for (int i = 1; i <= 12; i++)
        {
            dateTime = dateTime.AddMonths(i); //months
            //If we are not editing, use TaskManager's AddTask function to create and save the task
            GlobalTaskData.TaskManager.AddTask(
                this.name.Text,
                this.description.Text,
                dateTime,
                (int)this.priority.Value,
                timeLogged,
                totalTime);

        }
    }
    void HandleEditRecurrence(Guid id)
    {
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;
        DateTime dateTime = new DateTime(this.date.Date.Value.Year, this.date.Date.Value.Month, this.date.Date.Value.Day,
            this.time.Time.Value.Hour, this.time.Time.Value.Minute, this.time.Time.Value.Second);

        //Gets task list
        ObservableCollection<TaskItem> task = new ObservableCollection<TaskItem>();
        for (int i = 0; i < task.Count; i++)
        {
            //If information is not the same, then it gets saved for all
            if (task[i].Description != this.description.Text)
            {
                task[i].Description =this.description.Text;
            }
            if(task[i].DueTime != dateTime)
            {
                task[i].DueTime = dateTime;
            }
            if(task[i].CompletionProgress != timeLogged)
            {
                task[i].CompletionProgress = timeLogged;
            }
            if (task[i].TotalTimeNeeded != totalTime)
            {
                task[i].TotalTimeNeeded = totalTime;
            }

        }
        //Saves the informatiom when editing
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

}
