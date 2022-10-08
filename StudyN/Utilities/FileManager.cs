using System;
using System.IO;
using System.Threading.Tasks;

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
        static string TASK_FILENAME = "tasks.json";
        static string COMPLETE_TASK_FILENAME = "completedTask.json";

        public static AsyncQueue<FileOperation> FILE_OP_QUEUE = new AsyncQueue<FileOperation>();

        public static async Task WaitForFileOp()
        {
            await foreach(FileOperation op in FILE_OP_QUEUE)
            {
                if(op.Operation == Operation.AddTask)
                    Console.WriteLine("Adding Task");
                if (op.Operation == Operation.DeleteTask)
                    Console.WriteLine("Deleting Task");
                if (op.Operation == Operation.CompleteTask)
                    Console.WriteLine("Completing Task");
            }
        }

        static string generateTaskJson()
        {
            return "";
        }

        static string generateCompletedTaskJson()
        {
            return "";
        }
        static async Task WriteJsonAync()
        {
            Console.WriteLine("Async Write File has started");
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(DIR, TASK_FILENAME)))
            {
                Console.WriteLine("Writing to: " + TASK_FILENAME);
                await outputFile.WriteAsync(generateTaskJson());
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(DIR, COMPLETE_TASK_FILENAME)))
            {
                Console.WriteLine("Writing to: " + COMPLETE_TASK_FILENAME);
                await outputFile.WriteAsync(generateCompletedTaskJson());
            }
            Console.WriteLine("Async Write File has completed");
        }
    }
}
