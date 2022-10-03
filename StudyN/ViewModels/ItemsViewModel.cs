using StudyN.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StudyN.ViewModels
{
    public class ItemsViewModel : INotifyPropertyChanged
    {
        readonly CalenderData data;

        public event PropertyChangedEventHandler PropertyChanged;
        public DateTime StartDate { get { return DateTime.Today; } }
        public IReadOnlyList<Item> EventAppointments { get => data.CalenderItems; }

        public ItemsViewModel()
        {
            data = new CalenderData();
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}