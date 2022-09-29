using StudyN.Common;
using Ical.Net.CalendarComponents;
using DevExpress.Maui.DataForm;
using StudyN.Models;

namespace StudyN.ViewModels
{
    public class IcalViewModel : BaseViewModel
    {
        [DataFormDisplayOptions(IsVisible = false)]
        public List<CalendarEvent> Events { get; set; }

        [DataFormDisplayOptions(IsVisible = false)]
        public Command Import { get; }


        [DataFormDisplayOptions(IsVisible = false)]
        public Command LoadCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public IcalViewModel()
        {
            Events = new List<CalendarEvent>();
            Import = new Command(OnImport, ValidateSave);
            LoadCommand = new Command(OnLoadEvents);
            PropertyChanged += (_, __) => Import.ChangeCanExecute();
        }

        bool ValidateSave()
        {
            return Events != null && Events.Any();
        }
        
        public void OnImport()
        {
            var events = ICalManager.ReadICalFile();
            foreach(var calE in events)
            {
                var item = DataStore.GetItem(calE.Uid);
                if(item == null)
                    DataStore.AddItemAsync(calE);
            }
        }

        public void OnLoadEvents()
        {
            IsBusy = true;
            try
            {
                Events.Clear();
                var items = DataStore.GetItems(true);
                if(items != null && items.Any())
                {
                    foreach(var item in items)
                    {
                        Events.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
