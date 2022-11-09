﻿using Android.Gms.Tasks;
using DevExpress.Maui.Scheduler;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class TaskTimeDataManager
    {
        //lets us know if a task is being timed
        public bool BeingTimed;
        public Guid TaskidBeingTimed;
        public TaskItemTime taskitemtime;
        public String TaskName;
        //initializes the object
        public TaskTimeDataManager()
        {
            this.BeingTimed = false;
        }

        public void StartNew(DateTime datetimetaken, Guid taskid, String taskname)
        {
            this.taskitemtime = new TaskItemTime(datetimetaken, taskid);
            this.BeingTimed = true;
            this.TaskidBeingTimed = taskid;
            this.TaskName = taskname;
        }

        public void StopCurrent(DateTime datetimetaken)
        {
            this.taskitemtime.StopTime(datetimetaken);
            this.BeingTimed = false;
            AddNewTimeTaskItemListOfTimes();
            TaskItem taskitem = GlobalTaskData.TaskManager.GetTask(TaskidBeingTimed);

            TimeSpan difference = this.taskitemtime.stop - this.taskitemtime.start;
            this.taskitemtime.span = difference;
            taskitem.CompletionProgress += GlobalTaskData.TaskManager.SumTimes(difference.Hours, difference.Minutes);
            // make sure minutes don't go above 60
            if(taskitem.GetCompletionProgressMinutes() >= 60)
            {
                taskitem.CompletionProgress = GlobalTaskData.TaskManager.SumTimes((int)taskitem.CompletionProgress, taskitem.GetCompletionProgressMinutes());
            }

            GlobalTaskData.TaskManager.EditTask(taskitem.TaskId,
                                                taskitem.Name,
                                                taskitem.Description,
                                                taskitem.DueTime,
                                                taskitem.Priority,
                                                taskitem.CompletionProgress,
                                                taskitem.TotalTimeNeeded,
                                                taskitem.TimeList);
        }

        public void AddNewTimeTaskItemListOfTimes()
        {
            TaskItem thetaskitem = GlobalTaskData.TaskManager.GetTask(TaskidBeingTimed);

            thetaskitem.TimeList.Add(this.taskitemtime); 

        }



    }
    
   
}