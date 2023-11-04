using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для InputBox.xaml
    /// </summary>
    public partial class InputBox : UserControl
    {

        public static readonly DependencyProperty InputTextProperty =
             DependencyProperty.Register("InputText", typeof(string), typeof(Button));
        public static readonly DependencyProperty TextProperty =
             DependencyProperty.Register("Text", typeof(string), typeof(Button));


        public event Action<string> TextChanged;

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public InputBox()
        {

            InitializeComponent();

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                instance.Foreground = Brushes.Gray;
                instance.Text = InputText;
            }
        }
        public void SetText(string text)
        {
            InputTextBox.Text = text;
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == InputText)
            {
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }
        }

        private void InputTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputTextBox.Text = InputText;
            InputTextBox.Foreground = Brushes.Gray;
            Keyboard.ClearFocus();
            RoutedEventArgs eventArgs = new RoutedEventArgs(UIElement.LostFocusEvent, InputTextBox);
            InputTextBox.RaiseEvent(eventArgs);
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null) TextChanged(InputTextBox.Text);
            Text = InputTextBox.Text;
        }

        private void InputTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == InputText)
            {
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }
        }
    }
}
