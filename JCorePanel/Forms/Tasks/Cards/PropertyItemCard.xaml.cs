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
    public partial class PropertyItemCard : UserControl
    {
        public JCTask CurrectTask;
        public JCTaskItem CurrectTaskItem;
        public JCEventProperty CurrectProperty;
        Rectangle rectangle = new Rectangle();
        public PropertyItemCard(JCTaskItem taskItem, JCTask task, JCEventProperty property)
        {
            InitializeComponent();
            CurrectTask = task;
            CurrectProperty = property;
            CurrectTaskItem = taskItem;
            TaskData.Content = property.Name;

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void image_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new EditTaskPropertyWindow(CurrectTaskItem, CurrectTask, CurrectProperty));
        }
    }
}
