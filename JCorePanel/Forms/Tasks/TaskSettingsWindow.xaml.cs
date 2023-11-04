using JCorePanel.Classes.Managers;
using System;
using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskSettingsWindow.xaml
    /// </summary>
    public partial class TaskSettingsWindow : BasePopupWindow
    {
        JCTaskItem CurrectTask;
        public TaskSettingsWindow(JCTaskItem taskItem)
        {
            InitializeComponent();
            CurrectTask = taskItem;
            TaskNameBox.SetText(CurrectTask.TaskName);
            TaskDescBox.SetText(CurrectTask.TaskDescription);
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCTaskItem NewTask = CurrectTask;
            NewTask.TaskName = TaskNameBox.Text == null ? CurrectTask.TaskName : TaskNameBox.Text;
            NewTask.TaskDescription = TaskDescBox.TextInput == null ? CurrectTask.TaskDescription : TaskDescBox.TextInput;
            TaskManager.EditTask(CurrectTask, NewTask);
            OnWindowClose();
        }
    }
}
