using StudyN.ViewModels;
using StudyN.Models;

namespace StudyN.Views
{
	public partial class AddCategoryPage : ContentPage
	{
		Color color;
		
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
		/// Changes when user changes color and gets label
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="colorPicked"></param>
		private void PickedColorChanged(object sender, Color colorPicked)
		{
			this.color = colorPicked;
			displayLabel.Text = String.Format("Pick Color");
		}

		/// <summary>
		/// Either creates a new category or make changes to existing category
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void SaveButtonClicked(object sender, EventArgs e)
		{
			// Gets input values, if there are none, give default values
			this.name.Text = this.name.Text == null ? "No Name" : this.name.Text;
			this.color = this.color == null ? Color.FromArgb("#000000") : this.color;
			AppointmentCategory cat;
			if (GlobalAppointmentData.EditCategory == null) {
				// creates the category
				cat = GlobalAppointmentData.CalendarManager.CreateCategory(this.name.Text, this.color, 
																			this.colorPicker.PointerRingPositionXUnits, 
																			this.colorPicker.PointerRingPositionYUnits);
			}
			else
			{
				// edit the category
				GlobalAppointmentData.CalendarManager.EditCategory(this.name.Text, this.color, 
																	this.colorPicker.PointerRingPositionXUnits, 
																	this.colorPicker.PointerRingPositionYUnits,
																	GlobalAppointmentData.EditCategory.UniqueId);
				cat = GlobalAppointmentData.EditCategory;
				GlobalAppointmentData.EditCategory = null;
			}
			Routing.RegisterRoute(nameof(Views.CategoriesPage), typeof(Views.CategoriesPage));
			await Shell.Current.GoToAsync("..");
		}

		/// <summary>
		/// Gets Values from passed in category
		/// </summary>
		void LoadValues()
		{
			this.name.Text = GlobalAppointmentData.EditCategory.Caption;
			this.color = GlobalAppointmentData.EditCategory.Color;
			this.colorPicker.PointerRingPositionXUnits = GlobalAppointmentData.EditCategory.PickerXPosition;
			this.colorPicker.PointerRingPositionYUnits = GlobalAppointmentData.EditCategory.PickerYPosition;
		}

		/// <summary>
		/// Removes the category
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void RemoveButtonClicked(object sender, EventArgs e)
		{
			GlobalAppointmentData.CalendarManager.RemoveCategory(GlobalAppointmentData.EditCategory.UniqueId);
			GlobalAppointmentData.EditCategory= null;
			Routing.RegisterRoute(nameof(Views.CategoriesPage), typeof(Views.CategoriesPage));
			await Shell.Current.GoToAsync("..");
		}
	}
}