using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Utilities
{
    abstract public class TaskSubscriber
    {
        public abstract void OnNewTaskEvent(TaskEvent taskEvent);
    }
}
