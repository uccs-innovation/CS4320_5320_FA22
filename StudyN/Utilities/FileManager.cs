using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using StudyN.Models;
using static StudyN.Utilities.StudynEvent;
using StudyN.Common;

namespace StudyN.Utilities
{
    public class FileManager : StudynSubscriber
    {
        static string DIR = FileSystem.AppDataDirectory;
        static string TASK_DIR = DIR + "/tasks/";
        static string COMPLETE_TASK_DIR = DIR + "/completedTask/";

        public FileManager()
        {
            EventBus.Subscribe(this);

            // create directories
            System.IO.Directory.CreateDirectory(TASK_DIR);
            System.IO.Directory.CreateDirectory(COMPLETE_TASK_DIR);

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



        public static void TaskEdited(Guid taskId)
        {
            TasksDeleted(taskId);
            TasksAdded(taskId);
            //Unneeded. Mainly just writes out files in directory for testing purposes. 
            //LoadFileNames();
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
    }
}