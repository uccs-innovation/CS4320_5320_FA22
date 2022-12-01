using DevExpress.Maui.Scheduler;
using StudyN.ViewModels;
using StudyN.Common;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using DevExpress.Maui.DataGrid;
using DevExpress.Maui.Charts;
using StudyN.Models;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        SchedulerDataStorage storage = new SchedulerDataStorage();

        public HomePage()
        {
           
        }

        HomeViewModel ViewModel { get; }

        //After intialization, this is executed each time the tabbed is click to reopen the page.
        protected override void OnAppearing()
        {
            
        }


        //On a single tap of an appointment, this function opens the Appointment Edit Page by DevExpress with the current information filled it for that appointment.
        private void OnTapEditAppointment(object sender, DataGridGestureEventArgs e)
        {
            
        }

    }
}