using Ical.Net.CalendarComponents;

namespace StudyN.Models
{
    public class StudyNCalendarEvent
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Created { get; set; }
        public string Uid { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string LastModified { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public RecurrenceRule Recurrence { get; set; }
        public string RecurrenceRulesBy { get; set; }
        public string RecurrenceRulesCountUntil { get; set; }
        //public IList<string> RecurrenceRule { get; set; }

        public StudyNCalendarEvent() { }

        public StudyNCalendarEvent(CalendarEvent calE)
        {
            Start = calE.Start.ToString();
            End = calE.End.ToString();
            Created = calE.Created.ToString();
            Uid = calE.Uid;
            Description = RemoveTags(calE.Description);
            Summary = calE.Summary;
            LastModified = calE.LastModified.ToString();
            Location = calE.Location;
            Status = calE.Status;
            Recurrence = new RecurrenceRule(calE.RecurrenceRules);
        }

        private static string RemoveTags(string str)
        {
            // regex which match tags
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");

            // replace all matches with empty string
            str = rx.Replace(str, "");
            return str;
        }
    }

    public class RecurrenceRule
    {
        public int Count { get; }

        public string Frequency { get; }

        /// <summary>
        /// List of days
        /// </summary>
        public IList<string> ByDay { get; }

        /// <summary>
        /// List of months
        /// </summary>
        public IList<string> ByMonth { get; }

        /// <summary>
        /// List of hours
        /// </summary>
        public IList<string> ByHour { get; }

        public IList<string> ByMinute { get; }

        public IList<string> BySecond { get; }

        public string Until { get; }

        /// <summary>
        /// Parse the Recurrence Pattern provided by Ical.Net
        /// </summary>
        /// <param name="rRule"></param>
        public RecurrenceRule(IList<Ical.Net.DataTypes.RecurrencePattern> rRule)
        {
            Frequency = string.Empty;

            // Initialize the various lists
            ByDay = new List<string>();
            ByMonth = new List<string>();
            ByHour = new List<string>();
            ByMinute = new List<string>();
            BySecond = new List<string>();

            // Set recurrence values if any exist
            if (rRule.Any())
            {
                var r = rRule.FirstOrDefault();
                if (r.ByDay.Any())
                {
                    foreach(var d in r.ByDay)
                    {
                        ByDay.Add(d.DayOfWeek.ToString());
                    }
                }
                if (r.ByMonth.Any())
                {
                    foreach(var m in r.ByMonth)
                    {
                        ByMonth.Add(m.ToString());
                    }
                }
                if (r.ByHour.Any())
                {
                    foreach(var h in r.ByHour)
                    {
                        ByHour.Add(h.ToString());
                    }
                }
                if (r.ByMinute.Any())
                {
                    foreach(var min in r.ByMinute)
                    {
                        ByMinute.Add(min.ToString());
                    }
                }
                if (r.BySecond.Any())
                {
                    foreach(var sec in r.BySecond)
                    {
                        BySecond.Add(sec.ToString());
                    }
                }
                
                Frequency = r.Frequency.ToString();
                Count = r.Count;
                Until = r.Until.ToString();;
            }
        }
    }
}
