using StudyN.ViewModels;
using StudyN.Models;
using DevExpress.Maui.DataGrid;
using StudyN.Common;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesPage : ContentPage
	{
		public CategoriesPage()
		{
			InitializeComponent();
			BindingContext = new CategoriesViewModel();
		}

		/// <summary>
		/// Pressing this button will allow the user to go to add category page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void AddButtonClicked(object sender, EventArgs e)
		{
			GlobalAppointmentData.EditCategory = null;
			Routing.RegisterRoute(nameof(Views.AddCategoryPage), typeof(Views.AddCategoryPage));
			await Shell.Current.GoToAsync(nameof(AddCategoryPage));
		}

		/// <summary>
		/// Clicking on a cell will bring user to an edit category page with category information
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void CellClicked(object sender, DataGridGestureEventArgs e)
		{
			if(e.Item != null)
			{
				// Get selected category for editing
				AppointmentCategory cat = (AppointmentCategory)e.Item;
				GlobalAppointmentData.EditCategory = cat;
                Routing.RegisterRoute(nameof(Views.AddCategoryPage), typeof(Views.AddCategoryPage));
                await Shell.Current.GoToAsync(nameof(AddCategoryPage));
			}
		}
	}
}