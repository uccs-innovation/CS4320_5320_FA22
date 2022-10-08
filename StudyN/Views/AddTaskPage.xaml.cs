namespace StudyN.Views;

using Microsoft.Maui.Animations;
using StudyN.Models;
using StudyN.Utilities;
using StudyN.ViewModels;

public partial class AddTaskPage : ContentPage
{
    bool EditButtonsVisible;
	public AddTaskPage()
	{
		InitializeComponent();

        if (GlobalTaskData.ToEdit != null)
        {
            Title = "Edit Task";
            LoadValues();
            BindingContext = new EditTaskViewModel();
            EditButtonsVisible = true;
        }
        else
        {
            Title = "Add Task";
            EditButtonsVisible =false; 
            SetValues();
        }
        DeleteTaskButton.IsVisible = EditButtonsVisible;
        CompleteTaskButton.IsVisible = EditButtonsVisible;
	}

    //calls delete task
    private async void HandleDeleteTaskClicked(object sender, EventArgs args)
    {
        GlobalTaskData.ToEdit.Parent.DeleteTask(GlobalTaskData.ToEdit.TaskId);
        await Shell.Current.GoToAsync("..");

    }

    private async void HandleCompleteTaskClicked(object sender, EventArgs args)
    {
        GlobalTaskData.ToEdit.Parent.CompleteTask(GlobalTaskData.ToEdit.TaskId);
        await Shell.Current.GoToAsync("..");
    }

    void HandleSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        displayLabel.Text = String.Format("Priority");
    }

    private async void HandleAddTaskButton(object sender, EventArgs e)
    {
        // Make sure we aren't storing nulls
        this.name.Text = this.name.Text == null ? "No Name" : this.name.Text;
        this.description.Text = this.description.Text == null ? "" : this.description.Text;
        int timeLogged = this.tSpent.Value == null ? 0 : (int)this.tSpent.Value;
        int totalTime = this.tComplete.Value == null ? 0 : (int)this.tComplete.Value;

        TaskItem task = GlobalTaskData.TaskManager.AddTask(
            this.name.Text,
            this.description.Text,
            this.date.Date.Value.AddMilliseconds(this.time.Time.Value.TimeOfDay.TotalMilliseconds),
            (int)this.priority.Value,
            timeLogged,
            totalTime);

        if (GlobalTaskData.ToEdit != null)
        {
            GlobalTaskData.TaskManager.CompleteTask(GlobalTaskData.ToEdit.TaskId);
            GlobalTaskData.ToEdit = null;
        }
        
        await Shell.Current.GoToAsync("..");
    }
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

    void SetValues()
    {
        this.date.Date = DateTime.Now;
        this.time.Time = DateTime.Now;
    }
}