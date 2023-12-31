﻿using JCorePanel.Classes.Managers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskCard.xaml
    /// </summary>
    public partial class TaskCard : UserControl
    {
        TaskInstance TaskItem;
        public TaskCard(TaskInstance taskItem)
        {
            InitializeComponent();
            TaskItem = taskItem;
            TaskItem.TaskCard = this;
            TitleLabel.Content = TaskItem.TaskItem.TaskName;

        }

        private void DeleteButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UI_Menager.ShowDialogConfirm("Are you sure you want to delete this task?", (res) =>
            {
                if (!res) return;
                (Parent as UniformGrid).Children.Remove(this);
                TaskManager.DeleteTask(TaskItem.TaskItem);
            });
        }

        private void InfoButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TaskItem.ShowInfo();
        }

        private void SettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TaskItem.ShowSettings();
        }

        private void TasksButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new TaskEventListWindow(TaskItem.TaskItem));
        }

        private void AccountsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TaskItem.ShowAccountList();
        }

        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TaskItem.IsInWork)
            {
                TaskItem.StopTask();
            }
            else
            {
                TaskItem.StartTask();
            }

        }
    }
}
