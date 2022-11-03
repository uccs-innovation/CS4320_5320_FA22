using DevExpress.Maui.Scheduler;
using DevExpress.Web.ASPxScheduler.Forms;
using DevExpress.XamarinAndroid.Scheduler;
using DevExpress.XtraScheduler.Native;
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
            dailyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
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
            dailyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
            weeklyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);
            monthlyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);
        }

        void OnWeeklyClicked(object sender, EventArgs args)
        {
            dayView.IsVisible = false;
            weekView.IsVisible = true;
            monthView.IsVisible = false;
            dailyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);
            weeklyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
            monthlyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);

        }

        void OnMonthlyClicked(object sender, EventArgs args)
        {
            dayView.IsVisible = false;
            weekView.IsVisible = false;
            monthView.IsVisible = true;
            dailyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);
            weeklyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 0);
            monthlyButton.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        }

        protected override void OnAppearing()
        {
            Console.WriteLine("CalendarPage OnAppearing");
            SchedulerStorage.RefreshData();
            //SchedulerStorage.AppointmentItems.Refresh(); //https://supportcenter.devexpress.com/ticket/details/q320528/slow-scheduler-refresh //https://supportcenter.devexpress.com/ticket/details/t615692/how-to-programmatically-refresh-scheduler
            //SchedulerElement.
            InvalidateMeasure();
            
            var notes = SchedulerStorage.GetAppointments(new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)));
            CalendarDataView.LoadDataForNotification(notes.ToList());
            base.OnAppearing();
        }

        private void Handle_onCalendarTap_FromDayView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }
        private void Handle_onCalendarTap_FromWeekView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }
        private void Handle_onCalendarTap_FromMonthView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo == null)
            {
                ShowNewAppointmentEditPage(e.IntervalInfo);
                return;
            }
            AppointmentItem appointment = e.AppointmentInfo.Appointment;
            ShowAppointmentEditPage(appointment);
        }

        private async void Handle_onCalendarHold_FromDayView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo != null)
            {
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?",
                    appointment.Subject + " will be deleted.", "Yes", "No");

                if (answer == true)
                {
                    SchedulerStorage.RemoveAppointment(appointment);
                }
            }
        }
        private async void Handle_onCalendarHold_FromMonthView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo != null)
            {
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?",
                    appointment.Subject + " will be deleted.", "Yes", "No");

                if (answer == true)
                {
                    SchedulerStorage.RemoveAppointment(appointment);
                }
            }
        }
        private async void Handle_onCalendarHold_FromView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo != null)
            {
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?",
                    appointment.Subject + " will be deleted.", "Yes", "No");

                if (answer == true)
                {
                    SchedulerStorage.RemoveAppointment(appointment);
                }
            }
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

        // estep: I know there must be a better way to do this, but I just want to try it
        //        since it won't let me use the same storage name for both SchedulerDataStorage objects
       //        (so I have to have this kind of repeated code)
        

        private async void Handle_onCalendarHold_FromWeekView(object sender, SchedulerGestureEventArgs e)
        {
            if (e.AppointmentInfo != null)
            {
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?",
                    appointment.Subject + " will be deleted.", "Yes", "No");

                if (answer == true)
                {
                    SchedulerStorage.RemoveAppointment(appointment);
                }
            }
        }

        private void ShowAppointmentEditPage_WeekView(AppointmentItem appointment)
        {
            AppointmentEditPage appEditPage = new(appointment, SchedulerStorage);
            Navigation.PushAsync(appEditPage);
        }

        private void ShowNewAppointmentEditPage_WeekView(IntervalInfo info)
        {
            AppointmentEditPage appEditPage = new(info.Start, info.End,
                                                                     info.AllDay, SchedulerStorage);
            Navigation.PushAsync(appEditPage);
        }

       
        public void OnNewStudynEvent(StudynEvent sEvent)
        {
            Console.WriteLine("in CalendarPage.OnNewStudynEvent");
            // On any appointment event, refresh the data
            if (sEvent.EventType == StudynEventType.AppointmentAdd
                || sEvent.EventType == StudynEventType.AppointmentEdit
                || sEvent.EventType == StudynEventType.AppointmentDelete)
            {
                //SchedulerStorage.RefreshData(); //Not sure if this is crashing the app causing an "index out of range" or "handler being used elsewhere" error. The calendarpage does SchedulerStorage.RefreshData() every time it appears anyway, so im going to comment this out for now
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