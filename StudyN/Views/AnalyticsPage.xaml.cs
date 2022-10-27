using DevExpress.Maui.Charts;
using StudyN.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudyN.Views
{

    public partial class AnalyticsPage : ContentPage
    {

        public AnalyticsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new AnalyticsViewModel();

        }

        AnalyticsViewModel ViewModel { get; set; }

        protected override void OnAppearing()
        {
            BindingContext = ViewModel = new AnalyticsViewModel();
        }
    }
}