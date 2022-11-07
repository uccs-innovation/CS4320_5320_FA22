using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class DonutChartItem
    {
        public string Name { get; set; }
        public int Percentage { get; set; }

        public DonutChartItem(string name, int percentage)
        {
            Name = name;
            Percentage = percentage;
        }
    }
}
