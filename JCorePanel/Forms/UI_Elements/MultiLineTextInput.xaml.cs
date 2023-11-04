using System.Windows;
using System.Windows.Controls;

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
