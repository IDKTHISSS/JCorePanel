using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для EventTaskCard.xaml
    /// </summary>
    public partial class EventTaskCard : UserControl
    {
        Rectangle HoveredRectangle;
        Image AddEventIcon;
        public Action AddEvent;
        public EventTaskCard(JCEventInstance taskItem)
        {
            InitializeComponent();
            EventData.Content = taskItem.Name;

            HoveredRectangle = new Rectangle();
            HoveredRectangle.Name = "HoverRectangle";
            HoveredRectangle.SetValue(Grid.ColumnSpanProperty, 2);
            HoveredRectangle.RadiusX = 9;
            HoveredRectangle.RadiusY = 9;
            HoveredRectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            HoveredRectangle.Opacity = 0;
            HoveredRectangle.Height = 34;
            HoveredRectangle.Margin = new Thickness(-2, 0, 0, 0);
            HoveredRectangle.VerticalAlignment = VerticalAlignment.Center;
            HoveredRectangle.Width = 150;
            HoveredRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            MainGrid.Children.Add(HoveredRectangle);

            AddEventIcon = new Image();
            AddEventIcon.Source = new BitmapImage(new Uri("/Images/Icons/Plus.png", UriKind.Relative));
            AddEventIcon.Width = 14;
            AddEventIcon.Height = 14;
            AddEventIcon.HorizontalAlignment = HorizontalAlignment.Center;
            AddEventIcon.VerticalAlignment = VerticalAlignment.Center;
            AddEventIcon.Cursor = Cursors.Hand;
            AddEventIcon.Opacity = 0;
            AddEventIcon.MouseDown += (sender, e) =>
            {
                AddEvent();
            };
            Grid.SetColumn(AddEventIcon, 0);
            MainGrid.Children.Add(AddEventIcon);

            MainGrid.MouseEnter += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
                HoveredRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation AddEventIconAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                AddEventIcon.BeginAnimation(UIElement.OpacityProperty, AddEventIconAnimation);

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 5;
                EventData.Effect = titleLabelEffect;
            };

            MainGrid.MouseLeave += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                HoveredRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation AddEventIconAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                AddEventIcon.BeginAnimation(UIElement.OpacityProperty, AddEventIconAnimation);

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 0;
                EventData.Effect = titleLabelEffect;
            };

        }
    }
}
