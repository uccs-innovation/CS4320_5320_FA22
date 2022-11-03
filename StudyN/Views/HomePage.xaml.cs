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
            //Initializes the Home Page the first time it is opened. Sets AutoFilterValue to Today so that only items that are due at some point today appear.
            InitializeComponent();
            DateFilter.AutoFilterValue = DateTime.Today; 
            BindingContext = ViewModel = new HomeViewModel();
        }

        HomeViewModel ViewModel { get; }

        //After intialization, this is executed each time the tabbed is click to reopen the page.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //This refreshes the data from the DataSource for this page.
            myList.RefreshData();
        }


        //On a single tap of an appointment, this function opens the Appointment Edit Page by DevExpress with the current information filled it for that appointment.
        private void OnTapEditAppointment(object sender, DataGridGestureEventArgs e)
        {
            //Check to ensure an actual appointment is tapped.
            if (e.Item != null)
            {
                AppointmentItem appointment = (Appointment)e.Item;
                SchedulerDataStorage storage = new SchedulerDataStorage();
                AppointmentEditPage appEditPage = new(appointment, storage);
                Navigation.PushAsync(appEditPage);
            }
        }

    }
}