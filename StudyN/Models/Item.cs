using System.Collections.ObjectModel;

namespace StudyN.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CalenderData
    {
        public ObservableCollection<Item> CalenderItems { get; private set; }
    }
}