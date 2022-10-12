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

        public TaskItem GetTask(Guid taskId)
        {
            foreach (TaskItem task in TaskList)
            {
                if (task.TaskId == taskId)
                {
                    return task;
                }
            }

            foreach (TaskItem task in CompletedTasks)
            {
                if(task.TaskId == taskId)
                {
                    return task;
                }
            }

            return null;
        }

        public bool EditTask(Guid taskId,
                                string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {
            TaskItem task = GetTask(taskId);

            if(task == null)
            {
                return false;
            }

            task.Name = name;
            task.Description = description;
            task.DueTime = dueTime;
            task.Priority = priority;
            task.CompletionProgress = CompletionProgress;
            task.TotalTimeNeeded = TotalTimeNeeded;

            sendFileUpdate(FileManager.Operation.EditTask, taskId, updateFile);

            return true;
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

                    sendFileUpdate(FileManager.Operation.DeleteTask, taskId, updateFile);

                    return;
                }
            }
        }

        public void DeleteListOfTasks(List<Guid> taskIds, bool updateFile = true)
        {
            foreach (Guid id in taskIds)
            {
                DeleteTask(id);
            }
        }


        public void sendFileUpdate(FileManager.Operation op, Guid taskId, bool updateFile)
        {
            if (updateFile)
            {
                // Send update to Filemanager
                FileManager.FILE_OP_QUEUE.Enquue(
                    new FileManager.FileOperation(op, taskId));
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