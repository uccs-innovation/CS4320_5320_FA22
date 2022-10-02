namespace StudyN.Views;
using StudyN.Models;
using StudyN.ViewModels;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage()
	{
		InitializeComponent();
        if (UIGlobal.ToEdit != null)
        {
            LoadValues();
        }
	}

    //toolbar check button for adding task
    private async void AddButtonClicked(object sender, EventArgs e)
    {
        //will need to return to taskpage
        //This needs to be changed
        //don't u ignore me
        await Shell.Current.GoToAsync(nameof(AddTaskPage));
    }
    private async void OnDeleteTaskClicked(object sender, EventArgs args)
    {
    }

    private async void OnCompleteTaskClicked(object sender, EventArgs args)
    {
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        displayLabel.Text = String.Format("Priority");
    }

    private async void AddTaskButton(object sender, EventArgs e)
    {
        UIGlobal.MainPage.AddTask(this.name.Text, this.description.Text, this.date.Date.Value.AddMilliseconds(this.time.Time.Value.TimeOfDay.TotalMilliseconds), (int)this.priority.Value);

        await Shell.Current.GoToAsync("..");
    }
    void LoadValues()
    {
        this.name.Text = UIGlobal.ToEdit.Name;
        this.description.Text = UIGlobal.ToEdit.Description;
        this.date.Date.Value.Equals(UIGlobal.ToEdit.DueTime.Date);
        this.time.Time.Value.Equals(UIGlobal.ToEdit.DueTime.TimeOfDay);
        this.priority.Value.Equals(UIGlobal.ToEdit.Priority);
    }
}