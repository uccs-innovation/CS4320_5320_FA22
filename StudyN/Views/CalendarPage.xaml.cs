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
        // Flag to prevent multiple child pages opening
        bool isChildPageOpening = false;

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
            InvalidateMeasure();

            isChildPageOpening = false;

            var notes = SchedulerStorage.GetAppointments(new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)));
            CalendarDataView.LoadDataForNotification(notes.ToList());
            base.OnAppearing();
        }

        private void ShowAppointmentEditPage(AppointmentItem appointment)
        {
            if (!isChildPageOpening)
            {
                isChildPageOpening = true;
                AppointmentEditPage appEditPage = new(appointment, SchedulerStorage);
                Navigation.PushAsync(appEditPage);
            }
        }

        private void ShowNewAppointmentEditPage(IntervalInfo info)
        {
            if (!isChildPageOpening)
            {
                isChildPageOpening = true;
                AppointmentEditPage appEditPage = new(info.Start, info.End, info.AllDay, SchedulerStorage);
                Navigation.PushAsync(appEditPage);
            }
        }

        private async void OnCalendarTap(object sender, SchedulerGestureEventArgs e)
        {
            if (e.IntervalInfo != null)
            {
                if (e.AppointmentInfo == null)
                {
                    ShowNewAppointmentEditPage(e.IntervalInfo);
                    return;
                }
                
                AppointmentItem appointment = e.AppointmentInfo.Appointment;
                bool answer = await DisplayAlert("Are you sure?", appointment.Subject + " should be edited.", "Yes", "No");
                
                if (answer == true)
                {
                    ShowAppointmentEditPage(appointment);
                }
            }
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
                }
            }
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