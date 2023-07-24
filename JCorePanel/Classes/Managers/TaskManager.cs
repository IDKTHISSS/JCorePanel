using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading;

namespace JCorePanel.Classes.Managers
{
    public static class TaskManager
    {
        public static List<JCTaskItem> TaskList = new List<JCTaskItem>();
        public static List<JCTaskItem> GetAllTasks()
        {
            TaskList.Clear();
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                return new List<JCTaskItem>();
            }
            string[] taskFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*.jcTask");
            foreach (string taskFile in taskFiles)
            {
                TaskList.Add(JsonConvert.DeserializeObject<JCTaskItem>(File.ReadAllText(taskFile)));
            }
            return TaskList;
        }
        public static void StartTaskByTaskItem(JCTaskItem TaskItem)
        {
            Logger.Log("Start task: " + TaskItem.TaskName);
            foreach (var Plugin in PluginsManager.GetActivePlugins())
            {
                foreach (var Task in TaskItem.EventList)
                {
                    if (Task.PluginName == Plugin.Name)
                    {

                        Type[] types = Plugin.assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsClass && type.BaseType.Name == "JCTaskBase")
                            {
                                object instance = Activator.CreateInstance(type);
                                FieldInfo field = type.GetField("TaskName");
                                string taskName = (string)field.GetValue(instance);
                                if (taskName == Task.TaskName) {
                                    Logger.Log($"Starting [{Plugin.Name}] {taskName}");
                                    var math = type.GetMethod("Task");
                                    if(math == null)
                                    {
                                        Logger.Log($"[{Plugin.Name}] {taskName} not found task body.");
                                    }
                                    math.Invoke(instance, new object[] { Utils.GetAccountsFromLogins(TaskItem.AccountNames) });
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
