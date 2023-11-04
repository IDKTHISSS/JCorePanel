using JCorePanel.Classes.Managers;
using JCorePanelBase;
using System;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskPropertyListWindow.xaml
    /// </summary>
    public partial class TaskPropertyListWindow : BasePopupWindow
    {
        public TaskPropertyListWindow(JCTaskItem taskItem, JCTask task)
        {
            InitializeComponent();

            foreach (var proprerty in TaskManager.GetProperies(task))
            {
                PropreryListGrid.Children.Add(new PropertyItemCard(taskItem, task, proprerty));
            }
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            OnWindowClose();
        }
    }
}
