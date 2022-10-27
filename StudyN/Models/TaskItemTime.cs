namespace StudyN.Models
{
    public class TaskItemTime
    {
        //gets start time of a newly timed item
        public TaskItemTime(DateTime start, Guid taskid)
        {
            Console.WriteLine("Starting task tieming: datetime gotten: " + start);
            this.start = start;
        }

        //gets stop time of a newly timed item
        public void StopTime(DateTime stop)
        {
            Console.WriteLine("ending task timing: datetime gotten: " + stop);
            this.stop = stop;
        }


        public DateTime start { get; set; }
        public DateTime stop { get; set; }



    }
}


