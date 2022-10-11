using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                CreateJsonFiles();
            }

            public List<Guid> TaskIdList { get; set; }
            public Operation Operation { get; set; }
        }

        static string DIR = FileSystem.AppDataDirectory;
        static string TASK_FILENAME = DIR + "tasks.json";
        static string COMPLETE_TASK_FILENAME = DIR + "completedTask.json";
        

        public static AsyncQueue<FileOperation> FILE_OP_QUEUE = new AsyncQueue<FileOperation>();

        /// <summary>
        /// creates json files for listing tasks
        /// </summary>
        private static void CreateJsonFiles()
        {
            if (!File.Exists(TASK_FILENAME) && !File.Exists(COMPLETE_TASK_FILENAME))
            {
                File.Create(TASK_FILENAME);
                File.Create(COMPLETE_TASK_FILENAME);
            }
        }

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
            // serialaize tasks into task file
            var indent = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(taskIdList, indent);
            File.WriteAllText(TASK_FILENAME, jsonString);
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
            // deserialize task in task file, and serialize it in completed tasks
            var indent = new JsonSerializerOptions { WriteIndented = true };
            //deserializtion code here
            string jsonString = JsonSerializer.Serialize(taskIdList, indent);
            File.WriteAllText(COMPLETE_TASK_FILENAME, jsonString);
            // output, might be taken out later
            Console.WriteLine("Tasks Completed:");
            foreach (Guid id in taskIdList)
            {
                Console.WriteLine("    " + id.ToString());
            }
        }
    }
}
