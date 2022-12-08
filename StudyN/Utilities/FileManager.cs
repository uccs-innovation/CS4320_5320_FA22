using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using StudyN.Models;
using static StudyN.Utilities.StudynEvent;

namespace StudyN.Utilities
{
    public class FileManager : StudynSubscriber
    {
        static string DIR = FileSystem.AppDataDirectory;
        static string TASK_DIR = DIR + "/tasks/";
        static string COMPLETE_TASK_DIR = DIR + "/completedTask/";
        static string TASK_DIR_TEST = DIR + "/testForTasks/";
        static string CATEGORY_DIR = DIR + "/categories/"; 
        static string APPT_DIR = DIR + "/appointments/";

        public FileManager()
        {
            EventBus.Subscribe(this);

            // create directories
            System.IO.Directory.CreateDirectory(TASK_DIR);
            System.IO.Directory.CreateDirectory(COMPLETE_TASK_DIR);
            System.IO.Directory.CreateDirectory(CATEGORY_DIR);
            System.IO.Directory.CreateDirectory(APPT_DIR);
        }

        public void OnNewStudynEvent(StudynEvent taskEvent)
        {
            if(taskEvent.EventType == StudynEventType.AddTask)
            {
                TasksAdded(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.EditTask)
            {
                TaskEdited(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.DeleteTask)
            {
                TasksDeleted(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.CompleteTask)
            {
                TasksCompleted(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.CategoryAdd)
            {
                CategoryAdded(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.CategoryEdit)
            {
                CategoryEdited(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.CategoryDelete)
            {
                CategoryDeleted(taskEvent.Id);
            }
            else if (taskEvent.EventType == StudynEventType.SleepTimeChanged)
            {
                SleepTimeChanged();
            }
            else if (taskEvent.EventType == StudynEventType.AppointmentAdd)
            {
                ApptAdded(taskEvent.Id);
                return;
            }
        }

        //This function will take a given task and save it to a new file
        public static void TasksAdded(Guid taskId)
        {
            // serialaize tasks into task file
            string fileName = TASK_DIR + taskId + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            TaskItem task = GlobalTaskData.TaskManager.GetTask(taskId);
            
            string jsonString = JsonSerializer.Serialize(task, indent);


            File.WriteAllText(fileName, jsonString);
            // output, might be taken out later
            //Console.WriteLine("Tasks Added:");
            //Console.WriteLine("    " + taskId.ToString());

        }

        public static void ApptAdded(Guid apptId)
        {

            // serialaize tasks into task file
            string fileName = APPT_DIR + GlobalAppointmentData.CalendarManager.GetAppointment(apptId).Subject + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            Appointment appt = GlobalAppointmentData.CalendarManager.GetAppointment(apptId);

            string jsonString = JsonSerializer.Serialize(appt, indent);

            Console.WriteLine(fileName);
            File.WriteAllText(fileName, jsonString);
            // output, might be taken out later
            //Console.WriteLine("Tasks Added:");
            //Console.WriteLine("    " + taskId.ToString());
            
        }

        //This function will take a task and delete the associated file
        public static void TasksDeleted(Guid taskId)
        {
         
            //Creating the file name
            string fileName = TASK_DIR + taskId + ".json";

            //If the file exists, delete
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                //Console.WriteLine("    " + taskId.ToString());
            }

        }

        //This function will save a task as a completed task
        public static void TasksCompleted(Guid taskId)
        {
            try
            {
                // delete task in task directory, and serialize it in completed tasks
                string fileName = TASK_DIR + taskId + ".json";
                string completeFileName = COMPLETE_TASK_DIR + taskId + ".json";
                var indent = new JsonSerializerOptions { WriteIndented = true };
                TaskItem task = GlobalTaskData.TaskManager.GetTask(taskId);
                File.Delete(fileName);
                string jsonString = JsonSerializer.Serialize(task, indent);
                File.WriteAllText(completeFileName, jsonString);
                // output, might be taken out later
                //Console.WriteLine("Tasks Completed:");
                //Console.WriteLine("    " + taskId.ToString());
            }
            catch (NullReferenceException exception)
            {
                // most likely going to be caused by fileName
                Console.WriteLine(exception.Message);
            }
        }

        public static void TaskEdited(Guid taskId)
        {
            TasksDeleted(taskId);
            TasksAdded(taskId);
            //Unneeded. Mainly just writes out files in directory for testing purposes. 
            //LoadFileNames();
        }

        /// <summary>
        /// Adds json file when a category is added
        /// </summary>
        /// <param name="catId"></param>
        public static void CategoryAdded(Guid catId)
        {
            // serialize category into category file
            string fileName = CATEGORY_DIR + catId + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            AppointmentCategory category = GlobalAppointmentData.CalendarManager.GetAppointmentCategory(catId);
            SerializedAppointmentCategory serializer = new SerializedAppointmentCategory();
            serializer.Id = category.Id;
            serializer.UniqueId = catId;
            serializer.Caption = category.Caption;
            serializer.Color = category.Color.ToHex();
            serializer.PickerXPosition = category.PickerXPosition;
            serializer.PickerYPosition = category.PickerYPosition;
            string jsonString = JsonSerializer.Serialize(serializer, indent);
            File.WriteAllText(fileName, jsonString);
        }

        /// <summary>
        /// Deletes json file pertaining to a category
        /// </summary>
        /// <param name="catId"></param>
        public static void CategoryDeleted(Guid catId)
        {
            // get name of category file
            string fileName = CATEGORY_DIR + catId + ".json";
            // makes sure file exists
            if (File.Exists(fileName))
            {
                // Delete the file
                File.Delete(fileName);
            }
            else
            {
                // else notify the file doesn't exist
                Console.WriteLine("Erorr: File does not exist");
            }
        }

        /// <summary>
        /// Edits a category's json file
        /// </summary>
        /// <param name="catId"></param>
        public static void CategoryEdited(Guid catId)
        {
            // get name of category file
            string fileName = CATEGORY_DIR + catId + ".json";
            // make sure file exists
            if (File.Exists(fileName))
            {
                // serialize new date into category file, might be more wastefule to delete and make new file
                CategoryAdded(catId);
            }
            else
            {
                // else notify the file doesn't exist, and add the file
                Console.WriteLine("Error: File doesn't exist, adding new file");
                CategoryAdded(catId);
            }
        }

        /// <summary>
        /// When SleepTime changes, thoses changes get saved to a file
        /// </summary>
        public static void SleepTimeChanged()
        {
            // serialize sleep time into a file
            string filename = DIR + "/sleepTime.json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(GlobalAppointmentData.CalendarManager.SleepTime, indent);
            File.WriteAllText(filename, jsonString);
        }

        public static string[] LoadTaskFileNames()
        {
            string[] files = { };
            if (Directory.Exists(TASK_DIR))
            {
                files = Directory.GetFiles(TASK_DIR);
            }
            return files;
        }

        public static string[] LoadCompletedFileNames()
        {
            string[] files = { };
            if (Directory.Exists(TASK_DIR))
            {
                files = Directory.GetFiles(COMPLETE_TASK_DIR);
            }
            return files; 
        }

        public static string[] LoadApptFileNames()
        {
            string[] files = { };
            if (Directory.Exists(APPT_DIR))
            {
                Console.WriteLine("file:");
                Console.WriteLine("file:");
                Console.WriteLine("file:");
                Console.WriteLine("file:");
                Console.WriteLine("file:");
                files = Directory.GetFiles(APPT_DIR);
            }

            foreach(string file in files)
            {
                Console.WriteLine("file:");
                Console.WriteLine(file);
            }
            return files;
        }

        /// <summary>
        /// Loads the categories from the category directory
        /// </summary>
        /// <returns></returns>
        public static string[] LoadCategoryFileNames()
        {
            string[] files = { };
            // if directory exists get files from it
            if (Directory.Exists(CATEGORY_DIR))
            {
                files = Directory.GetFiles(CATEGORY_DIR);
            }
            return files;
        }
        //For testing
        public static string[] LoadTaskFileTest(string directoryName)
        {
            string[] files = { };
            if (Directory.Exists(DIR + directoryName))
            {
                files = Directory.GetFiles(DIR + directoryName);
            }
            return files;
        }

        public static string[] LoadFileNames()
        {
            Console.WriteLine("WRITING OUT FILES");
            string[] files = Directory.GetFiles(TASK_DIR);
            foreach (string file in files)
            {
                Console.WriteLine("file:");
                Console.WriteLine(file);
            }
            return files;
        }

        // Method that saves tasks data to specific location for testing
        public static void SaveTaskTestOnApp(Guid taskId)
        {
            // serialaize tasks into task file for testing
            string fileName = TASK_DIR_TEST + taskId + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            TaskItem task = GlobalTaskData.TaskManager.GetTask(taskId);
            string jsonString = JsonSerializer.Serialize(task, indent);
            File.WriteAllText(fileName, jsonString);
        }

        //load any file from a file path on the phone
        public static string[] loadFile(string directoryPath)
        {
            string[] files = { };

            if (Directory.Exists(directoryPath))
            {
                files = Directory.GetFiles(directoryPath);
            }

            return files;
        }

        public static void ApptEdited(Guid taskId)
        {
            Console.WriteLine("YYYYYYYYYYYYYYYY11981911561616516YYYyadfadfadsfsfasdf");
            ApptDeleted(taskId);
            ApptAdded(taskId);
            //Unneeded. Mainly just writes out files in directory for testing purposes. 
            //LoadFileNames();
        }

        //This function will take a task and delete the associated file
        public static void ApptDeleted(Guid apptId)
        {

            //Creating the file name
            string fileName = APPT_DIR + GlobalAppointmentData.CalendarManager.GetAppointment(apptId).Subject + ".json";

            //If the file exists, delete
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                //Console.WriteLine("    " + taskId.ToString());
            }

        }
    }
}