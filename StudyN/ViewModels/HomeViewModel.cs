using StudyN.Common;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public IcalViewModel Model { get; set; }
        public HomeViewModel()
        {
            Title = "Dashboard";
            //Items = new ObservableCollection<Item>();
            Model = new IcalViewModel();

        }

        //public ObservableCollection<Item> Items { get; private set; }

        async public void OnAppearing()
        {
            //IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
            //Items.Clear();
            //foreach (Item item in items)
            //{
            //    Items.Add(item);
            //}
        }
    }
}