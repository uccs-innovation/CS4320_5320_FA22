using DevExpress.Maui.Scheduler;
using DevExpress.Utils.Serializing;
using Microsoft.VisualBasic;
using StudyN.Models;
using StudyN.Services;
//using DevExpress.XamarinAndroid.Scheduler.Visual.Data;
using StudyN.ViewModels;
using System.ComponentModel;
using static Android.Icu.Text.IDNA;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            
            InitializeComponent();
            BindingContext = ViewModel = new CalendarViewModel();
        }

        CalendarViewModel ViewModel { get; }


        void OnDailyClicked(object sender, EventArgs args)
        {
            dayView.IsVisible = true;
            weekView.IsVisible = false;
            monthView.IsVisible = false;

        }

        void OnWeeklyClicked(object sender, EventArgs args)
        {
            dayView.IsVisible = false;
            weekView.IsVisible = true;
            monthView.IsVisible = false;

        }

        void OnMonthlyClicked(object sender, EventArgs args)
        {
            dayView.IsVisible = false;
            weekView.IsVisible = false;
            monthView.IsVisible = true;
        }

        void OnAddCalendarEventClicked(object sender, EventArgs args)
        {
            AppointmentEditPage appEditPage = new AppointmentEditPage(DateTime.Today, DateTime.Today, false, this.dayView.DataStorage);
            Navigation.PushAsync(appEditPage);
            this.weekView.DataStorage = this.dayView.DataStorage;
            this.monthView.DataStorage = this.dayView.DataStorage;
        }

        void OnPopupAddButtonClicked(object sender, EventArgs args)
        {
            Item newItem = new Item();
            newItem.Id = "5"; //TODO generate unique id's on each add
            newItem.Text = Description.Text;
            newItem.Description = Description.Text;

            string[] sstring = StartTime.Text.Split('/');
            string[] estring = EndTime.Text.Split('/');

            DateTime start = new DateTime(Int16.Parse(sstring[0]), Int16.Parse(sstring[1]), Int16.Parse(sstring[2]), Int16.Parse(sstring[3]), Int16.Parse(sstring[4]), Int16.Parse(sstring[5])); //Should probably find a datetimepicker library
            DateTime end = new DateTime(Int16.Parse(estring[0]), Int16.Parse(estring[1]), Int16.Parse(estring[2]), Int16.Parse(estring[3]), Int16.Parse(estring[4]), Int16.Parse(estring[5])); 
            newItem.StartTime = start;
            newItem.EndTime = end;
            newItem.Value = 1; //Not sure what "value" is tbh
            ViewModel.AddToDataStore(newItem);
            ViewModel.Items.Add(newItem); //adds to calendar, but not datastore
            Popup.IsOpen = false;
        }

        void CalenderDoubleTap(object sender, SchedulerGestureEventArgs args)
        {
            if (args.AppointmentInfo == null)
            {
                ShowNewEventEdit(args.IntervalInfo);
                return;
            }
            else
            {
               AppointmentItem appointment = args.AppointmentInfo.Appointment;
                ShowEventEdit(appointment);
            }

        }

        private void ShowNewEventEdit(IntervalInfo info)
        {
            AppointmentEditPage appEditPage = new AppointmentEditPage(info.Start, info.End, info.AllDay, this.dayView.DataStorage);
            Navigation.PushAsync(appEditPage);
            this.weekView.DataStorage = this.dayView.DataStorage;
            this.monthView.DataStorage = this.dayView.DataStorage;
        }

        private void ShowEventEdit(AppointmentItem appointment)
        {
            AppointmentEditPage appEditPage = new AppointmentEditPage(appointment, this.dayView.DataStorage);
            Navigation.PushAsync(appEditPage);
            this.weekView.DataStorage = this.dayView.DataStorage;
            this.monthView.DataStorage = this.dayView.DataStorage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}