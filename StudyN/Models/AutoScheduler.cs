namespace StudyN.Models;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DevExpress.Utils;
using StudyN.Models;

public class AutoScheduler
{ 
    public bool taskPastDue;
    public List<TaskItem> pastDueTasks;
    private List<TaskItem> TaskBlockList;

    //public ObservableCollection<CalendarAppointment> CalendarList { get; set; }
    private ObservableCollection<TaskItem> Tasklist { get; set; }
    private List<double> weightAssoc;   //weightAssoc[0] corresponds to TaskBlockList[0], weightAssoc[1] corresponds to TaskBlockList[1]...
    public List<DateTime> calPosAssoc;  //calPosAssoc[0] corresponds to TaskBlockList[0], calPosAssoc[1] corresponds to TaskBlockList[1]...

    //calPosAssoc holds the START TIMES of each task. The end time can be calculated using 
    //task.TotalTimeNeeded * (1 - task.CompletionProgress / 100), and then adding that to the start time
    private List<int> AllCurBlocks;

    public AutoScheduler( ObservableCollection<TaskItem> TL )
	{
        taskPastDue = false;
        pastDueTasks = new List<TaskItem>();
        Tasklist = TL;
        //CalendarList = GlobalData.cs.calendarManager
    }
   
    private void associateCalendarPositions()
    {

        //If a multiple task blocks are apart of the same task (TaskId is the same), then add tolerance so they don't all get scheduled at the same time.
        //Tolerance is based on how many TaskBlocks are apart of the same Task. If there are 10 TaskBlocks from the same Task, the first TaskBlock will get scheduled with
        //a tolerance of 1, and the 10th TaskBlock will be scheduled with a Tolerance of 10 (so it will be placed farther from the "ideal" scheduled time).
        Guid prevTaskId = Guid.Empty;
        int numTaskIdMatches = 0;
        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            if(prevTaskId == TaskBlockList[i].TaskId)
            {
                numTaskIdMatches++;
            }

            int tolerance = numTaskIdMatches;
            calculateCalendarPosition(i, tolerance);
            prevTaskId = TaskBlockList[i].TaskId;
        }
    }

    //Compute each items calendar position based on its associated weight
    private void calculateCalendarPosition(int index, int tolerance)
    {
        double weight = weightAssoc[index];
        if (weight > 0)
        {
            double spread = weight / tolerance; //Higher tolerance = lower spread = scheduled later
            DateTime startTime = DateTime.Now.AddDays(1 / spread); //If the spread is small, schedule it far in the future
                                                                   //IE, if spread is 1, schedule it in 1 day. If spread is 10, schedule it in .1 days. If spread is HUGE, schedule it now.

            //If TaskBlock goes past dueDate, keep making the task block earlier until it fits
            while(startTime.AddHours(1) > TaskBlockList[index].DueTime)
            {
                startTime = startTime.AddHours(-1); 
            }

            calPosAssoc[index] = startTime;
        }

        //Task Cannot be scheduled. Schedule it past its duetime so it can be detected as unschedulable.
        else 
        {
            calPosAssoc[index] = TaskBlockList[index].DueTime.AddHours(1);
        } 
    }

    private void associateWeights()
    {
        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            weightAssoc[i] = calculateWeight(TaskBlockList[i]);
        }
    }

    private double calculateWeight(TaskItem task)
    {
        //Higher weight means item of more importance, IE schedule earlier
        double weight = 1;
        //double remainingMinutesNeeded = task.TotalTimeNeeded * (1 - task.CompletionProgress / 100);   //Assuming task.TotaltimeNeeded is in minutes,
                                                                                                        //and assuming the numerical value of completion
                                                                                                        //progresses represents a percent
                                                                                                        //IE 85 = 85% complete

        double remainingMinutesNeeded = task.TotalTimeNeeded * 60; //completionProgress seems to be bugged, so currently not using. BUT WE SHOULD USE THE ABOVE LINE IDEALLY


        //Amount of days between (now + estimated time remaining to complete task), and task due date
        double leadtime = ( task.DueTime - (DateTime.Now.AddMinutes(remainingMinutesNeeded)) ).TotalDays; 

        Console.WriteLine("Time now: " + DateTime.Now);
        Console.WriteLine("dueTime: " + task.DueTime);
        Console.WriteLine("leadtime: " + leadtime);
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
            weight = -1; 
        }

        return weight;
    }


    //This is where conflict resolution will happen. If an event gets scheduled during blackout time, put it in the first
    //available slot after blackout time.
    //If two things get scheduled at the same time or overlapping, put the one with more weight first.
    //If two things have the same weight (realllyyyyyy unlikely), just randomly put one before the other.
    private void addToCalendar()
    {
        Console.WriteLine("TODO: implement autoScheduler.addToCalendar");
    }

    private void breakTasksIntoBlocks()
    {
        int numTask = Tasklist.Count;
        TaskBlockList = new List<TaskItem>();
        int totalNumBlocks = 0;

        foreach(var task in Tasklist)
        {
            int length = task.TotalTimeNeeded; //TODO: update to be based on TIME REMAINING, once we figure out whether "completion progress" is how many hours have been logged, or a percent

            //Assuming TotalTimeNeeded is in hours
            int numBlocksForTask = length;
            totalNumBlocks += numBlocksForTask;

            for(int i = 0; i < numBlocksForTask; i++)
            {
                TaskItem taskBlock = new TaskItem(task.Name, task.Description, task.DueTime, task.Priority, task.CompletionProgress, 1); //1 = totalTimeNeeded (1 hour per block)
                taskBlock.TaskId = task.TaskId;
                
                TaskBlockList.Add(taskBlock);

            }
        }

        //Must initialize Assoc lists here, because they depend on the size of TaskBlockList
        weightAssoc = new List<double>( new double[TaskBlockList.Count] ); 
        calPosAssoc = new List<DateTime>( new DateTime[TaskBlockList.Count] );

    }
    //Before? or After? or during?? a rightPush blocks should be compressed! The leftPush inherently compresses the blocks, as it only incrementally pushes them left
    private void compressBlocks()
    {

    }
   
    private Tuple<int, int> findOverlap() 
    { 
        for (int i = 0; i < calPosAssoc.Count; i++)
        {
            for (int j = 0; j < calPosAssoc.Count; j++)
            {
                if (calPosAssoc[j] <= calPosAssoc[i] && calPosAssoc[i] <= calPosAssoc[j].AddHours(1) && i != j)
                {
                    return Tuple.Create(i, j);
                }
            }
        }

        return null;
    }

    private void pushLeft(int i, int j, int randomness)
    {
        int lowerWeighted;
        int higherWeighted;
        if (weightAssoc[i] < weightAssoc[j]) { lowerWeighted = i; higherWeighted = j; }
        else { lowerWeighted = j; higherWeighted = i; }
        
        calPosAssoc[higherWeighted] = calPosAssoc[lowerWeighted].AddHours(-1 - randomness); //Add in randomness incase a task block keeps getting scheduled back and forth between two blocks infinitely
    }

    private void pushRight()
    {
        DateTime now = DateTime.Now;
        DateTime farthestLeft = now;
        for(int i = 0; i < calPosAssoc.Count; i++)
        {
            if(calPosAssoc[i] < farthestLeft) { farthestLeft = calPosAssoc[i]; }
        }

        TimeSpan rightPush = DateTime.Now - farthestLeft;

        if(rightPush.Ticks > 0)
        {
            for(int i = 0; i < calPosAssoc.Count; i++)
            {
                calPosAssoc[i] = calPosAssoc[i] + rightPush;
            }
        }

        for(int i = 0; i < calPosAssoc.Count; i++)
        {
            if(calPosAssoc[i].AddHours(1) > TaskBlockList[i].DueTime)
            {
                Console.WriteLine("ISSUE SCHEDULING TASK BLOCK");
                calPosAssoc[i] = DateTime.Now.AddDays(-2); //idk where to schedule tasks that cant be completed ontime, so im just putting them in the past for now
                taskPastDue = true;
                pastDueTasks.Add(TaskBlockList[i]);
            }
        }
    }

    private void driveOverlapCorrection()
    {
        //Using Random() somewhat like simulated annealing. To deal with cases where a TaskBlock gets sandwiched between a higher and lower weighted one (when that happens it will just move up and down on the schedule infinitely)
        Random random = new Random(); 
        Tuple<int, int> overlaps = findOverlap();
        int findOverlapCalls = 1;
        while (overlaps != null)
        {
            Console.WriteLine("Fixing overlap");

            if (findOverlapCalls % 10 == 0)
            {
                pushLeft(overlaps.Item1, overlaps.Item2, random.Next(0, findOverlapCalls/100)); //If a taskblock gets stuck between a higherWeight and lowerWeight block, this should very slowly increase the distance it is rescheduled (eventually getting it unstuck)
            } 
            else { pushLeft(overlaps.Item1, overlaps.Item2, 0); }

            overlaps = findOverlap();
            findOverlapCalls++;
        }

        pushRight();
    }

    private void checkForUnscheduables()
    {
        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            if (calPosAssoc[i].AddHours(1) > TaskBlockList[i].DueTime)
            {
                Console.WriteLine("ISSUE SCHEDULING TASK BLOCK");
                calPosAssoc[i] = DateTime.Now.AddDays(-2); //idk where to schedule tasks that cant be completed ontime, so im just putting them in the past for now
                taskPastDue = true;
                pastDueTasks.Add(TaskBlockList[i]);
            }
        }
    }

    private void handleMaxTasksInADay()
    {
        //Loop through TaskBlockList
        //Check if more than 4 TaskItems have the same GUID in one day
        //If they do, spread them apart more if possible
        //If not possible, dont change it

        //NOTES:
        //calPosAssoc is a list of the startTimes for each taskBlock.

        foreach (var task in Tasklist)
        {
            Guid curId = task.TaskId;

            int numInDay = 0;
            DateTime curDate = new DateTime();

            AllCurBlocks = new List<int>();

            for (int i = 0; i < TaskBlockList.Count; i++)
            {
                if(TaskBlockList[i].TaskId == curId)
                {
                    AllCurBlocks.Add(i);
                }
            }

            for (int i = 0; i < AllCurBlocks.Count; i++)
            {

                Console.WriteLine(TaskBlockList[AllCurBlocks[i]].Name + ", Weight: " + weightAssoc[AllCurBlocks[i]] + ", startTime: " + calPosAssoc[AllCurBlocks[i]] + ", date:" + calPosAssoc[AllCurBlocks[i]]);

            }
        }
    }

    private void refreshArrays()
    {
        taskPastDue = false;
        pastDueTasks = new List<TaskItem>();
        TaskBlockList = new List<TaskItem>();

        weightAssoc = new List<double>();
        calPosAssoc = new List<DateTime>();
    }

    public void run()
    {
        Console.WriteLine("started running auto scheduler");
        refreshArrays();
        breakTasksIntoBlocks();
        associateWeights();
        associateCalendarPositions();
        driveOverlapCorrection();
        checkForUnscheduables();
        handleMaxTasksInADay();
        addToCalendar();

        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            Console.WriteLine(TaskBlockList[i].Name + ", Weight: " + weightAssoc[i] + ", startTime: " + calPosAssoc[i]);

        }

        Console.WriteLine("done running auto scheduler");
    }


}