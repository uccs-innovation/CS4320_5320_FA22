using StudyN.ViewModels;

namespace StudyN.Views
{
	public partial class AddCategoryPage : ContentPage
	{
		public AddCategoryPage()
		{
			InitializeComponent();
			BindingContext = new AddCategoryViewModel();
		}
	}
}