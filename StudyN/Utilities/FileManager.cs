using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using StudyN.Models;

namespace StudyN.Utilities
{
    public static class FileManager
    {
        public enum Operation
        {
            AddTask,
            EditTask,
            DeleteTask,
            CompleteTask
        }
        public class FileOperation
        {
            public FileOperation(Operation operation, Guid id)
            {
                TaskId = id;
                Operation = operation;
                // create directories
                System.IO.Directory.CreateDirectory(TASK_DIR);
                System.IO.Directory.CreateDirectory(COMPLETE_TASK_DIR);
            }

            public Guid TaskId { get; set; }
            public Operation Operation { get; set; }
            
        }

        static string DIR = FileSystem.AppDataDirectory;
        static string TASK_DIR = DIR + "/tasks/";
        static string COMPLETE_TASK_DIR = DIR + "/completedTask/";
        

        public static AsyncQueue<FileOperation> FILE_OP_QUEUE = new AsyncQueue<FileOperation>();
        
        public static async Task WaitForFileOp()
        {
            await foreach(FileOperation op in FILE_OP_QUEUE)
            {
                if(op.Operation == Operation.AddTask)
                {
                    TasksAdded(op.TaskId);
                }
                else if (op.Operation == Operation.EditTask)
                {
                    TaskEdited(op.TaskId);
                }
                else if (op.Operation == Operation.DeleteTask)
                {
                    TasksDeleted(op.TaskId);
                }
                else if (op.Operation == Operation.CompleteTask)
                {
                    TasksCompleted(op.TaskId);
                }
            }
        }

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

        public static void TasksDeleted(Guid taskId)
        {
         
            string fileName = TASK_DIR + taskId + ".json";

      
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                //Console.WriteLine("    " + taskId.ToString());
            }

        }


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