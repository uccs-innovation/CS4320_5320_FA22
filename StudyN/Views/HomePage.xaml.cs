using DevExpress.Maui.Scheduler;
using StudyN.ViewModels;
using StudyN.Common;
using StudyN.Models;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using DevExpress.Maui.DataGrid;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

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
            ViewModel.OnAppearing();
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
    }
}