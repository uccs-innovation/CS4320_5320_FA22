namespace StudyN.Models
{
    public class TaskItemTime
    {
        //gets start time of a newly timed item
        public TaskItemTime(DateTime start)
        {
            this.start = start;
        }

        //gets stop time of a newly timed item
        public void StopTime(DateTime stop)
        {
            this.stop = stop;
        }


        public DateTime start { get; set; }
        public DateTime stop { get; set; }



    }
}


