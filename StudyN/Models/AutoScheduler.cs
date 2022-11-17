namespace StudyN.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Android.Accessibilityservice.AccessibilityService;
using DevExpress.CodeParser;
using DevExpress.Utils;
using DevExpress.XtraRichEdit.Model.History;
using StudyN.Models;
using StudyN.Utilities;
using static StudyN.Utilities.StudynEvent;

//Because of the minuteMap, this iteration of the autoScheduler doesn't seem to have a need for blocks
public class AutoScheduler : StudynSubscriber
{
    private DateTime baseTime; //The base time the autoscheduler will use to do all its calculations
    ObservableCollection<Appointment> appts;
    ObservableCollection<TaskItem> tasks;
    private minuteSnapshot[] minuteMap;
    public bool taskPastDue;
    public List<TaskItem> pastDueTasks;

    public AutoScheduler()
    {
        taskPastDue = false;
        tasks = GlobalTaskData.TaskManager.TaskList;
        appts = GlobalAppointmentData.CalendarManager.Appointments;
        pastDueTasks = new List<TaskItem>();
        minuteMap = new minuteSnapshot[40320]; //40320 minutes in 4 weeks. AutoScheduler will only scheduler out 4 weeks.
        for(int i = 0; i < minuteMap.Length; i++) { minuteMap[i] = new minuteSnapshot(); }
        baseTime = DateTime.Now;
    }
   
    //Put appointments from the global appointments list into the minute mapping, for future use in scheduling
    private void MapAppointments()
    {
        Console.WriteLine("autoScheduler.MapAppointments()");
        foreach(Appointment appt in appts)
        {
            if (appt.From == "autoScheduler") //If the appointment is from the autoScheduler, delete it from the calendar so we can reschedule it without duplicating it
            {
                Console.WriteLine("rescheduling appointment from autoScheduler");
                //GlobalAppointmentData.CalendarManager.DeleteAppointment(minuteMap[i].id); //To be able to delete an appointment, the calnedarManager needs a function to do so
            } 
        }

        foreach (Appointment appt in appts)
        {
            DateTime start, end;
            start = appt.Start; end = appt.End;
            if(end < baseTime.AddMinutes(40320) && end > baseTime) //If the appointment falls wholly within the 4 week autoscheduling time frame
            {
                if(start < baseTime) { start = baseTime; } //If appointment is going on right now, treat it as if it starts right now
                int offset = (int)(start - baseTime).TotalMinutes;
                int span = (int)(end - start).TotalMinutes; 
                for(int i = offset; i < offset + span; i++)
                {
                    minuteMap[i].id = appt.UniqueId; 
                    minuteMap[i].from = "appts";
                    minuteMap[i].name = appt.Subject; 
                }
            }
        }
    }

    //Map each task minute by minute back from its duedate. If something is already scheduled for that minute, look back to the minute before it.
    //It will map higher important tasks first. Therefore lower importance tasks will be scheduled around the higher importance ones.
    private void MapTasks()
    {
        Console.WriteLine("autoScheduler.MapTasks()");
        IOrderedEnumerable<TaskItem> sortedTasks = sortByWeight();
        foreach (TaskItem task in sortedTasks) //Schedule the higher weight (IE high importance) tasks first, so if something is unscheduleable it will be a lower weight task
        {
            if(task.DueTime < baseTime.AddMinutes(40320) && task.DueTime > baseTime) //Task is due within the 4 weeks the scheduler will schedule
            {
                int offset = (int)(task.DueTime - baseTime).TotalMinutes;
                int minutesMapped = 0;
                while(minutesMapped < task.TotalTimeNeeded * 60)
                {
                    if(offset < 0) //meaning task cannot be completed unless its scheduled before baseTime (aka in the past)
                    {
                        Console.WriteLine("UNSCHEDUABLE TASK");
                        pastDueTasks.Add(task);
                        taskPastDue = true;
                        break;
                    }
                    if(minuteMap[offset].id == null) //If nothing has been mapped to this spot yet, put it here
                    {
                        minuteMap[offset].id = task.TaskId;
                        minuteMap[offset].from = "autoScheduler";
                        minuteMap[offset].name = task.Name;
                        minutesMapped++;
                        offset--;
                    }
                    else //There IS something already mapped here. Dont map it but keep looking backwards
                    {
                        //This else statement can get a LOT more complicated in the future, depending on how we want to handle collisions
                        offset--;
                    }
                }
            }
        }
    }

    //All contiguous minute mappings should be transformed into a continous appointment. IE indexes in the minute mapping that are next to each other and have the same Guid should be combined together.
    private List<Appointment> CoalesceMinuteMapping()
    {
        Console.WriteLine("autoScheduler.CoalesceMinuteMapping()");
        List<Appointment> coalescedAppointments = new List<Appointment>();

        Guid? curGuid = null;
        int guidStart = 0;
        for(int i = 0; i < minuteMap.Length; i++)
        {
            if(curGuid != minuteMap[i].id)
            {
                if(i > 0 && minuteMap[i - 1].id != null) 
                {
                    Appointment appt = new Appointment();
                    appt.Start = baseTime.AddMinutes(guidStart);
                    appt.End = baseTime.AddMinutes(i - 1); //The previous index was the last index with the previous guid
                    appt.Subject = minuteMap[i - 1].name;
                    appt.UniqueId = (Guid)minuteMap[i - 1].id;
                    appt.From = minuteMap[i - 1].from;
                    coalescedAppointments.Add(appt);
                }
                curGuid = minuteMap[i].id;
                guidStart = i;
            }
        }

        return coalescedAppointments;
    }

    private void AddToCalendar(List<Appointment> appts)
    {
        Console.WriteLine("autoScheduler adding to calendar");

        int defaultLabelId = 1;  // default Label/Color set to "StudyN Time" 
        int defaultStatusId = 2; // default Status set to "Blocked" 

        foreach (Appointment appt in appts)
        {
            GlobalAppointmentData.CalendarManager.CreateAppointment(appt.Subject, 
                                                                    appt.Start, 
                                                                    appt.End - appt.Start, 
                                                                    -1, // location field not used
                                                                    defaultLabelId,
                                                                    defaultStatusId,
                                                                    appt.UniqueId, 
                                                                    "autoScheduler"); //idk what "room" is for CreateAppointment() method
        }
    }

    private IOrderedEnumerable<TaskItem> sortByWeight()
    {
        List<TaskItem> tempTasks = new List<TaskItem>();
        foreach (TaskItem task in tasks)
        {
            TaskItem tempTask = task;
            tempTask.weight = calculateWeight(tempTask);
            tempTasks.Add(tempTask);
        }

        return tempTasks.OrderByDescending(f => f.weight);  //https://stackoverflow.com/questions/11190220/sort-c-sharp-list-of-objects-by-variable        
    }

    private double? calculateWeight(TaskItem task)
    {
        //Higher weight means item of more importance, IE schedule earlier
        double? weight = 1;
        //double remainingMinutesNeeded = task.TotalTimeNeeded * (1 - task.CompletionProgress / 100);   //Assuming task.TotaltimeNeeded is in minutes,
        //and assuming the numerical value of completion
        //progresses represents a percent
        //IE 85 = 85% complete

        double remainingMinutesNeeded = task.TotalTimeNeeded * 60; //completionProgress seems to be bugged, so currently not using. BUT WE SHOULD USE THE ABOVE LINE IDEALLY


        //Amount of days between (now + estimated time remaining to complete task), and task due date
        double leadtime = (task.DueTime - (DateTime.Now.AddMinutes(remainingMinutesNeeded))).TotalDays;

        Console.WriteLine(task.Name + " leadtime: " + leadtime);
        if (leadtime > 0) //Item is possible to complete BEFORE deadline
        {
            weight = weight / leadtime;   //As leadtime trends to infinity, weight trends to 0. Smaller weights have less scheduling importance.
            weight = weight + task.Priority; //Assuming priority is on a scale from 1 to 10, 1 being not important, 10 being very important.
                                             //If an item has a large dueDistance, it will still have a large weight if it has a high priority
        }                                    //Think of it like this: If an item is super far away in dueDistance, essentially it gets sorted by its priority.

        else //Item is NOT possible to complete before deadline. Give it negative weight so it will be scheduled past its deadline, and be detected as unscheduable.
        {
            //taskPastDue = true;
            //pastDueTasks.Add(task);
            weight = null;
        }

        return weight;
    }

    public void run(Guid id)
    {
        Console.WriteLine("Running autoScheduler");
        refreshData();
        MapAppointments();
        MapTasks();
        AddToCalendar( CoalesceMinuteMapping() );
        Console.WriteLine("Done running autoScheduler");

        for (int i = 0; i < minuteMap.Length; i++)
        {
            if (minuteMap[i].id != null)
            {
                Console.WriteLine("Minute: " + i + "  Name:" + minuteMap[i].name);
            }
        }
    }
   
     private void refreshData()
    {
        taskPastDue = false;
        pastDueTasks = new List<TaskItem>();
        minuteMap = new minuteSnapshot[40320];
        for (int i = 0; i < minuteMap.Length; i++) { minuteMap[i] = new minuteSnapshot(); }
        baseTime = DateTime.Now;
    }

    public void OnNewStudynEvent(StudynEvent taskEvent)
    {
        Console.WriteLine("autoScheduler.OnNewStudynEvent");
        switch (taskEvent.EventType)
        {
            // On any add, edit, or modify task/appointment, rerun the scheduler
            case StudynEventType.AddTask:
            case StudynEventType.EditTask:
            case StudynEventType.DeleteTask:
                //run(taskEvent.Id);
                break;
            case StudynEventType.AppointmentAdd:
            case StudynEventType.AppointmentEdit:
            case StudynEventType.AppointmentDelete:
            {
                run(taskEvent.Id); 
                break;
            }
            case StudynEventType.CompleteTask:
            {
                // Console Logging just so we can see in the output something is happening
                Console.WriteLine("Scheduler Has CompleteTask Events!");
                GlobalAppointmentData.CalendarManager.TaskCompleted(taskEvent.Id);
                break;
            }
        }
    }

}

internal class minuteSnapshot
{   
    public Guid? id;
    public string name;
    public string from; //Where it is from. IE cretaed by autoScheduler? From calendar? ics file?
    public minuteSnapshot() { id = null; from = ""; name = ""; }
}