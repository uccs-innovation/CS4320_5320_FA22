using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudyN.Utilities.FileManager;

namespace StudyN.Utilities
{
    static class EventBus
    {
        static private AsyncQueue<TaskEvent> EventQueue = new AsyncQueue<TaskEvent>();
        static private List<TaskSubscriber> Subscribers = new List<TaskSubscriber>();

        public static void PublishEvent(TaskEvent taskEvent)
        {
            EventQueue.Enquue(taskEvent);
        }

        public static void Subscribe(TaskSubscriber subscriber)
        {
            Subscribers.Add(subscriber);
        }

        public static async Task WaitForTaskEvent()
        {
            await foreach (TaskEvent taskEvent in EventQueue)
            {
                foreach (TaskSubscriber subscriber in Subscribers)
                {
                    subscriber.OnNewTaskEvent(taskEvent);
                }
            }
        }


    }
}
