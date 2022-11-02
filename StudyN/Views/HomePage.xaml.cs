using DevExpress.Maui.Scheduler;
using StudyN.Models;
using StudyN.ViewModels;
using StudyN.Common;
using StudyN.Models;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using DevExpress.Maui.DataGrid;
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
            //Initializes the Home Page the first time it is opened. Sets AutoFilterValue to Today.
            InitializeComponent();
            DateFilter.AutoFilterValue = DateTime.Today; 
            BindingContext = ViewModel = new HomeViewModel();
        }

        HomeViewModel ViewModel { get; }

        //After intialization, this is executed each time the tabbed is click to reopen the page.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            myList.RefreshData();
        }

        private async void EditAppointment(object sender, DataGridGestureEventArgs e)
        {
            if (e.Item != null && e.FieldName != "DueTime")
            {
                // TaskItem we need to edit...
                TaskItem task = (TaskItem)e.Item;
                GlobalTaskData.ToEdit = task;
                // Get it in here
                await Shell.Current.GoToAsync(nameof(AddTaskPage));
            }

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