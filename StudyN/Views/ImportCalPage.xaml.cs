using StudyN.ViewModels;

namespace StudyN.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImportCalPage : ContentPage
	{
		public ImportCalPage()
		{
			InitializeComponent();
			BindingContext = ViewModel = new ImportCalViewModel();
		}

		ImportCalViewModel ViewModel { get; }

		void SimpleButton_Clicked(object sender, EventArgs e)
		{
			ViewModel.OnSubmit();
		}
	}
}