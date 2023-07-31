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
    /// Логика взаимодействия для TaskInfoWindow.xaml
    /// </summary>
    public partial class TaskInfoWindow : BasePopupWindow
    {
        public TaskInfoWindow(JCTaskItem taskItem)
        {
            InitializeComponent();

            TaskNameBox.Content = taskItem.TaskName;
            TaskDescriptionBox.Text = taskItem.TaskDescription;
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }
    }
}
