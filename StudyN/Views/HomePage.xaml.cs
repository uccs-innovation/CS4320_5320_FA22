using StudyN.Models;
using StudyN.ViewModels;
using System.Net;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        protected string link;
        public static string content { get; set; }
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

        protected async void Button_OnClick(object sender, EventArgs e)
        {
            Console.WriteLine(link);
            if (!string.IsNullOrEmpty(link))
            { 
                HttpResponseMessage response = await client.GetAsync(link);
                var content1 = client.GetStringAsync(link);
                content = content1.Result;
                Console.WriteLine(content1.Result);
            }
        }



        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = e.NewTextValue;
        }
    }
}