using Android.Gms.Tasks;
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
        public bool TaskIsBeingTimed;
        public Guid TheTaskidBeingTimed;
        public TaskItemTime taskitemtime;

        //initializes the object
        public TaskTimeDataManager()
        {
            this.TaskIsBeingTimed = false;
        }

        public void StartNew(DateTime datetimetaken, Guid taskid)
        {
            this.taskitemtime = new TaskItemTime(datetimetaken, taskid);
            this.TaskIsBeingTimed = true;
            this.TheTaskidBeingTimed = taskid;
        }

        public void StopCurrent(DateTime datetimetaken)
        {
            this.taskitemtime.StopTime(datetimetaken);
            this.TaskIsBeingTimed = false;
            AddNewTimeTaskItemListOfTimes();
            TaskItem taskitem = GlobalTaskData.TaskManager.GetTask(TheTaskidBeingTimed);

            TimeSpan difference = this.taskitemtime.stop - this.taskitemtime.start;
            this.taskitemtime.span = difference;
            taskitem.CompletionProgress += GlobalTaskData.TaskManager.SumTimes(difference.Hours, difference.Minutes);

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
            TaskItem thetaskitem = GlobalTaskData.TaskManager.GetTask(TheTaskidBeingTimed);

            thetaskitem.TimeList.Add(this.taskitemtime); 

        }



    }
    
   
}