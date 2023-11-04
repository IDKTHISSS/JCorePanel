using JCorePanel.Classes.Managers;
using JCorePanelBase;
using JCorePanelBase.Structures;
using System;
using System.Linq;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для EditTaskPropertyWindow.xaml
    /// </summary>
    public partial class EditTaskPropertyWindow : BasePopupWindow
    {
        JCTaskItem CurrectTask;
        JCEventProperty CurrectProperty;
        JCTask CurrectEvent;
        public EditTaskPropertyWindow(JCTaskItem task, JCTask eventName, JCEventProperty property)
        {
            InitializeComponent();
            CurrectTask = task;
            CurrectProperty = property;
            CurrectEvent = eventName;
            PropertyNameBox.SetText(property.Name);
            PropertyValueBox.SetText(property.Value);
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCTaskItem newTask = CurrectTask;
            foreach (var Event in newTask.EventList.ToList())
            {
                if (Event.Name == CurrectEvent.Name)
                {
                    Event.PropertiesList.RemoveAll(item => item.Name == CurrectProperty.Name);
                    Event.PropertiesList.Add(new JCEventProperty(CurrectProperty.Name, PropertyValueBox.TextInput));
                }
            }
            TaskManager.EditTask(CurrectTask, newTask);
            OnWindowClose();
        }
    }
}
