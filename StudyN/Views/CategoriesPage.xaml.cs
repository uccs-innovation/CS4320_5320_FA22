using StudyN.ViewModels;
using StudyN.Models;
using DevExpress.Maui.DataGrid;

namespace StudyN.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoriesPage : ContentPage
	{
        	// Flag to prevent multiple child pages opening
        	bool isChildPageOpening = false;
		public CategoriesPage()
		{
			InitializeComponent();
			BindingContext = ViewModel = new CategoriesViewModel();
			DataGrid.EndUpdate();
		}

        	CategoriesViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
		    isChildPageOpening = false;
		    BindingContext = ViewModel = new CategoriesViewModel();
			DataGrid.EndUpdate();
		}

		/// <summary>
		/// Pressing this button will allow the user to go to add category page
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        	private async void AddButtonClicked(object sender, EventArgs e)
		{
			if (!isChildPageOpening)
			{
				isChildPageOpening = true;
				GlobalAppointmentData.EditCategory = null;
				DataGrid.BeginUpdate();
				Routing.RegisterRoute(nameof(Views.AddCategoryPage), typeof(Views.AddCategoryPage));
				await Shell.Current.GoToAsync(nameof(AddCategoryPage));
			}
		}

		/// <summary>
		/// Clicking on a cell will bring user to an edit category page with category information
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void CellClicked(object sender, DataGridGestureEventArgs e)
		{
			if(e.Item != null && !isChildPageOpening)
			{
                // Get selected category for editing
                isChildPageOpening = true;
                AppointmentCategory cat = (AppointmentCategory)e.Item;
				GlobalAppointmentData.EditCategory = cat;
				DataGrid.BeginUpdate();
               	Routing.RegisterRoute(nameof(Views.AddCategoryPage), typeof(Views.AddCategoryPage));
                await Shell.Current.GoToAsync(nameof(AddCategoryPage));
			}
		}
	}
}
