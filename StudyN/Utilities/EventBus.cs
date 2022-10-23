using StudyN.Models;
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
        static private AsyncQueue<StudynEvent> EventQueue = new AsyncQueue<StudynEvent>();
        
        static private List<StudynSubscriber> Subscribers = new List<StudynSubscriber>();

        public static void PublishEvent(StudynEvent sEvent)
        {
            EventQueue.Enquue(sEvent);
        }

        public static void Subscribe(StudynSubscriber subscriber)
        {
            Subscribers.Add(subscriber);
        }

        public static async Task WaitForStudynEvent()
        {
            await foreach (StudynEvent taskEvent in EventQueue)
            {
                foreach (StudynSubscriber subscriber in Subscribers)
                {
                    subscriber.OnNewStudynEvent(taskEvent);
                }
            }
        }

    }
}
