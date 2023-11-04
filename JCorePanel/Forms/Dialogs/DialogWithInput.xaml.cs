using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для DialogWithInput.xaml
    /// </summary>
    public partial class DialogWithInput : BasePopupWindow
    {
        public string Messege;
        public Action<string> OnCloseDialog;
        public string Placeholder;
        public DialogWithInput(string msg, string placeholder)
        {
            InitializeComponent();
            Messege = msg;
            Placeholder = placeholder;
            msgBox.Content = Messege;
            InputBox.Foreground = Brushes.Gray;
            InputBox.Text = Placeholder;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                instance.Foreground = Brushes.Gray;
                instance.Text = Placeholder;
            }

        }
        public string Value { get { if (msgBox.Foreground == Brushes.Gray) return null; else return InputBox.Text; } }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == Placeholder)
            {
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            if (InputBox.Foreground == Brushes.Gray)
            {
                OnWindowClose();
                OnCloseDialog(null);
            }
            else
            {
                OnWindowClose();
                OnCloseDialog(InputBox.Text);
            }
        }
    }
}
