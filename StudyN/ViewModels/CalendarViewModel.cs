using StudyN.Models;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public CalendarViewModel()
        {
            Title = "Calendar";
            //Items = new ObservableCollection<Item>();

        }

        //public ObservableCollection<Item> Items { get; private set; }

        async public void OnAppearing()
        {
            //IEnumerable<CalendarTask> items = await DataStore.GetItemsAsync(true);
            //Items.Clear();
            //foreach (Item item in items)
            //{
            //Items.Add(item);
            //}
        }
    }
}