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
            // what naming new files look like
            //string fileName = TASK_DIR + "task" + taskIdList.First() + ".json";
            // output, might be taken out later
            Console.WriteLine("Tasks Added:");
            Console.WriteLine("    " + taskId.ToString());
        }

        public static void TasksDeleted(Guid taskId)
        {
            Console.WriteLine("Tasks Deleted:");
            Console.WriteLine("    " + taskId.ToString());
        }

        public static void TasksCompleted(Guid taskId)
        {
           // what naming new files looks like
           //string fileName = TASK_DIR + "task" + taskIdList.First() + ".json";
           //string completeFileName = COMPLETE_TASK_DIR + "completedtask" + taskIdList.First() + ".json";
           // output, might be taken out later
           Console.WriteLine("Tasks Completed:");
           Console.WriteLine("    " + taskId.ToString());
        }

        public static void TaskEdited(Guid taskId)
        {
            Console.WriteLine("Task Edited:");
            Console.WriteLine("    " + taskId.ToString());
        }
    }
}
