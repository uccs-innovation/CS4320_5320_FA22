using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Android.Widget;
using DevExpress.Maui.DataGrid;
using StudyN.Models;
using StudyN.ViewModels;

namespace StudyN.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageViewModel viewModel = new HomePageViewModel();
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            viewModel.GetDailyList();
            base.OnAppearing();
        }

       
    }
}