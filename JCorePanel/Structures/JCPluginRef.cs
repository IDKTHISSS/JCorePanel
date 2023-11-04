using JCorePanelBase;
using System.Collections.Generic;

namespace JCorePanel
{
    public struct JCPluginRef
    {
        public string PluginName;
        public bool isEnabled;
        public List<JCPluginProperty> PluginSettings;
    }
}
