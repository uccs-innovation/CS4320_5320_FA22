using StudyN.ViewModels;
using DevExpress.XtraEditors;

namespace StudyN.Views
{
	public partial class AddCategoryPage : ContentPage
	{
		public AddCategoryPage()
		{
			InitializeComponent();
			BindingContext = new AddCategoryViewModel();
			Title = "Add Category";
		}
	}
}