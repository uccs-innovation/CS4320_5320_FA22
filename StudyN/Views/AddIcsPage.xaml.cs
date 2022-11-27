
/*
 * 
 * WHAT IS HAPPENING HERE??:
 * 
 * This takes in string inputted into text box
 * then pings https to get response of what is in https
 * returns as string to be parsed
 * parse string for needed info
 * convert needed info to appointment
 * let createAppointment() handle that shit
 * shit on everything not needed or that has already been parsed
 * done ;)
 * return
 * hopefully dont get fatal errors
 * 
*/

using AndroidX.Lifecycle;
using StudyN.Models;
using StudyN.ViewModels;
using System.Net;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddIcsPage : ContentPage
    {
        protected string link;
        public static string content { get; set; }
        static readonly HttpClient client = new HttpClient();
        public AddIcsPage()
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
        protected async void Submit_Button(object sender, EventArgs e)
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