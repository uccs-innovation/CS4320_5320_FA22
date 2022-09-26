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
            Item newItem = new Item();
            newItem.Id = "5";
            newItem.Text = "Hard coded item to add";
            newItem.Description = "Description for hard coded item to add";
            newItem.StartTime = DateTime.Today.AddHours(24);
            newItem.EndTime = DateTime.Today.AddHours(32);
            newItem.Value = 40.5;
            ViewModel.Items.Add(newItem);
            Console.WriteLine(ViewModel.Items.Count);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }
    }
}