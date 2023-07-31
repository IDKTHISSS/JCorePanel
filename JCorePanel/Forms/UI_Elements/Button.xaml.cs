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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
