using StudyN.ViewModels;
using DevExpress.XtraEditors;
using StudyN.Models;

namespace StudyN.Views
{
	public partial class AddCategoryPage : ContentPage
	{
		public AddCategoryPage()
		{
			InitializeComponent();
			BindingContext = new AddCategoryViewModel();
			Title = "Add Category";
			RemoveButton.IsVisible = false;
		}

		/// <summary>
		/// Either creates a new category or make changes to existing category
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void SaveButtonClicked(object sender, EventArgs e)
		{
			this.name.Text = this.name.Text == null ? "No Name" : this.name.Text;
			// if input is invalid than make color black
			if (this.color.Text == null)
			{
				this.color.Text = "#000000";
			}
			else if (!this.color.Text.StartsWith('#'))
			{
				this.color.Text = "#000000";
			}
			else
			{
				this.color.Text = this.color.Text.Substring(7);
			}
			// turns color string input into color
			Color useColor = Color.FromArgb(this.color.Text);
			// creates the category
			AppointmentCategory cat;
			cat = GlobalAppointmentData.CalendarManager.CreateCategory(this.name.Text, useColor);
			await Shell.Current.GoToAsync("..");
		}
	}
}