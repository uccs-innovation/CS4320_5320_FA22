using DevExpress.Maui.Charts;
using StudyN.ViewModels;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace StudyN.Views
{

    public partial class AnalyticsPage : ContentPage
    {

        public AnalyticsPage()
        {
            InitializeComponent();
        }

        AnalyticsViewModel ViewModel { get; set; }

        protected override void OnAppearing()
        {
            BindingContext = ViewModel = new AnalyticsViewModel();
            EventBreakdown.Series[0].LegendTextPattern = "{L}: {V}";
            if (ViewModel.CalendarEvents.Count > 5)
            {
                EventBreakdown.Series[0].VisibleInLegend = true;
                EventBreakdown.Hint.Enabled = true;
            }
            else
            {
                EventBreakdown.Series[0].VisibleInLegend = true;
                EventBreakdown.Hint.Enabled = false;
            }
        }
    }
}
