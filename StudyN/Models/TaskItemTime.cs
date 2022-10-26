namespace StudyN.Models
{
    public class TaskItemTime
    {
        public TaskItemTime(DateTime start)
        {
            this.start = start;
        }

        public void StopTime(DateTime stop)
        {
            this.stop = stop;
        }


        public DateTime start { get; set; }
        public DateTime stop { get; set; }



    }
}


