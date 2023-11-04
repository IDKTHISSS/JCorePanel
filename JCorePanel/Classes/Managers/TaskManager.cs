using JCorePanelBase;
using JCorePanelBase.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JCorePanel.Classes.Managers
{
    public static class TaskManager
    {
        public static List<TaskInstance> TaskList = new List<TaskInstance>();
        public static void LoadTasks()
        {
            TaskList.Clear();
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                return;
            }
            string[] taskFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*.jcTask");
            foreach (string taskFile in taskFiles)
            {
                TaskInstance NewTask = new TaskInstance(JsonConvert.DeserializeObject<JCTaskItem>(File.ReadAllText(taskFile)));
                NewTask.TaskCard = new TaskCard(NewTask);
                TaskList.Add(NewTask);
            }

        }

        public static void DeleteTask(JCTaskItem Task)
        {
            string[] taskFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*.jcTask");
            foreach (string taskFile in taskFiles)
            {
                JCTaskItem TaskFile = JsonConvert.DeserializeObject<JCTaskItem>(File.ReadAllText(taskFile));
                if (TaskFile.TaskName == Task.TaskName)
                {
                    File.Delete(taskFile);
                }
            }
            foreach (var item in TaskList.ToList())
            {
                if (item.TaskItem.TaskName == Task.TaskName)
                {
                    TaskList.Remove(item);
                }
            }
        }
        public static void DeleteTask(JCTask Task)
        {
            string[] taskFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*.jcTask");
            foreach (string taskFile in taskFiles)
            {
                JCTaskItem TaskFile = JsonConvert.DeserializeObject<JCTaskItem>(File.ReadAllText(taskFile));
                if (TaskFile.TaskName == Task.TaskName)
                {
                    File.Delete(taskFile);
                }
            }
            foreach (var item in TaskList)
            {
                if (item.TaskItem.TaskName == Task.TaskName)
                {
                    TaskList.Remove(item);
                }
            }
        }
        public static void EditTask(JCTaskItem OldTask, JCTaskItem NewTask)
        {
            string[] taskFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*.jcTask");
            foreach (string taskFile in taskFiles)
            {
                JCTaskItem TaskFile = JsonConvert.DeserializeObject<JCTaskItem>(File.ReadAllText(taskFile));
                if (TaskFile.TaskName == OldTask.TaskName)
                {
                    File.Delete(taskFile);
                    File.WriteAllText(taskFile, JsonConvert.SerializeObject(NewTask).ToString());

                }
            }
            foreach (var item in TaskList)
            {
                if (item.TaskItem.TaskName == OldTask.TaskName)
                {
                    item.TaskItem.TaskName = NewTask.TaskName;
                    item.TaskItem.TaskDescription = NewTask.TaskDescription;
                    item.TaskItem.AccountNames = NewTask.AccountNames;
                    item.TaskItem.EventList = NewTask.EventList;
                    item.UpdateName(NewTask.TaskName);
                }
            }

        }
        public static List<JCEventProperty> GetProperies(JCTask task)
        {
            List<JCEventProperty> properties = new List<JCEventProperty>();
            try
            {
                Type[] types = PluginsManager.GetPluginByName(task.PluginName).assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsClass && type.BaseType.Name == "JCEventBase")
                    {
                        object instance = Activator.CreateInstance(type, new object[] { task.PropertiesList });
                        FieldInfo field = type.GetField("Name");
                        string taskName = (string)field.GetValue(instance);

                        if (taskName == task.TaskName)
                        {
                            properties = (List<JCEventProperty>)type.GetField("Properties").GetValue(instance);
                        }
                    }
                }
                return properties;
            }
            catch
            {
                return new List<JCEventProperty>();
            }

        }
        public static void CreateTask(JCTaskItem NewTask)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"));
            }

            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks") + "/" + NewTask.TaskName + ".jcTask", JsonConvert.SerializeObject(NewTask).ToString());


        }
    }
}
