using DevExpress.Maui.Scheduler;
using StudyN.Common;
using StudyN.Models; //Calls Calendar Data
using StudyN.Utilities;
using StudyN.ViewModels;
using System.ComponentModel;
using static StudyN.Utilities.StudynEvent;

namespace StudyN.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class CalendarPage : ContentPage, StudynSubscriber
    {
        readonly CalendarDataView _calendarDataView;
        public CalendarPage()
        {
            InitializeComponent();
            ViewModel = new CalendarViewModel();
            BindingContext = _calendarDataView = new CalendarDataView(); //Use to pull data of CalendarData under Models

            EventBus.Subscribe(this);

            // Reuse data storage between all the views
            weekView.DataStorage = dayView.DataStorage;
            monthView.DataStorage = dayView.DataStorage;
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
            var notes = SchedulerStorage.GetAppointments(new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)));
            CalendarDataView.LoadDataForNotification(notes.ToList());
            base.OnAppearing();
        }

        private void ShowAppointmentEditPage(AppointmentItem appointment)
        {
            AppointmentEditPage appEditPage = new(appointment, SchedulerStorage);
            Navigation.PushAsync(appEditPage);
        }

        private void ShowNewAppointmentEditPage(IntervalInfo info)
        {
            AppointmentEditPage appEditPage = new(info.Start, info.End, info.AllDay, SchedulerStorage);
            Navigation.PushAsync(appEditPage);
        }

        private void OnCalendarTap(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }

        private async void OnCalendarHold(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo != null)
            {
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?",
                    appointment.Subject + " will be deleted.", "Yes", "No");

                if (answer == true)
                {
                    SchedulerStorage.RemoveAppointment(appointment);

                    // Publish task delete appointment
                    EventBus.PublishEvent(
                                new StudynEvent(new Guid(), 
                                StudynEvent.StudynEventType.AppointmentDelete));
                }
            }
        }

        public void OnNewStudynEvent(StudynEvent sEvent)
        {
            // On any appointment event, refresh the data
            if (sEvent.EventType == StudynEventType.AppointmentAdd
                || sEvent.EventType == StudynEventType.AppointmentEdit
                || sEvent.EventType == StudynEventType.AppointmentDelete)
            {
                SchedulerStorage.RefreshData();
            }
        }

        //View of events 
        public class CalendarDataView : INotifyPropertyChanged
        {
            readonly CalendarManager data;

            public event PropertyChangedEventHandler PropertyChanged;
            public static DateTime StartDate { get { return CalendarManager.BaseDate; } }

            public IReadOnlyList<Appointment> Appointments { get => data.Appointments; }
            public IReadOnlyList<AppointmentCategory> AppointmentCategories { get => data.AppointmentCategories; }
            public IReadOnlyList<AppointmentStatus> AppointmentStatuses { get => data.AppointmentStatuses; }

            public CalendarDataView()
            {
                data = GlobalAppointmentData.CalendarManager;
            }

            /// <summary>
            /// Uses static class DataAccess to load the notification database with AppointmentItems
            /// </summary>
            public void LoadDataForNotification()
            {
                LoadDataForNotification(Appointments);
            }

            /// <summary>
            /// Takes a collection of AppointmentItems and loads the notification database using static class DataAccess
            /// </summary>
            /// <param name="appointments"></param>
            public static void LoadDataForNotification(IReadOnlyList<AppointmentItem> appointments)
            {
                DataAccess.LoadData(appointments);
            }

            protected void RaisePropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                    LoadDataForNotification();
                }
            }
        }


    }
} 