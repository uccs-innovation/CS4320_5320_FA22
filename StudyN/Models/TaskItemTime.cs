namespace StudyN.Models
{
    public class TaskItemTime
    {
        //gets start time of a newly timed item
        public TaskItemTime(DateTime start)
        {
            Console.WriteLine("Starting task tieming");
            this.start = start;
        }

        //gets stop time of a newly timed item
        public void StopTime(DateTime stop)
        {
            Console.WriteLine("ending task timing");
            this.stop = stop;
        }


        public DateTime start { get; set; }
        public DateTime stop { get; set; }



    }
}


