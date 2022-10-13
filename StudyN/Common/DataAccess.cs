using DevExpress.Maui.Scheduler;

namespace StudyN.Common
{
    public static class DataAccess
    {
        static readonly List<AppointmentItem> _data = new();

        public static IEnumerable<AppointmentItem> GetData()
        {
            return _data;
        }

        public static void LoadData(IEnumerable<AppointmentItem> data)
        {
            _data.Clear();
            foreach(var item in data)
            {
                _data.Add(item);
            }
        }
    }
}
