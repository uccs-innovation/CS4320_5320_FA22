using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;

namespace StudyN.Models
{
    public class Task
    {
        public Task(string name)
        {
            this.Name = name;
        }

        public bool Completed { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int TimeNeeded { get; set; }
    }

    public class TaskData
    {
        void GenerateTasks()
        {
            ObservableCollection<Task> result = new ObservableCollection<Task>();
            result.Add(
                new Task("HW: Pitch your Application Idea")
                {
                    Completed = true,
                    Id = 1,
                    Description = "Pitch your appilcation idea...",
                    DueDate = DateTime.Today,
                    TimeNeeded = 3
                }
            );
            result.Add(
                new Task("HW: Technology Proof of Concept")
                {
                    Completed = false,
                    Id = 2,
                    Description = "Prove your technology works...",
                    DueDate = DateTime.Today,
                    TimeNeeded = 7
                }
            );
            result.Add(
                new Task("HW: Prototype of Key Features")
                {
                    Completed = false,
                    Id = 3,
                    Description = "Build a prototype of the feature...",
                    DueDate = DateTime.Today,
                    TimeNeeded = 5
                }
            );
            Tasks = result;
        }

        public ObservableCollection<Task> Tasks { get; private set; }

        public TaskData()
        {
            GenerateTasks();
        }
    }
}
