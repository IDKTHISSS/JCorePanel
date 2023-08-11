using JCorePanelBase;
using JCorePanelBase.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamKit2.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using static SteamKit2.DepotManifest;

namespace JCorePanel
{
    public static class PluginsManager
    {
        public static List<JCPlugin> PluginsList = new List<JCPlugin>();
        public static JArray PluginStatusList = new JArray();
        public static List<JCPlugin> GetAllPlugins()
        {

            if(!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins")))
            {
                return new List<JCPlugin>();
            }
            PluginStatusList = LoadAllStatus();
            string[] dllFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");
            foreach (string dllFile in dllFiles)
            {
                string PluginStatus = GetPluginStatus(Utils.CalculateSHA512Hash(dllFile));
                if (PluginStatus == "Virus") continue;
                if ((PluginStatus == "Not Verified" || PluginStatus == "Unsafe") && !ConfigMenager.PanelConfig.DeveloperMode) continue;

                Assembly assembly = Assembly.LoadFrom(dllFile);

                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsClass && type.IsSealed && type.IsAbstract && type.Name == "JCPluginConfig")
                    {
                        JCPlugin PluginInfo = new JCPlugin();

                        FieldInfo field = type.GetField("PLUGIN_NAME", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Name = field.GetValue(null) as string;
                        }
                        field = type.GetField("PLUGIN_FRENDLY_VERSION", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.FrendlyVersion = field.GetValue(null) as string;
                        }
                        field = type.GetField("PLUGIN_VERSION", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Version = Convert.ToInt32(field.GetValue(null));
                        }
                        field = type.GetField("PLUGIN_DESCRIPTION", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Description = field.GetValue(null) as string;
                        }
                        field = type.GetField("PLUGIN_FRENDLY_NAME", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.FrendlyName = field.GetValue(null) as string;
                        }

                        field = type.GetField("PLUGIN_SETTINGS", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Properties = field.GetValue(null) as List<JCPluginProperty>;
                            foreach (var plugin in ConfigMenager.PanelConfig.PluginsSettings)
                            {
                                if (plugin.PluginName == PluginInfo.Name)
                                {
                                    Utils.AddOrUpdateProperties(PluginInfo.Properties, plugin.PluginSettings);
                                }
                            }
                            
                        }

                        field = type.GetField("PLUGIN_AUTHOR", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Author = field.GetValue(null) as string;
                        }
                        PluginInfo.Hash = Utils.CalculateSHA512Hash(dllFile);

                        foreach(var plugin in ConfigMenager.PanelConfig.PluginsSettings)
                        {
                            if(plugin.PluginName == PluginInfo.Name)
                            {
                                if(plugin.isEnabled)
                                    PluginInfo.IsEnabled = true;
                            }
                        }
                        PluginInfo.Status = PluginStatus;
                        PluginInfo.assembly = assembly;
                        if (!ConfigMenager.PanelConfig.PluginsSettings.Exists(item => item.PluginName == PluginInfo.Name))
                        {
                            ConfigMenager.PanelConfig.PluginsSettings.Add(new JCPluginRef { PluginName = PluginInfo.Name, isEnabled = false, PluginSettings = PluginInfo.Properties });
                            ConfigMenager.SaveSettings();
                        }

                        PluginsList.Add(PluginInfo);
                    }
                }
            }

            return PluginsList;
        }

        public static void EnablePlugin(JCPlugin Plugin)
        {
            for (int i = 0; i < PluginsList.Count; i++)
            {
                if (PluginsList[i].Name == Plugin.Name)
                {
                    var tempPlugin = PluginsList[i];
                    tempPlugin.IsEnabled = true;
                    PluginsList[i] = tempPlugin;

                    foreach (var plugin in ConfigMenager.PanelConfig.PluginsSettings.ToList())
                    {
                        if (plugin.PluginName == Plugin.Name)
                        {
                            JCPluginRef newPlugin = plugin;
                            newPlugin.isEnabled = true;
                            ConfigMenager.PanelConfig.PluginsSettings.Remove(plugin);
                            ConfigMenager.PanelConfig.PluginsSettings.Add(newPlugin); 
                            ConfigMenager.SaveSettings();
                        }
                    }
                    ConfigMenager.SaveSettings();
                }
            }
        }
        public static void EnablePluginByName(string PluginName)
        {
            for (int i = 0; i < PluginsList.Count; i++)
            {
                if (PluginsList[i].Name == PluginName)
                {
                    var tempPlugin = PluginsList[i];
                    tempPlugin.IsEnabled = true;
                    PluginsList[i] = tempPlugin;

                    
                }
            }
        }
        public static void DisablePlugin(JCPlugin Plugin)
        {
            for (int i = 0; i < PluginsList.Count; i++)
            {
                if (PluginsList[i].Name == Plugin.Name)
                {
                    var tempPlugin = PluginsList[i];
                    tempPlugin.IsEnabled = false;
                    PluginsList[i] = tempPlugin;


                    foreach (var plugin in ConfigMenager.PanelConfig.PluginsSettings.ToList())
                    {
                        if (plugin.PluginName == Plugin.Name)
                        {
                            JCPluginRef newPlugin = plugin;
                            newPlugin.isEnabled = false;
                            ConfigMenager.PanelConfig.PluginsSettings.Remove(plugin);
                            ConfigMenager.PanelConfig.PluginsSettings.Add(newPlugin);
                            ConfigMenager.SaveSettings();
                        }
                    }
                    ConfigMenager.SaveSettings();

                }
            }
        }
        public static JCPlugin GetPluginByName(string PluginName)
        {
            foreach (var cPlugin in PluginsList)
            {
                if (cPlugin.Name == PluginName)
                {
                    return cPlugin;
                }
            }
            return new JCPlugin();
        }
        public static bool PluginIsEnabled(JCPlugin Plugin)
        {
            foreach (var cPlugin in PluginsList)
            {
                if (cPlugin.Name == Plugin.Name)
                {
                    return cPlugin.IsEnabled;
                }
            }
            return false;
        }
        public static List<JCPlugin> GetActivePlugins()
        {
            List<JCPlugin> ActivePlugins = new List<JCPlugin>();
            foreach (var Plugin in PluginsList)
            {
                if (Plugin.IsEnabled)
                {
                    ActivePlugins.Add(Plugin);
                }
            }
            return ActivePlugins;
        }
        public static List<JCAction> GetActionsFromPlugins(JCSteamAccount CurrectAccount)
        {
            List <JCAction> PluginsActions = new List<JCAction>();
            foreach (var Plugin in GetActivePlugins())
            {
                Type[] types = Plugin.assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsClass && type.BaseType.Name == "JCAccountActionBase")
                    {
                        object instance = Activator.CreateInstance(type);

                        type.GetMethod("LoadActions").Invoke(instance, new object[] { CurrectAccount } );
                        var method = type.GetMethod("GetActions");
                        List<JCAction> result = (List<JCAction>)method.Invoke(instance, null);
                       
                        foreach (var Action in result)
                        {
                            PluginsActions.Add(Action);
                        }
                    }
                }
            }
            return PluginsActions;
        }
        public static JArray LoadAllStatus()
        {
            JArray result = Utils.GetJsonArrayFromUrl("https://pastebin.com/raw/BqvJiVHj");
            if(result == null)
            {
                UI_Menager.ShowDialog("No Internet All plugins was marked as not Verified.");
                return new JArray();
            }
            return result;
        }
        public static string GetPluginStatus(string PluginHash)
        {
            foreach (JToken element in PluginStatusList)
            {
                if (element["Hash"].ToString() == PluginHash)
                {
                    return element["Status"].ToString();
                }
            }
            return "Not Verified";
        }
   
        public static List<JCEventInstance> GetEventByPlugin(JCPlugin plugin)
        {
            List<JCEventInstance> tasks = new List<JCEventInstance>();

            Type[] types = GetPluginByName(plugin.Name).assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsClass && type.BaseType.Name == "JCEventBase")
                {
                    object instance = Activator.CreateInstance(type, new object[] { new List<JCEventProperty>() });
                    FieldInfo field = type.GetField("Name");
                    string taskName = (string)field.GetValue(instance);
                    JCEventInstance newEvent = new JCEventInstance();
                    newEvent.Name = (string)type.GetField("Name").GetValue(instance);
                    newEvent.Description = (string)type.GetField("Description").GetValue(instance);
                    JCTask newTask = new JCTask();
                    newTask.TaskName = taskName;
                    newTask.PluginName = plugin.Name;
                    newEvent.TaskInfo = newTask;
                    tasks.Add(newEvent);
                }
            }
            return tasks;
        }
    }
}
