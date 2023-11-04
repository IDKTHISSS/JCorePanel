using System;
using System.Windows;
using System.Windows.Controls;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для Button.xaml
    /// </summary>
    /// 
    public partial class Button : UserControl
    {
        public event EventHandler ButtonClick;
        public static readonly DependencyProperty ButtonTextProperty =
             DependencyProperty.Register("ButtonText", typeof(string), typeof(Button));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public Button()
        {

            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
