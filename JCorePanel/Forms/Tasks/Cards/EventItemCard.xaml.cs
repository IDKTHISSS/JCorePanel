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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SteamKit2.GC.Artifact.Internal.CMsgGauntletConfig.EntryType;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для EventItemCard.xaml
    /// </summary>
    public partial class EventItemCard : UserControl
    {
        public JCTask CurrectTask;
        public JCTaskItem TaskItem;
        Rectangle rectangle = new Rectangle();
        public EventItemCard(JCTask task, JCTaskItem Item)
        {
            InitializeComponent();
            CurrectTask = task;
            TaskItem = Item;
            TaskData.Content = new TextBlock
            {
                Inlines =
                {
                    new Run(task.TaskName),
                    new Run("    #"+task.PluginName) {  Foreground =(System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#8A41FB") }
                }
            };
            if(TaskManager.GetProperies(task).Count == 0)
            {
                image_Copy.Visibility = Visibility.Collapsed;
            }
            rectangle.Name = "HoverRectangle";
            rectangle.SetValue(Grid.ColumnSpanProperty, 2);
            rectangle.RadiusX = 9;
            rectangle.RadiusY = 9;
            rectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            rectangle.Opacity = 0.5;
            rectangle.Visibility = Visibility.Collapsed;
            rectangle.Height = 34;
            rectangle.Margin = new Thickness(-2, 0, 0, 0);
            rectangle.VerticalAlignment = VerticalAlignment.Center;
            rectangle.Width = 456;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            MainGrid.Children.Add(rectangle);
        }
        public void SetHover(bool isHover)
        {
            rectangle.Visibility = isHover ? Visibility.Visible : Visibility.Collapsed;
        }
        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            JCTaskItem newTask = TaskItem;
            newTask.EventList.RemoveAll(item => item.Name == CurrectTask.Name);
            TaskManager.EditTask(TaskItem, newTask);
            (Parent as UniformGrid).Children.Remove(this);
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void image_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new TaskPropertyListWindow(TaskItem, CurrectTask));
        }
    }
}
