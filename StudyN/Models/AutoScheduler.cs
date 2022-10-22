namespace StudyN.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using StudyN.Models;

public class AutoScheduler
{ 
    public bool taskPastDue;
    public List<TaskItem> pastDueTasks;
    private List<TaskItem> TaskBlockList;
    private ObservableCollection<TaskItem> Tasklist { get; set; }
    private List<double> weightAssoc;   //weightAssoc[0] corresponds to TaskBlockList[0], weightAssoc[1] corresponds to TaskBlockList[1]...
    public List<DateTime> calPosAssoc;  //calPosAssoc[0] corresponds to TaskBlockList[0], calPosAssoc[1] corresponds to TaskBlockList[1]...

                                    //calPosAssoc holds the START TIMES of each task. The end time can be calculated using 
                                    //task.TotalTimeNeeded * (1 - task.CompletionProgress / 100), and then adding that to the start time

    public AutoScheduler( ObservableCollection<TaskItem> TL )
	{
        taskPastDue = false;
        pastDueTasks = new List<TaskItem>();
        Tasklist = TL;
        //weightAssoc = new List<double>();
        //calPosAssoc = new List<DateTime>();
    }
   
    private void associateCalendarPositions()
    {
        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            calculateCalendarPosition(i);
        }
    }

    //Compute each items calendar position based on its associated weight
    private void calculateCalendarPosition(int index)
    {
        double weight = weightAssoc[index];
        if (weight > 0)
        {
            DateTime startTime = DateTime.Now.AddDays(1 / weight); //If the weight is small, schedule it far in the future
            calPosAssoc[index] = startTime;                        //IE, if weight is 1, schedule it in 1 day. If weight is 10, schedule it in .1 days. If weight is HUGE, schedule it now.
        
            //Maybe its better to calculate BACK from the duetime, rather than FORWARD from right now?
            //Or maybe I can check if the calendar position lets it get completed before its due date, and if not then we can schedule it so it gets done exactly on duetime.
        }

        else { DateTime startTime = DateTime.Now.AddDays(7); calPosAssoc[index] = startTime; } //If weight is really really small, then schedule it for a week from now. 
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

        else //Item is NOT possible to complete before deadline. Assign it highest priority. 
        {
            taskPastDue = true;
            pastDueTasks.Add(task);
            weight = 9999999999; 
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
        addToCalendar();

        for(int i = 0; i < TaskBlockList.Count; i++)
        {
            Console.WriteLine(TaskBlockList[i].Name + ", Weight: " + weightAssoc[i] + ", startTime: " + calPosAssoc[i]);

        }

        Console.WriteLine("done running auto scheduler");
    }


}