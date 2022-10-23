using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Utilities
{
    public class TaskEvent
    {
        public enum TaskEventType
        {
            AddTask,
            EditTask,
            DeleteTask,
            CompleteTask
        }

        public TaskEvent(Guid id, TaskEventType eventType)
        {
            TaskId = id;
            EventType = eventType;
        }
        public Guid TaskId { get; set; }
        public TaskEventType EventType { get; set; }
    }
}
