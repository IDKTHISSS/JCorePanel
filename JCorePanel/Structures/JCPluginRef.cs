using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanel
{
    public struct JCPluginRef
    {
        public string PluginName;
        public bool isEnabled;
        public List<JCPluginProperty> PluginSettings;
    }
}
