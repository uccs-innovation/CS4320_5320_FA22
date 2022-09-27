using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views;

public partial class TaskDetailPage : ContentPage
{
	public TaskDetailPage()
	{
		InitializeComponent();
		BindingContext = new TaskDetailViewModel();
	}
	private async void RemoveButton(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(TaskPage));
    }
	private void ModifyButton(object sender, EventArgs e)
	{

	}
}