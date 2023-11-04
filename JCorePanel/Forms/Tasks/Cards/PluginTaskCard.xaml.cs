using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для PluginTaskCard.xaml
    /// </summary>
    public partial class PluginTaskCard : UserControl
    {
        public JCPlugin CurrectPlugin;
        public Action Select;
        private Rectangle SelectRectangle;
        public PluginTaskCard(JCPlugin plugin)
        {
            InitializeComponent();
            PluginData.Content = plugin.FrendlyName;

            SelectRectangle = new Rectangle();
            SelectRectangle.Name = "HoverRectangle";
            SelectRectangle.SetValue(Grid.ColumnSpanProperty, 2);
            SelectRectangle.RadiusX = 9;
            SelectRectangle.RadiusY = 9;
            SelectRectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            SelectRectangle.Opacity = 0.5;
            SelectRectangle.Visibility = Visibility.Collapsed;
            SelectRectangle.Height = 34;
            SelectRectangle.Margin = new Thickness(-2, 0, 0, 0);
            SelectRectangle.VerticalAlignment = VerticalAlignment.Center;
            SelectRectangle.Width = 150;
            SelectRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            MainGrid.Children.Add(SelectRectangle);
        }

        public void SelectEvent()
        {
            SelectRectangle.Visibility = Visibility.Visible;
        }
        public void UnSelectEvent()
        {
            SelectRectangle.Visibility = Visibility.Collapsed;
        }
        private void LoginBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectEvent();
            Select();
        }
    }
}
