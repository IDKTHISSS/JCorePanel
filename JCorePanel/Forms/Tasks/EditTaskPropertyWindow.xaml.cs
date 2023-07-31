using JCorePanel.Classes.Managers;
using JCorePanelBase;
using JCorePanelBase.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            foreach(var Event in newTask.EventList.ToList())
            {
                if(Event.Name == CurrectEvent.Name)
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
