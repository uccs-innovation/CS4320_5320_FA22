using DevExpress.Maui.Scheduler;
using DevExpress.Utils.Serializing;
using Microsoft.VisualBasic;
using StudyN.Models;
using StudyN.Services;
//using DevExpress.XamarinAndroid.Scheduler.Visual.Data;
using StudyN.ViewModels;
using System.ComponentModel;

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

        void OnClickOpenHowToUse(object sender, EventArgs args)
        {
            Popup.IsOpen = true;
        }

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

        void CalenderSingleTap(object sender, SchedulerGestureEventArgs args)
        {
            if (args.AppointmentInfo != null)
            {
                AppointmentItem appointment = args.AppointmentInfo.Appointment;
                AppointmentDetailPage appDetailPage = new AppointmentDetailPage(appointment, this.dayView.DataStorage);
                Navigation.PushAsync(appDetailPage);
                this.weekView.DataStorage = this.dayView.DataStorage;
                this.monthView.DataStorage = this.dayView.DataStorage;
            }
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
        }
    }
}