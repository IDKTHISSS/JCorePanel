using JCorePanelBase;
using System.Collections.Generic;
using System.Reflection;

namespace JCorePanel
{
    public struct JCPlugin
    {

        public string FrendlyName;
        public string FrendlyVersion;

        public string Name;
        public int Version;
        public string Author;
        public string Description;

        public Assembly assembly;
        public List<JCPluginProperty> Properties;
        public bool IsEnabled;
    }
}
