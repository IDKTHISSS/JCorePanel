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
    /// Логика взаимодействия для MultiLineTextInput.xaml
    /// </summary>
    public partial class MultiLineTextInput : UserControl
    {
        public static readonly DependencyProperty TextProperty =
             DependencyProperty.Register("TextInput", typeof(string), typeof(Button));

        public string TextInput
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public MultiLineTextInput()
        {
            InitializeComponent();
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextInput = InputTextBox.Text;
        }
        public void SetText(string text)
        {
            InputTextBox.Text = text;
        }
    }
}
