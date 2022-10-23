using StudyN.Models;
using StudyN.Utilities;
using System.Collections.ObjectModel;

namespace StudyN.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            Title = "Dashboard";
        }

        async public void OnAppearing()
        {
            await EventBus.WaitForTaskEvent();
        }
    }
}