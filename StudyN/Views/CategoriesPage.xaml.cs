using StudyN.ViewModels;

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
	}
}