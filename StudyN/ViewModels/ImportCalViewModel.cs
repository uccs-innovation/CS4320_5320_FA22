using DevExpress.Maui.DataForm;
using StudyN.Models;
using StudyN.Services;
using StudyN.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.ViewModels
{
    public class ImportCalViewModel : BaseViewModel
    {
        public const string ViewName = "ImportCalPage";
        string name;
        [DataFormDisplayOptions(IsVisible = false)]
        public IcalViewModel Model { get; }
        NavigationService NavigationService { get; set; }

        [DataFormDisplayOptions(IsVisible = false)]
        public ObservableCollection<StudyNCalendarEvent> Events { get; }

        /// <summary>
        /// 
        /// </summary>
        public ImportCalViewModel()
        {
            Title = "Import Calendar";
            NavigationService = new NavigationService();
            Model = new IcalViewModel();
            Events = new ObservableCollection<StudyNCalendarEvent>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnSubmit()
        {
            Model.OnImport();
            _ = NavigationService.NavigateToAsync<EventDataGridViewModel>();
        }
    }
}
