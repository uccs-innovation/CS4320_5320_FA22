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
            try
            {
                //inserts stop time for taskitemtime object
                this.taskitemtime.StopTime(datetimetaken);
                //turns off being timed
                this.BeingTimed = false;
                //getting task
                TaskItem taskitem = GlobalTaskData.TaskManager.GetTask(TaskidBeingTimed);
                
                TimeSpan difference = this.taskitemtime.stop - this.taskitemtime.start;
                this.taskitemtime.span = difference;
                taskitem.TimeWorked += GlobalTaskData.TaskManager.SumTimes(difference.Hours, difference.Minutes);
                // make sure minutes don't go above 60
                if (taskitem.GetMinutesWorked() >= 60)
                {
                    taskitem.TimeWorked = GlobalTaskData.TaskManager.SumTimes((int)taskitem.TimeWorked, taskitem.GetMinutesWorked());
                }
                AddNewTimeTaskItemListOfTimes();

                //Updates hard files
                GlobalTaskData.TaskManager.EditTask(taskitem.TaskId,
                                                    taskitem.Name,
                                                    taskitem.Description,
                                                    taskitem.DueTime,
                                                    taskitem.Priority,
                                                    taskitem.Category,
                                                    taskitem.TimeWorked,
                                                    taskitem.TimeEstimated,
                                                    taskitem.TimeList);
                this.TaskidBeingTimed = Guid.Empty;
            } catch(NullReferenceException) {
                Console.WriteLine("ERROR NULL REFERENCE EXCEPTION");
                Console.WriteLine("Error occured in tasktimemanager - function : StopCurrent()");
            }


        }

        public void AddNewTimeTaskItemListOfTimes()
        {
            try
            {
                TaskItem thetaskitem = GlobalTaskData.TaskManager.GetTask(TaskidBeingTimed);
                thetaskitem.TimeList.Add(this.taskitemtime);
            } catch (NullReferenceException) {
                Console.WriteLine("ERROR NULL REFERENCE EXCEPTION");
                Console.WriteLine("Error occured in tasktimemanager - function : AddNewTaskTImeItemListOfTimes()");
            }


        }



    }
    
   
}