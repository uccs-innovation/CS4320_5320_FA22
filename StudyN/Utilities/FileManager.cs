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
            }

            public List<Guid> TaskIdList { get; set; }
            public Operation Operation { get; set; }
        }

        static string DIR = FileSystem.AppDataDirectory;
        static string TASK_FILENAME = DIR + "tasks/";
        static string COMPLETE_TASK_FILENAME = DIR + "completedTask/";
        

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
            // serialaize tasks into task file
            string fileName = TASK_FILENAME + "task" + taskIdList.First() + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(taskIdList, indent);
            File.WriteAllText(fileName, jsonString);
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
            string completeFileName = COMPLETE_TASK_FILENAME + "completedtask" + taskIdList.First() + ".json";
            var indent = new JsonSerializerOptions { WriteIndented = true };
            //deserializtion code here
            string jsonString = JsonSerializer.Serialize(taskIdList, indent);
            File.WriteAllText(completeFileName, jsonString);
            // output, might be taken out later
            Console.WriteLine("Tasks Completed:");
            foreach (Guid id in taskIdList)
            {
                Console.WriteLine("    " + id.ToString());
            }
        }
    }
}
