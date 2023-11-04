using JCorePanel.Classes.Managers;
using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для AddEventToTaskWindow.xaml
    /// </summary>
    public partial class AddEventToTaskWindow : BasePopupWindow
    {
        private List<PluginTaskCard> pluginTaskCards = new List<PluginTaskCard>();
        private JCTaskItem TaskItem;
        public Action<JCTask> OnSelectEvent;
        public AddEventToTaskWindow(JCTaskItem taskItem)
        {
            InitializeComponent();
            TaskItem = taskItem;
            foreach (var plugin in PluginsManager.GetActivePlugins())
            {
                if (PluginsManager.GetEventByPlugin(plugin).Count() > 0)
                {
                    PluginTaskCard pluginCard = new PluginTaskCard(plugin);
                    pluginCard.Select += () =>
                    {
                        foreach (var plg in pluginTaskCards)
                        {
                            plg.UnSelectEvent();
                        }
                        pluginCard.SelectEvent();
                        SelectPlugin(plugin);
                    };
                    pluginTaskCards.Add(pluginCard);
                    PluginsListGrid.Children.Add(pluginTaskCards.Last());
                }
            }

        }
        private void SelectPlugin(JCPlugin plugin)
        {
            EventListGrid.Children.Clear();
            foreach (var eve in PluginsManager.GetEventByPlugin(plugin))
            {
                EventTaskCard eventTaskCard = new EventTaskCard(eve);
                eventTaskCard.AddEvent += () =>
                {
                    JCTaskItem newTask = TaskItem;
                    JCTask newEvent = eve.TaskInfo;
                    newEvent.Name = eve.Name + "_" + Utils.GenerateRandomString();
                    newEvent.PropertiesList = new List<JCorePanelBase.Structures.JCEventProperty>();
                    newTask.EventList.Add(newEvent);
                    TaskManager.EditTask(TaskItem, newTask);
                    OnSelectEvent(newEvent);
                    OnWindowClose();
                };
                EventListGrid.Children.Add(eventTaskCard);
            }
        }
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }
    }
}
