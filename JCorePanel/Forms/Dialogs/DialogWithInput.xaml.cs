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
    /// Логика взаимодействия для DialogWithInput.xaml
    /// </summary>
    public partial class DialogWithInput : UserControl
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
            if(InputBox.Foreground == Brushes.Gray)
            {
                OnCloseDialog(null);
            }
            else
            {
                OnCloseDialog(InputBox.Text);
            }
        }
    }
}
