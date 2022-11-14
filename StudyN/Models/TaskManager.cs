using Newtonsoft.Json;
﻿using Android.Gms.Tasks;
using Android.Service.Autofill;
using StudyN.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using DevExpress.CodeParser;

namespace StudyN.Models
{
    //This class manages all of our tasks and preforms actions related to them
    public class TaskDataManager
    {

        public TaskItem AddTask(string name,
                                string description,
                                DateTime dueTime,
                                int priority,
                                double CompletionProgress,
                                double TotalTimeNeeded)
        {
            //Creating new task with sent parameters
            TaskItem newTask  = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded,
                                            "");

            //This will add the tasks to the list
            TaskList.Add(newTask);

            // Publish task add event
            EventBus.PublishEvent(
                        new StudynEvent(newTask.TaskId, StudynEvent.StudynEventType.AddTask));

            return newTask;
        }
        //This function will add a new task to our list of tasks
        public TaskItem AddTask(string name,
                               string description,
                               DateTime dueTime,
                               int priority,
                               double CompletionProgress,
                               double TotalTimeNeeded,
                               string recur)
        {
            //Creating new task with sent parameters
            TaskItem newTask = new TaskItem(name,
                                            description,
                                            dueTime,
                                            priority,
                                            CompletionProgress,
                                            TotalTimeNeeded,
                                            recur);




            //This will add the tasks to the list
            TaskList.Add(newTask);

            // Publish task add event
            EventBus.PublishEvent(
                        new StudynEvent(newTask.TaskId, StudynEvent.StudynEventType.AddTask));

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
                                double CompletionProgress,
                                double TotalTimeNeeded,
                                List<TaskItemTime> TimeList = null,
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
            task.TimeList = TimeList;

            // Publish task edit event
            EventBus.PublishEvent(
                        new StudynEvent(taskId, StudynEvent.StudynEventType.EditTask));

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

                    // Publish task complete event
                    EventBus.PublishEvent(
                        new StudynEvent(taskId, StudynEvent.StudynEventType.CompleteTask));

                    return;
                }
            }
        }

        //This function will delete a given task from the normal TaskList
        public void DeleteTask(Guid taskId, bool updateFile = true)
        {
            //Looking for the task based on Id
            foreach (TaskItem task in TaskList)
            {
                //If found
                if (task.TaskId == taskId)
                {
                    //Remove the task from list
                    TaskList.Remove(task);

                    // Publish task delete event
                    EventBus.PublishEvent(
                        new StudynEvent(taskId, StudynEvent.StudynEventType.DeleteTask));

                    return;
                }
            }
        }
        

        //This function will delete every task for the ids avalaible
        public void DeleteListOfTasks(List<Guid> taskIds)
        {
            foreach (Guid id in taskIds)
            {
                DeleteTask(id);
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
                //Console.WriteLine(jsonfiletext);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext); 
                //TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                TaskList.Add(task);

                if (task.TimeList != null)
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Writing out task times");
                    foreach (TaskItemTime tasktime in task.TimeList)
                    {
                        Console.WriteLine("Time Start" + tasktime.start);
                        Console.WriteLine("TimeStop" + tasktime.stop);
                        Console.WriteLine("Timespanned" + tasktime.span);
                    }
                }
            }



            // gets completed tasks
            string[] completedfiles = FileManager.LoadCompletedFileNames();
            foreach (string file in completedfiles)
            {
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext);

                //TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                CompletedTasks.Add(task);
            }

            //gets test tasks
            string[] testFile = FileManager.LoadTaskFileNames();
            foreach (string file in testFile)
            {
                jsonfiletext = File.ReadAllText(file);
                //Console.WriteLine(jsonfiletext);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext);
                TaskListTest.Add(task);

                if (task.TimeList != null)
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Writing out task times");
                    foreach (TaskItemTime tasktime in task.TimeList)
                    {
                        Console.WriteLine("Time Start" + tasktime.start);
                        Console.WriteLine("TimeStop" + tasktime.stop);
                        Console.WriteLine("Timespanned" + tasktime.span);
                    }
                }
            }
        }

        public void LoadFilesIntoListsTest(string dirName)
        {
            string jsonfiletext;

            // gets completed tasks
            string[] taskfilelist = FileManager.LoadTaskFileTest(dirName);
            foreach (string file in taskfilelist)
            {
                jsonfiletext = File.ReadAllText(file);
                //Console.WriteLine(jsonfiletext);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext);
                //TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                TaskList.Add(task);

                if (task.TimeList != null)
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Writing out task times");
                    foreach (TaskItemTime tasktime in task.TimeList)
                    {
                        Console.WriteLine("Time Start" + tasktime.start);
                        Console.WriteLine("TimeStop" + tasktime.stop);
                        Console.WriteLine("Timespanned" + tasktime.span);
                    }
                }
            }



            // gets completed tasks
            string[] completedfiles = FileManager.LoadCompletedFileNames();
            foreach (string file in completedfiles)
            {
                jsonfiletext = File.ReadAllText(file);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext);

                //TaskItem task = JsonSerializer.Deserialize<TaskItem>(jsonfiletext)!;
                CompletedTasks.Add(task);
            }

            //gets test tasks
            string[] testFile = FileManager.LoadTaskFileTest(dirName);
            foreach (string file in testFile)
            {
                jsonfiletext = File.ReadAllText(file);
                //Console.WriteLine(jsonfiletext);
                TaskItem task = JsonConvert.DeserializeObject<TaskItem>(jsonfiletext);
                TaskListTest.Add(task);

                if (task.TimeList != null)
                {
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("--------------------------------");
                    Console.WriteLine("Writing out task times");
                    foreach (TaskItemTime tasktime in task.TimeList)
                    {
                        Console.WriteLine("Time Start" + tasktime.start);
                        Console.WriteLine("TimeStop" + tasktime.stop);
                        Console.WriteLine("Timespanned" + tasktime.span);
                    }
                }
            }
        }

        /// <summary>
        /// Turns record of hours and minutes and makes them doubles
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public double SumTimes(int hours, int minutes)
        {
            // make sure minutes are below 60
            if(minutes >= 60)
            {
                // take 60 out of minutes and add to hours
                while(minutes >= 60)
                {
                    minutes -= 60;
                    hours++;
                }
            }
            // turn minutes into decimals
            double decimalMins = (double)minutes / 60;
            // return hours.minutes
            return (double)hours + decimalMins;
        }

        // Count the number of total tasks due today
        public int NumTasksDueToday()
        {
            int numTasksDue = NumTasksCompletedToday();
            foreach(TaskItem task in TaskList)
            {
                if (task.DueTime.Date == DateTime.Today)
                {
                    numTasksDue++;
                }
            }
            return numTasksDue;
        }

        // Count the number of task completed that were due today
        public int NumTasksCompletedToday()
        {
            int numCompleted = 0;
            foreach(TaskItem task in CompletedTasks)
            {
                if(task.DueTime.Date == DateTime.Today)
                {
                    numCompleted++;
                }
            }
            return numCompleted;
        }

        public ObservableCollection<TaskItem> TaskList { get; private set; }
        private ObservableCollection<TaskItem> CompletedTasks { get; set; }

        public ObservableCollection<TaskItem> TaskListTest { get; private set; }

        //This constructor will create the normal TaskList and the list for
        //completed tasks, CompletedTasks
        public TaskDataManager()
        {
            TaskList = new ObservableCollection<TaskItem>();
            CompletedTasks = new ObservableCollection<TaskItem>();
            TaskListTest = new ObservableCollection<TaskItem>();
        }

    }
}