namespace StudyN.Views;
using StudyN.Models;
using StudyN.ViewModels;

public partial class AddTaskPage : ContentPage
{
	public AddTaskPage()
	{
		InitializeComponent();
	}

    void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        double value = args.NewValue;
        displayLabel.Text = String.Format("Priority");
    }

    private async void AddTaskButton(object sender, EventArgs e)
    {
  
            //File.WriteAllText(note.Filename, TextEditor.Text);
            UIGlobal.MainPage.AddTask(this.name.Text, this.description.Text, new DateTime(), (int)this.priority.Value);

        await Shell.Current.GoToAsync("..");
    }
}