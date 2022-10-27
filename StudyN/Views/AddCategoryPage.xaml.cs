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
			if(GlobalAppointmentData.EditCategory == null)
			{
				// Pulls up Add Category Page
				BindingContext = new AddCategoryViewModel();
				Title = "Add Category";
				RemoveButton.IsVisible = false;
			}
			else
			{
				// Pulls up Edit Category Page
				BindingContext = new EditCategoryViewModel();
				Title = "Edit Category";
				LoadValues();
				RemoveButton.IsVisible = true;
			}
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
			if (this.color.Text == null || !this.color.Text.StartsWith('#'))
			{
				this.color.Text = "#000000";
			}
			else
			{
				this.color.Text = this.color.Text.Substring(7);
			}
			// turns color string input into color
			Color useColor = Color.FromArgb(this.color.Text);
			AppointmentCategory cat;
			if (GlobalAppointmentData.EditCategory == null) {
				// creates the category
				cat = GlobalAppointmentData.CalendarManager.CreateCategory(this.name.Text, useColor);
			}
			else
			{
				// edit the category
				GlobalAppointmentData.CalendarManager.EditCategory(this.name.Text, useColor, 
																	GlobalAppointmentData.EditCategory.Id);
				cat = GlobalAppointmentData.EditCategory;
				GlobalAppointmentData.EditCategory = null;
			}
			await Shell.Current.GoToAsync("..");
		}

		/// <summary>
		/// Gets Values from passed in category
		/// </summary>
		void LoadValues()
		{
			this.name.Text = GlobalAppointmentData.EditCategory.Caption;
			string colorHex = GlobalAppointmentData.EditCategory.Color.ToHex();
			this.color.Text = colorHex;
		}

		/// <summary>
		/// Removes the category
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void RemoveButtonClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}