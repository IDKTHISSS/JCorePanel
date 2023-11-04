using JCorePanel.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskSettingsWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : BasePopupWindow
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCTaskItem NewTask = new JCTaskItem();
            NewTask.TaskName = TaskNameBox.Text;
            NewTask.TaskDescription = TaskDescBox.TextInput;
            NewTask.AccountNames = new List<string>();
            NewTask.EventList = new List<JCorePanelBase.JCTask>();
            TaskManager.CreateTask(NewTask);
            TaskInstance taskInstance = new TaskInstance(NewTask);
            TaskManager.TaskList.Add(taskInstance);
            taskInstance.TaskCard = new TaskCard(taskInstance);
            (Application.Current.MainWindow as MainWindow).TasksListGrid.Children.Add(taskInstance.TaskCard);
            OnWindowClose();
        }
    }
}
