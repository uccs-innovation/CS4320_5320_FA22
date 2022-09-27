using Ical.Net;
using System.Reflection;

namespace StudyN.Common
{
    public static class ICalManager
    {        
        // File added as embedded resource for feature POC
        private static readonly string _fileName = "StudyN.TestData.test.ics";

        /// <summary>
        /// Uses Ical.Net to load events from .ics file
        /// </summary>
        /// <returns>IUniqueComponentList of Ical.Net.CalendarComponents.CalendarEvent</returns>
        public static async Task<Ical.Net.Proxies.IUniqueComponentList<Ical.Net.CalendarComponents.CalendarEvent>> ReadICalFile()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using Stream stream = assembly.GetManifestResourceStream(_fileName);
                using StreamReader reader = new(stream);
                var result = await reader.ReadToEndAsync();
                var calendar = Calendar.Load(result);
                return calendar.Events;
            }
            catch (Exception e)
            {
                // Temporary until logging scheme is established
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
    }
}
