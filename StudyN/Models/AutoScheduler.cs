namespace StudyN.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using StudyN.Models;

public class AutoScheduler
{
    private ObservableCollection<TaskItem> Tasklist { get; set; }
    private int[] weightAssoc; //weightAssoc[0] corresponds to TaskList[0], weightAssoc[1] corresponds to TaskList[1]...
    private DateTime timeBlockAssoc; //timeBlockAssoc[0] corresponds to TaskList[0], timeBlockAssoc[1] corresponds to TaskList[1]...

    public AutoScheduler( ObservableCollection<TaskItem> TL )
	{
        Tasklist = TL;
        weightAssoc = new int[Tasklist.Count];
    }

    //Compute each items calendar position based on its associated weight
    void computeCalendarPosition()
    {

    }

    void associateWeights()
    {

    }

    double calculateWeight(TaskItem task)
    {
        //Higher weight means item of more importance, IE schedule earlier
        double weight = 1;
        double remainingMinutesNeeded = task.TotalTimeNeeded * (1 - task.CompletionProgress / 100); //Assuming task.TotaltimeNeeded is in minutes,
                                                                                                    //and assuming the numerical value of completion
                                                                                                    //progresses represents a percent
                                                                                                    //IE 85 = 85% complete

        //Amount of time between (now + estimated time remaining to complete task), and task due date
        double dueDistance = ( (DateTime.Now.AddMinutes(remainingMinutesNeeded)) - task.DueTime ).TotalMinutes;
        if (dueDistance > 0) //Item is possible to complete BEFORE deadline
        {
            weight = weight / dueDistance;   //As dueDistance trends to infinity, weight trends to 0. Smaller weights have less scheduling importance.
            weight = weight + task.Priority; //Assuming priority is on a scale from 1 to 10, 1 being not important, 10 being very important.
                                             //If an item has a large dueDistance, it will still have a large weight if it has a high priority
        }                                    //Think of it like this: If an item is super far away in dueDistance, essentially it gets sorted by its priority.

        else { weight = 9999999999; } //Item is NOT possible to complete before deadline. Assign it highest priority. 

        return weight;
    }



}