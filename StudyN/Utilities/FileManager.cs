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
            DeleteTask,
            CompleteTask
        }
        public class FileOperation
        {
            public FileOperation(Operation operation, List<Guid> idList)
            {
                TaskIdList = idList;
                Operation = operation;
                // create directories
                string taskDir = System.IO.Path.Combine(DIR, "tasks");
                string completeDir = System.IO.Path.Combine(DIR, "completedTasks");
                System.IO.Directory.CreateDirectory(taskDir);
                System.IO.Directory.CreateDirectory(completeDir);
            }

            public List<Guid> TaskIdList { get; set; }
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
                    TasksAdded(op.TaskIdList);
                }
                else if (op.Operation == Operation.DeleteTask)
                {
                    TasksDeleted(op.TaskIdList);
                }
                else if (op.Operation == Operation.CompleteTask)
                {
                    TasksCompleted(op.TaskIdList);
                }
            }
        }

        public static void TasksAdded(List<Guid> taskIdList)
        {
            // what naming new files look like
            //string fileName = TASK_DIR + "task" + taskIdList.First() + ".json";
            // output, might be taken out later
            Console.WriteLine("Tasks Added:");
            foreach(Guid id in taskIdList)
            {
                Console.WriteLine("    " + id.ToString());
            }
        }

        public static void TasksDeleted(List<Guid> taskIdList)
        {
            Console.WriteLine("Tasks Deleted:");
            foreach (Guid id in taskIdList)
            {
                Console.WriteLine("    " + id.ToString());
            }
        }

        public static void TasksCompleted(List<Guid> taskIdList)
        {
           // what naming new files looks like
           //string fileName = TASK_DIR + "task" + taskIdList.First() + ".json";
           //string completeFileName = COMPLETE_TASK_DIR + "completedtask" + taskIdList.First() + ".json";
           // output, might be taken out later
           Console.WriteLine("Tasks Completed:");
           foreach (Guid id in taskIdList)
           {
               Console.WriteLine("    " + id.ToString());
           }
        }
    }
}
