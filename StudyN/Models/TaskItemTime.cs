namespace StudyN.Models
{
    public class TaskItemTime
    {
        //gets start time of a newly timed item
        public TaskItemTime(DateTime start, Guid taskid)
        {
            Console.WriteLine("Starting task tieming: datetime gotten: " + start);
            try
            {
                this.start = start;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            Console.WriteLine("After start set.");
        }

        //gets stop time of a newly timed item
        public void StopTime(DateTime stop)
        {
            Console.WriteLine("ending task timing: datetime gotten: " + stop);
            try
            {
                this.stop = stop;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            Console.WriteLine("After stop set.");
        }


        public DateTime start { get; set; }
        public DateTime stop { get; set; }
        public TimeSpan span { get; set; }



    }
}


