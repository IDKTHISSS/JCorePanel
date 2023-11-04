using System.Windows.Input;

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
