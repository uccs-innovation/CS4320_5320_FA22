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
        public double Value { get; set; }
    }

    public class CalenderData
    {
        Item CreateCalenderItem(string Id, string Text, string Description, DateTime StartTime, DateTime EndTime, double Value)
        {
            Item calenderItem = new Item();
            calenderItem.Id = Id;
            calenderItem.Text = Text;
            calenderItem.Description = Description;
            calenderItem.StartTime = StartTime;
            calenderItem.EndTime = EndTime;
            calenderItem.Value = Value;
            return calenderItem;
        }

        public ObservableCollection<Item> CalenderItems { get; private set; }

    }
}