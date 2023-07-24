using JCorePanelBase;
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
            // Выводим имена DLL-файлов
            foreach (string dllFile in dllFiles)
            {
                string PluginStatus = GetPluginStatus(Utils.CalculateSHA512Hash(dllFile));
                if (PluginStatus == "Virus") continue;
                Assembly assembly = Assembly.LoadFrom(dllFile);

                // Получаем все типы из сборки
                Type[] types = assembly.GetTypes();

                // Перебираем все типы
                foreach (Type type in types)
                {
                    // Проверяем, является ли тип статическим классом и имеет имя JCPluginConfig
                    if (type.IsClass && type.IsSealed && type.IsAbstract && type.Name == "JCPluginConfig")
                    {
                        JCPlugin PluginInfo = new JCPlugin();
                        // Получаем поле (переменную) по имени
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
                        field = type.GetField("PLUGIN_AUTHOR", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginInfo.Author = field.GetValue(null) as string;
                        }
                        PluginInfo.Hash = Utils.CalculateSHA512Hash(dllFile);
                        PluginInfo.Status = PluginStatus;
                        PluginInfo.assembly = assembly;
                        try
                        {
                            string filePath = "EnabledPlugins.json";
                            string jsonString = File.ReadAllText(filePath);

                            string[] enabledPlugins = JsonConvert.DeserializeObject<string[]>(jsonString);

                            foreach (string plugin in enabledPlugins)
                            {
                                if(plugin == PluginInfo.Name)
                                {
                                    PluginInfo.IsEnabled = true;
                                }
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("Файл EnabledPlugins.json не найден.");
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

                    if (!File.Exists("EnabledPlugins.json"))
                    {
                        // Если файл не существует, создаем его и добавляем пустой массив
                        JArray pluginArray2 = new JArray();
                        File.WriteAllText("EnabledPlugins.json", pluginArray2.ToString());
                    }
                    // Если файл существует, читаем его содержимое и проверяем наличие строки
                    string fileContent = File.ReadAllText("EnabledPlugins.json");
                    JArray pluginArray = JArray.Parse(fileContent);

                    bool hasString = false;
                    foreach (string pluginName in pluginArray)
                    {
                        if (pluginName == tempPlugin.Name)
                        {
                            hasString = true;
                            break;
                        }
                    }

                    if (!hasString)
                    {
                        // Если нет строки, добавляем ее в массив и сохраняем обновленное содержимое в файл
                        pluginArray.Add(tempPlugin.Name);
                        File.WriteAllText("EnabledPlugins.json", pluginArray.ToString());
                    }

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
                    if (!File.Exists("EnabledPlugins.json"))
                    {
                        // Если файл не существует, создаем его и добавляем пустой массив
                        JArray pluginArray2 = new JArray();
                        File.WriteAllText("EnabledPlugins.json", pluginArray2.ToString());
                    }
                    // Если файл существует, читаем его содержимое и проверяем наличие строки
                    string fileContent = File.ReadAllText("EnabledPlugins.json");
                    JArray pluginArray = JArray.Parse(fileContent);
                    for (int j = 0; j < pluginArray.Count; j++)
                    {
                        string pluginName = pluginArray[j].ToString();
                        if (pluginName == tempPlugin.Name)
                        {
                            pluginArray.RemoveAt(j);
                            File.WriteAllText("EnabledPlugins.json", pluginArray.ToString());
                            break;
                        }
                    }

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
                // Получаем все типы из сборки
                Type[] types = Plugin.assembly.GetTypes();

                // Перебираем все типы
                foreach (Type type in types)
                {
                    // Проверяем, является ли тип статическим классом и имеет имя JCPluginConfig
                    if (type.IsClass && type.BaseType.Name == "JCAccountActionBase")
                    {
                        object instance = Activator.CreateInstance(type);

                        type.GetMethod("LoadActions").Invoke(instance, null);
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
    }
}
