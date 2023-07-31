using JCorePanel.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
