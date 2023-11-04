using JCorePanelBase;
using System.Collections.Generic;

namespace JCorePanel
{
    public struct JCTaskItem
    {
        public string TaskName;
        public string TaskDescription;
        public List<string> AccountNames;
        public List<JCTask> EventList;
    }
}
