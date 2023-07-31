using JCorePanelBase;
using JCorePanelBase.Structures;
using SteamKit2.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    public class TaskInstance : JCorePanelBase.JCEventInstance
    {
        public JCTaskItem TaskItem;
        public TaskCard TaskCard;
        private Thread TaskThread;
        public List<JCSteamAccountInstance> AccountList = new List<JCSteamAccountInstance>();
        

        public TaskInstance(JCTaskItem taskItem)
        {
            TaskItem = taskItem;
            AccountList = Utils.GetAccountsFromLogins(TaskItem.AccountNames);
            IsInWorkChangedHandler += OnIsInWork;
        }


        private void OnError(bool newError)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCard.HoverRectangle.Fill = newError ? (Brush)new BrushConverter().ConvertFrom("#FF0000") : (Brush)new BrushConverter().ConvertFrom("#3CFF8D");
            });
        }
        private void OnIsInWork(bool newInWork) {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCard.InfoButton.Visibility = !newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.AccountsButton.Visibility = !newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.TasksButton.Visibility = !newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.SettingsButton.Visibility = !newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.DeleteButton.Visibility = !newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.WorkStatus.Visibility = newInWork ? Visibility.Visible : Visibility.Collapsed;
                TaskCard.StartButton.Source = new BitmapImage(new Uri("/Images/Icons/"+ (newInWork ? "Stop" : "Play") +".png", UriKind.Relative));
            });
            

        }
        private void OnStatus(string newStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCard.WorkStatus.Text = newStatus.Length > 14 ? newStatus.Substring(0, 14) + "..." : newStatus;
            });
        }
        public void StartTask()
        {
            TaskThread = new Thread(() =>
            {
                SetIsInWork(true);
            
                Logger.Log("Start task: " + TaskItem.TaskName);
                AccountList = Utils.GetAccountsFromLogins(TaskItem.AccountNames);
                foreach (var Plugin in PluginsManager.GetActivePlugins())
                {
                    foreach (var Task in TaskItem.EventList)
                    {
                        if (Task.PluginName != Plugin.Name) continue;

                        Type[] types = Plugin.assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (!type.IsClass || type.BaseType.Name != "JCEventBase") continue;
                            
                            object instance = Activator.CreateInstance(type, new object[] { Task.PropertiesList });
                            FieldInfo field = type.GetField("Name");
                            string taskName = (string)field.GetValue(instance);
                            List <JCEventProperty> properties = (List<JCEventProperty>)type.GetField("Properties").GetValue(instance);
                            if (taskName != Task.TaskName) continue;

                            Logger.Log($"Starting [{Plugin.Name}] {taskName}");
                            var math = type.GetMethod("EventBody");
                            EventInfo eventInfo = type.GetEvent("ErrorChangedHandler", BindingFlags.Instance | BindingFlags.Public);
                            TaskErrorChangedEventHandler eventHandler = new TaskErrorChangedEventHandler(OnError);
                            eventInfo.AddEventHandler(instance, eventHandler);
                            EventInfo eventInfo2 = type.GetEvent("WorkStatusChangedHandler", BindingFlags.Instance | BindingFlags.Public);
                            TaskWorkStatusChangedEventHandler eventHandler2 = new TaskWorkStatusChangedEventHandler(OnStatus);
                            eventInfo2.AddEventHandler(instance, eventHandler2);
                            if (math == null)
                            {
                                Logger.Log($"[{Plugin.Name}] {taskName} not found task body.");
                            }
                            SetPlaceholder("#"+taskName);
                            math.Invoke(instance, new object[] { AccountList });
                            
                        }
                    }
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    StopTask();
                });

            });
            TaskThread.Start();
        }

        public void SetPlaceholder(string placeholder)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCard.PlaceholderLabel.Content = placeholder;

            });
        }
        public void UpdateName(string newName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCard.TitleLabel.Content = newName;

            });
           
        }
        public void StopTask()
        {
            if(TaskThread!=null) TaskThread.Suspend();
            SetIsInWork(false);
            
            SetPlaceholder("#" + "IDLE");
        }
        public void ShowInfo()
        {
            Utils.ShowPopupWindow(new TaskInfoWindow(TaskItem));
        }
        public void ShowSettings()
        {
            Utils.ShowPopupWindow(new TaskSettingsWindow(TaskItem));
        }
        public void ShowAccountList()
        {
            Utils.ShowPopupWindow(new AccountListTaskWindow(TaskItem));
        }
    }
}
