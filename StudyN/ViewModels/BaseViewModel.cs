using StudyN.Models;
using StudyN.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudyN.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        readonly CalendarDataStore data;

        bool isBusy = false;
        string title = string.Empty;

        public IReadOnlyList<CalendarItem> Employees { get => data.CalendarItems; }

        public INavigationService Navigation => DependencyService.Get<INavigationService>();

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { SetProperty(ref this.isBusy, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { SetProperty(ref this.title, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}