using StudyN.Common;
using Ical.Net.CalendarComponents;
using DevExpress.Maui.DataForm;

namespace StudyN.ViewModels
{
    public class IcalViewModel : BaseViewModel
    {
        public Ical.Net.Proxies.IUniqueComponentList<CalendarEvent> Events { get; set; }

        [DataFormDisplayOptions(IsVisible = false)]
        public Command Import { get; }


        [DataFormDisplayOptions(IsVisible = false)]
        public Command LoadCommand { get; }

        /// <summary>
        /// 
        /// </summary>
        public IcalViewModel()
        {
            Import = new Command(OnImport, ValidateSave);
            LoadCommand = new Command(OnLoadEvents);
            PropertyChanged += (_, __) => Import.ChangeCanExecute();
        }

        bool ValidateSave()
        {
            return Events != null && Events.Any();
        }
        
        public async void OnImport()
        {
            Events = await ICalManager.ReadICalFile();
            foreach(var calE in Events)
            {
                await DataStore.AddItemAsync(calE);
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
