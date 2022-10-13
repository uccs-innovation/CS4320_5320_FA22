namespace StudyN.Models
{
    //This class is used to create an object that will allow us to identify where data is held and retrieve/store it 
    public static class GlobalTaskData
    {
        //This is for the object that stores all of our tasks
        public static TaskDataManager TaskManager { get; set; }

        //This will be used to keep track of a task we are currently editing
        public static TaskItem ToEdit { get; set; }
    }
}


