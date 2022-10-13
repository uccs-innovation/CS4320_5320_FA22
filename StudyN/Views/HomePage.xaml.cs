using StudyN.Models;
using StudyN.ViewModels;
using System.Net;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        protected string link;
        public static string Result { get; set; }
        static readonly HttpClient client = new HttpClient();

        public HomePage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new HomeViewModel();
            ViewModel.OnAppearing();
        }

        HomeViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        protected void Button_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine(link);
            if (!string.IsNullOrEmpty(link))
            { 
                try
                {
                    var content = client.GetStringAsync(link);
                    Result = content.Result;
                } 
                catch (Exception ex)
                {
                }
            }
        }



        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = e.NewTextValue;
        }
    }
}