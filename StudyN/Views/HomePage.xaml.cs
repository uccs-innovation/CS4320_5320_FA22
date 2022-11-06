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
            //Initializes the Home Page the first time it is opened. Sets AutoFilterValue to Today so that only items that are due at some point today appear.
            InitializeComponent();
            DateFilter.AutoFilterValue = DateTime.Today;

            TaskDonutChart.ChartStyle = new PieChartStyle()
            {
                Palette = new Color[]
                {
                    Color.FromArgb("#1db2f5"),
                    Color.FromArgb("#dcdcdc")
                }
            };

            // Get total screen width in "maui units"
            var screenWidthUnits = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            // Adjust for column spacing
            screenWidthUnits -= (StatGrid.Children.Count - 1) * StatGrid.ColumnSpacing;
            // Adjust for padding of all parent components
            screenWidthUnits -= (ParentStack.Padding.Left +
                                 ParentStack.Padding.Right +
                                 StatGrid.Padding.Left +
                                 StatGrid.Padding.Right);
            // Divide by number of columns to get each columns disired width in "maui units"
            var widgetWidth = screenWidthUnits / StatGrid.Children.Count;

            TaskDonutChart.WidthRequest = widgetWidth;
            TaskDonutChart.HeightRequest = widgetWidth;

            TaskCompletedStack.WidthRequest = widgetWidth;
            TaskCompletedStack.HeightRequest = widgetWidth;

            TaskRemainingStack.WidthRequest = widgetWidth;
            TaskRemainingStack.HeightRequest = widgetWidth;
        }

        HomeViewModel ViewModel { get; }

        //After intialization, this is executed each time the tabbed is click to reopen the page.
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //This refreshes the data from the DataSource for this page.
            myList.RefreshData();

            TaskSeries.CenterLabel = new PieCenterTextLabel
            {
                TextPattern = "50%"
            };
        }


        //On a single tap of an appointment, this function opens the Appointment Edit Page by DevExpress with the current information filled it for that appointment.
        private void OnTapEditAppointment(object sender, DataGridGestureEventArgs e)
        {
            //Check to ensure an actual appointment is tapped.
            if (e.Item != null)
            {
                AppointmentEditPage appointmentEditPage = new((AppointmentItem)e.Item, storage);
                Navigation.PushAsync(appointmentEditPage);
            }
        }

    }
}