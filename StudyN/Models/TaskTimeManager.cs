using DevExpress.Maui.Scheduler;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudyN.Models
{
    public class TaskTimeDataManager
    {
        //lets us know if a task is being timed
        public bool TaskIsBeingTimed;
        public Guid TheTaskidBeingTimed;
        public TaskItemTime taskitemtime;

        //initializes the object
        public TaskTimeDataManager()
        {
            this.TaskIsBeingTimed = false;
        }

        public void UpdateTaskItemTime(DateTime datetimetaken, Guid taskid)
        {
            if(this.TaskIsBeingTimed)
            {
                this.taskitemtime.StopTime(datetimetaken);
                this.TaskIsBeingTimed = false;
                AddNewTimeTaskItemListOfTimes();

                TaskItem taskitem = GlobalTaskData.TaskManager.GetTask(TheTaskidBeingTimed);
                GlobalTaskData.TaskManager.EditTask(taskitem.TaskId,
                                                    taskitem.Name,
                                                    taskitem.Description,
                                                    taskitem.DueTime,
                                                    taskitem.Priority,
                                                    taskitem.CompletionProgress,
                                                    taskitem.TotalTimeNeeded,
                                                    taskitem.TimeList);


                //add to list of task times here
            } else {
                this.taskitemtime = new TaskItemTime(datetimetaken, taskid);
                this.TaskIsBeingTimed = true;
                this.TheTaskidBeingTimed = taskid;
            }
        }

        public void AddNewTimeTaskItemListOfTimes()
        {
            TaskItem thetaskitem = GlobalTaskData.TaskManager.GetTask(TheTaskidBeingTimed);


            thetaskitem.TimeList.Add(this.taskitemtime); 

        }



    }
    
    
    
/**
    //This class manages all of our tasks and preforms actions related to them
    public class TaskDataManager
    {
        //This function will add a new task to our list of tasks
        public TaskItem AddTask(string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {
            //Creating new task with sent parameters
            TaskItem newTask  = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded);

            //This will add the tasks to the list
            TaskList.Add(newTask);

            //Creating a new file to store the task with
            sendFileUpdate(FileManager.Operation.AddTask, newTask.TaskId, updateFile);

            return newTask;
        }

        //This will return a requested task using its id
        public TaskItem GetTask(Guid taskId)
        {
            
            //Checking each item in the current task list
            foreach (TaskItem task in TaskList)
            {
                //If the task is found, return the task
                if (task.TaskId == taskId)
                {
                    return task;
                }
            }

            //Checking the completed tasks list
            foreach (TaskItem task in CompletedTasks)
            {
                if(task.TaskId == taskId)
                {
                    return task;
                }
            }

            //If not found in either list, return null
            return null;
        }

        //This function will recieve data to update a task with
        public bool EditTask(Guid taskId,
                                string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                int CompletionProgress,
                                int TotalTimeNeeded,
                                bool updateFile = true)
        {
            //Retrieving the task
            TaskItem task = GetTask(taskId);

            //If the task is not found, return false to end early
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

            //Updating the tasks's file
            sendFileUpdate(FileManager.Operation.EditTask, taskId, updateFile);

            return true;
        }

        //This function will move a given task to the completed task list
        public void CompleteTask(Guid taskId, bool updateFile = true)
        {
            //Searching for the task in the normal TaskList
            foreach (TaskItem task in TaskList)
            {
                //If it is found in the list
                if (task.TaskId == taskId)
                {
                    //Set the tasks to completed, add it to the completed list, and remove
                    //it from the normal one
                    task.Completed = true;
                    CompletedTasks.Add(task);
                    TaskList.Remove(task);

                    //Update the files
                    sendFileUpdate(FileManager.Operation.CompleteTask, taskId, updateFile);

                    return;
                }
            }
        }

        //This function will delete a given task from the normal TaskList
        public void DeleteTask(Guid taskId, bool updateFile = true)
        {
            //Looking for the task based on TaskId
            foreach (TaskItem task in TaskList)
            {
                //If found
                if (task.TaskId == taskId)
                {
                    //Remove the task from list
                    TaskList.Remove(task);

                    //Deleteing the associated file
                    sendFileUpdate(FileManager.Operation.DeleteTask, taskId, updateFile);

                    return;
                }
            }
        }
        

        //This function will delete every task for the ids avalaible
        public void DeleteListOfTasks(List<Guid> taskIds, bool updateFile = true)
        {
            foreach (Guid id in taskIds)
            {
                DeleteTask(id);
            }
        }

        //This function will interact with the file manager to preform the correct file action
        public void sendFileUpdate(FileManager.Operation op, Guid taskId, bool updateFile)
        {
            //Provided we are allowed to update the file (using updatefile as an indicator),
            // begin the interaction with the FileManager
            if (updateFile)
            {
                // Send update to Filemanager
                FileManager.FILE_OP_QUEUE.Enquue(
                    new FileManager.FileOperation(op, taskId));
            }
        }

        
        public void LoadFilesIntoLists()
        {
            string jsonfiletext;

            // gets completed tasks
            string[] taskfilelist = FileManager.LoadTaskFileNames();
            foreach (string file in taskfilelist)
            {
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                TaskList.Add(task);
            }

            // gets completed tasks
            string[] completedfiles = FileManager.LoadCompletedFileNames();
            foreach (string file in completedfiles)
            {
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                CompletedTasks.Add(task);
            }
        }

        public ObservableCollection<TaskItem> TaskList { get; private set; }
        private ObservableCollection<TaskItem> CompletedTasks { get; set; }

        //This constructor will create the normal TaskList and the list for
        //completed tasks, CompletedTasks
        public TaskDataManager()
        {
            TaskList = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();
        }

    }**/
}