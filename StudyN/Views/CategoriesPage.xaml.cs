using StudyN.ViewModels;
using StudyN.Models;
using DevExpress.Maui.DataGrid;

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

		private async void AddButtonClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync(nameof(AddCategoryPage));
		}

		private async void CellClicked(object sender, DataGridGestureEventArgs e)
		{

		}
	}
}