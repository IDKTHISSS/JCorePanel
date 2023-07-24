using System;
using System.Collections.Generic;
using System.Linq;
using JCorePanelBase;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanel
{
    public struct JCTaskItem
    {
        public string TaskName;
        public string TaskStatus;
        public List<string> AccountNames;
        public List<JCTask> EventList;
    }
}
