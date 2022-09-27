using DevExpress.Maui.Scheduler;
using DevExpress.Utils.Serializing;
using Microsoft.VisualBasic;
using StudyN.Models;
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
            //Console.WriteLine("hi");
            Popup.IsOpen = true;


        }

        void OnPopupAddButtonClicked(object sender, EventArgs args)
        {
            Item newItem = new Item();
            newItem.Id = "5"; //TODO generate unique id's on each add
            newItem.Text = Description.Text;
            newItem.Description = "Desc placeholder";
            newItem.StartTime = DateTime.Today.AddHours(24);
            newItem.EndTime = DateTime.Today.AddHours(32);
            newItem.Value = 40.5;
            ViewModel.Items.Add(newItem);
            Popup.IsOpen = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}