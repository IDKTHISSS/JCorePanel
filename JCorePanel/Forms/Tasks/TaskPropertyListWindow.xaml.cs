using JCorePanel.Classes.Managers;
using JCorePanelBase;
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
    /// Логика взаимодействия для TaskPropertyListWindow.xaml
    /// </summary>
    public partial class TaskPropertyListWindow : BasePopupWindow
    {
        public TaskPropertyListWindow(JCTaskItem taskItem, JCTask task)
        {
            InitializeComponent();

            foreach(var proprerty in TaskManager.GetProperies(task))
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
