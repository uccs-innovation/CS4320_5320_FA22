using DevExpress.Maui.Scheduler;
using StudyN.ViewModels;
using StudyN.Common;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using DevExpress.Maui.DataGrid;
using DevExpress.Maui.Charts;
using StudyN.Models;

//This file is part of the task overview under the Task page
namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskChartsPage : ContentPage
    {

        SchedulerDataStorage storage = new SchedulerDataStorage();

        public TaskChartsPage()
        {
            BindingContext = ViewModel = new HomeViewModel();
            //Initializes the Home Page the first time it is opened.
            //Sets AutoFilterValue to Today so that only items that are due at some point today appear.
            InitializeComponent();


            TaskDonutChart.ChartStyle = new PieChartStyle()
            {
                Palette = new Color[]
                {
                    Color.FromArgb("#1db2f5"),
                    Color.FromArgb("#dcdcdc")
                }
            };

            HoursDonutChart.ChartStyle = TaskDonutChart.ChartStyle;

            // Get total screen width in "maui units"
            var screenWidthUnits = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            // Adjust for column spacing
            screenWidthUnits -= (StatGrid.ColumnDefinitions.Count - 1) * StatGrid.ColumnSpacing;
            // Adjust for padding of all parent components
            screenWidthUnits -= (ParentStack.Padding.Left +
                                 ParentStack.Padding.Right +
                                 StatGrid.Padding.Left +
                                 StatGrid.Padding.Right);
            // Divide by number of columns to get each columns disired width in "maui units"
            var widgetWidth = screenWidthUnits / StatGrid.ColumnDefinitions.Count;

            // Set the width and height of all the stat boxes
            TaskCompletedStack.WidthRequest = widgetWidth;
            TaskCompletedStack.HeightRequest = widgetWidth;

            TaskRemainingStack.WidthRequest = widgetWidth;
            TaskRemainingStack.HeightRequest = widgetWidth;

            TaskDonutChart.WidthRequest = widgetWidth;
            TaskDonutChart.HeightRequest = widgetWidth;

            HoursCompletedStack.WidthRequest = widgetWidth;
            HoursCompletedStack.HeightRequest = widgetWidth;

            HoursRemainingStack.WidthRequest = widgetWidth;
            HoursRemainingStack.HeightRequest = widgetWidth;

            HoursDonutChart.WidthRequest = widgetWidth;
            HoursDonutChart.HeightRequest = widgetWidth;
        }

        HomeViewModel ViewModel { get; }

        //After intialization, this is executed each time the icon is click to reopen the page.
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Populate the statistics fields
            int numTasksCompleted = GlobalTaskData.TaskManager.NumTasksCompletedToday();
            int numTasksDueToday = GlobalTaskData.TaskManager.NumTasksDueToday();

            int numHoursCompleted = GlobalAppointmentData.CalendarManager.NumHoursCompletedToday();
            int numHoursScheduled = GlobalAppointmentData.CalendarManager.NumHoursScheduledToday();

            double taskPercentage = numTasksDueToday == 0 ?
                                    0 : (((double)numTasksCompleted / (double)numTasksDueToday) * 100);

            ViewModel.SetTaskPercentage(taskPercentage);

            double hourPercentage = numHoursScheduled == 0 ?
                                    0 : (((double)numHoursCompleted / (double)numHoursScheduled) * 100);

            ViewModel.SetHourPercentage(hourPercentage);

            string taskPercentageString = numTasksDueToday == 0 ?
                                    "--%" : taskPercentage.ToString() + "%";

            string hoursPercentageString = numHoursScheduled == 0 ?
                                    "--%" : hourPercentage.ToString() + "%";

            TaskSeries.CenterLabel = new PieCenterTextLabel
            {
                TextPattern = taskPercentageString
            };

            HourSeries.CenterLabel = new PieCenterTextLabel
            {
                TextPattern = hoursPercentageString
            };

            NumTasksCompleted.Text = numTasksCompleted.ToString();
            NumTasksRemaining.Text = numTasksDueToday.ToString();

            NumHoursCompleted.Text = numHoursCompleted.ToString();
            NumHoursRemaining.Text = numHoursScheduled.ToString();
        }


        //On a single tap of an appointment, this function opens the Appointment Edit Page
        //by DevExpress with the current information filled it for that appointment.
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
