using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class TaskDataManager
    {
        public TaskItem AddTask(string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {
            TaskItem newTask  = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded);

            TaskList.Add(newTask);

            sendFileUpdate(FileManager.Operation.AddTask, newTask.TaskId, updateFile);

            return newTask;
        }

        public void CompleteTask(Guid taskId, bool updateFile = true)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    TaskList.Remove(task);

                    sendFileUpdate(FileManager.Operation.CompleteTask, taskId, updateFile);

                    return;
                }
            }
        }

        public void DeleteTask(Guid taskId, bool updateFile = true)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    TaskList.Remove(task);

                    sendFileUpdate(FileManager.Operation.AddTask, taskId, updateFile);

                    return;
                }
            }
        }

        public void DeleteListOfTasks(List<Guid> taskIds, bool updateFile = true)
        {
            foreach (Guid id in taskIds)
            {
                DeleteTask(id, false);
            }

            sendFileUpdate(FileManager.Operation.DeleteTask, taskIds, updateFile);
        }


        public void sendFileUpdate(FileManager.Operation op, Guid taskId, bool updateFile)
        {
            List<Guid> taskIdList = new List<Guid>();
            taskIdList.Add(taskId);

            sendFileUpdate(op, taskIdList, updateFile);
        }
        public void sendFileUpdate(FileManager.Operation op, List<Guid> taskIds, bool updateFile)
        {
            if (updateFile)
            {
                // Send update to Filemanager
                FileManager.FILE_OP_QUEUE.Enquue(
                    new FileManager.FileOperation(op, taskIds));
            }
        }

        public ObservableCollection<TaskItem> TaskList { get; private set; }
        private ObservableCollection<TaskItem> CompletedTasks { get; set; }

        public TaskDataManager()
        {
            TaskList = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();
        }
    }
}


