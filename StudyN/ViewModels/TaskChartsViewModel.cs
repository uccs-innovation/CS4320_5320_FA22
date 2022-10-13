using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Graphics;
using StudyN.Models;

namespace StudyN.ViewModels
{
    class TaskChartsViewModel
    {
        public IReadOnlyList<HoursRemainingItem> HoursRemainingList { get; }
        //public ObservableCollection<HoursRemainingItem> HoursRemainingList { get; private set; }

        public TaskChartsViewModel()
        {
 
            DateTime currDay = DateTime.Today;
            //HoursRemainingList = new ObservableCollection<HoursRemainingItem>()
            HoursRemainingList = new List<HoursRemainingItem>()
            {
                new HoursRemainingItem(currDay, 0, 3),
                new HoursRemainingItem(currDay.AddDays(1), 4, 6),
                new HoursRemainingItem(currDay.AddDays(3), 3, 5) 
                
            };

            //// estepanek: I know that this should be in it's own method :-(
            //int totalHoursForDay = 0;
            //int totalHoursRemaining = 0;

            //// estepanek: I'm assuming the list is already sorted by day
            //foreach (TaskItem task in TaskList) // estepanek: can't figure out how to access this list
            //{
            //    if (task.DueTime.Day < currDay.Day)
            //    {
            //        continue; // cycle
            //    }
                
            //    if (task.DueTime.Day == currDay.Day)
            //    {
            //        totalHoursRemaining += (task.TotalTimeNeeded - task.CompletionProgress);
            //        totalHoursForDay += task.TotalTimeNeeded;
            //        continue; // cycle
            //    }

            //    if (totalHoursForDay == 0) // don't bother adding if total is zero
            //    {
            //        continue; // cycle
            //    }

            //    HoursRemainingItem item = new HoursRemainingItem(currDay, totalHoursForDay, totalHoursRemaining);
            //    HoursRemainingList.Add(item);

            //    currDay = currDay.AddDays(1);
            //    totalHoursRemaining = 0;
            //    totalHoursForDay = 0;
            //} // end foreach
            //if (totalHoursForDay > 0) // still stuff left to add
            //{
            //    HoursRemainingItem item = new HoursRemainingItem(currDay, totalHoursForDay, totalHoursRemaining);
            //    HoursRemainingList.Add(item);
            //}

            palette = PaletteLoader.LoadPalette("#975ba5", "#03bfc1", "#f8c855", "#f45a4e",
                                                    "#496cbe", "#f58f35", "#d293fd", "#25a966");
        }
        readonly Color[] palette;
        public Color[] Palette => palette;

    }

    // estepanek: might need this later?
    static class PaletteLoader
    {
        public static Color[] LoadPalette(params string[] values)
        {
            Color[] colors = new Color[values.Length];
            for (int i = 0; i < values.Length; i++)
                colors[i] = Color.FromArgb(values[i]);
            return colors;
        }
    }

    class HoursRemainingItem
    {
        // Properties
        public DateTime StudyDay { get; set; }
        public int TotalHoursForDay { get; set; }
        public int HoursLeftForDay { get; set; }


        // Constructor
        public HoursRemainingItem(DateTime studyDay, int totalHoursForDay, int hoursLeftForDay)
        {
            Console.WriteLine("In the HoursRemainingItem constructor");
            this.StudyDay = studyDay;
            this.TotalHoursForDay = totalHoursForDay;
            this.HoursLeftForDay = hoursLeftForDay;
        }
    }
}