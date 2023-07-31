using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanel
{
    public struct JCPlugin
    {
        
        public string FrendlyName;
        public string FrendlyVersion;

        public string Name;
        public int Version;
        public string Author;
        public string Hash;
        public string Status;
        public string Description;

        public Assembly assembly;
        public List<JCPluginProperty> Properties;
        public bool IsEnabled;
    }
}
