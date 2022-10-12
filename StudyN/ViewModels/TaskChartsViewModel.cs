using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using StudyN.Models;

namespace StudyN.ViewModels
{
    class TaskChartsViewModel
    {
        public IReadOnlyList<HoursRemainingItem> HoursRemainingList { get; }

        public TaskChartsViewModel()
        {
            GetData();
            HoursRemainingList = new List<HoursRemainingItem>()
            {
                new HoursRemainingItem(DateTime.Now, 6, 5),
                new HoursRemainingItem(DateTime.Now, 5, 3)
            };

            palette = PaletteLoader.LoadPalette("#975ba5", "#03bfc1", "#f8c855", "#f45a4e",
                                                    "#496cbe", "#f58f35", "#d293fd", "#25a966");
        }
        readonly Color[] palette;
        public Color[] Palette => palette;

        void GetData() // estepanek: left off here
        {
            foreach (TaskItem item in TaskDataManager.)
            {

            }
        }

    }
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
        public DateTime StudyDay { get; }
        public int TotalHoursForDay { get; }
        public int HoursLeftForDay { get; }


        // Constructor
        public HoursRemainingItem(DateTime studyDay, int totalHoursForDay, int hoursLeftForDay)
        {
            this.StudyDay = studyDay;
            this.TotalHoursForDay = totalHoursForDay;
            this.HoursLeftForDay = hoursLeftForDay;
        }
    }    
          
}
