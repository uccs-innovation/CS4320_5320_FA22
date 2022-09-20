using StudyN.Models;
using System.Collections.ObjectModel;

namespace StudyN.Services
{
    public class CalendarDataStore
    {
        public ObservableCollection<CalendarItem> CalendarItems { get; private set; }

        public CalendarDataStore()
        {

        }

        public async Task<bool> AddCalendarItemAsync(CalendarItem CalendarItem)
        {
            this.CalendarItems.Add(CalendarItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateCalendarItemAsync(CalendarItem CalendarItem)
        {
            var oldCalendarItem = this.CalendarItems.Where((CalendarItem arg) => arg.Id == CalendarItem.Id).FirstOrDefault();
            this.CalendarItems.Remove(oldCalendarItem);
            this.CalendarItems.Add(CalendarItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCalendarItemAsync(string id)
        {
            var oldCalendarItem = this.CalendarItems.Where((CalendarItem arg) => arg.Id == id).FirstOrDefault();
            this.CalendarItems.Remove(oldCalendarItem);

            return await Task.FromResult(true);
        }

        public async Task<CalendarItem> GetCalendarItemAsync(string id)
        {
            return await Task.FromResult(this.CalendarItems.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<CalendarItem>> GetCalendarItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(this.CalendarItems);
        }
    }
}