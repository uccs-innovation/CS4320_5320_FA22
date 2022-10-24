using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyN.Utilities
{
    interface StudynSubscriber
    {
        public void OnNewStudynEvent(StudynEvent sEvent);
    }
}
