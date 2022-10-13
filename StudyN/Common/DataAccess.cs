using DevExpress.Maui.Scheduler;
using System.Runtime.CompilerServices;

namespace StudyN.Common
{
    public static class DataAccess
    {
        static readonly List<AppointmentItem> _data = new();

        /// <summary>
        /// Get all stored AppointmentItems from database
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AppointmentItem> GetData()
        {
            return _data;
        }

        /// <summary>
        /// Load the database with AppointmentItems
        /// </summary>
        /// <param name="data"></param>
        public static void LoadData(IEnumerable<AppointmentItem> data)
        {
            _data.Clear();
            foreach (var item in data)
            {
                _data.Add(item);
            }
        }

        /// <summary>
        /// Remove AppointmentItem from database
        /// </summary>
        public static void DeleteItem(AppointmentItem item)
        {
            _data.Remove(item);
        }
    }
}
