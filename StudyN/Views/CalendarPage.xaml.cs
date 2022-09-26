using DevExpress.Maui.Scheduler;
using DevExpress.Utils.Serializing;
using StudyN.Models; //Calls Calendar Data
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
            ViewModel = new CalendarViewModel();
            BindingContext = new UserCalendarDataView(); //Use to pull data of CalendarData under Models

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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private void CalendarTap(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }

        private void ShowAppointmentEditPage(AppointmentItem appointment)
        {
            AppointmentEditPage appEditPage = new AppointmentEditPage(appointment, this.storage);
            Navigation.PushAsync(appEditPage);
        }

        private void ShowNewAppointmentEditPage(IntervalInfo info)
        {
            AppointmentEditPage appEditPage = new AppointmentEditPage(info.Start, info.End,
                                                                     info.AllDay, this.storage);
            Navigation.PushAsync(appEditPage);
        }

        //View of events 
        public class UserCalendarDataView : INotifyPropertyChanged
        {
            readonly AppData data;

            public event PropertyChangedEventHandler PropertyChanged;
            public DateTime StartDate { get { return AppData.BaseDate; } }
            public IReadOnlyList<UserEvents> UserEvents { get => data.UserEven; }
            public IReadOnlyList<UserEventsType> AppointmentTypes { get => data.Labels; }

            public UserCalendarDataView()
            {
                data = new AppData();
            }

            protected void RaisePropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }

    }
} 