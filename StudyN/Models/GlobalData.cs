namespace StudyN.Models
{
    //This class is used to create an object that will allow us to identify where data is held and retrieve/store it 
    public static class GlobalTaskData
    {
        //This is for the object that manages all of our tasks
        public static TaskDataManager TaskManager { get; set; }

        //This will be used to keep track of a task we are currently editing
        public static TaskItem ToEdit { get; set; }
    }

    public static class GlobalAppointmentData
    {
        //This is for the object that manages all of our appointments
        public static CalendarManager CalendarManager { get; set; }
        //This will be used to keep track of a category we are editing
        public static AppointmentCategory EditCategory { get; set; }
    }

    public static class GlobalAutoScheduler
    {
        //This is for the object that auto schedules stuff
        public static AutoScheduler AutoScheduler { get; set; }

    }
}


