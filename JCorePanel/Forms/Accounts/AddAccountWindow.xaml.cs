using JCorePanelBase;
using Microsoft.Win32;
using Newtonsoft.Json;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddAccountWindow.xaml
    /// </summary>
    public partial class AddAccountWindow : BasePopupWindow
    {
        private bool PasswordIsFocused = false;
        private string Password = "";
        SteamGuardAccount SteamGuardAccount = null;
        public AddAccountWindow()
        {
            InitializeComponent();

            PasswordIsFocused = false;
            PasswordTextBox.Foreground = Brushes.Gray;
            PasswordTextBox.Text = "Pasword";
            LoginTextBox.Foreground = Brushes.Gray;
            LoginTextBox.Text = "Login";
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy; // Указываем, что разрешено копирование
            }
            else
            {
                e.Effects = DragDropEffects.None; // Запрещаем операцию, если данные не являются файлами
            }
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0];

                // Получаем расширение файла из пути
                string fileExtension = filePath.Substring(filePath.LastIndexOf('.'));

                // Проверяем формат файла
                if (fileExtension == ".maFile")
                {
                    SteamGuardAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(File.ReadAllText(filePath));
                    MaFileTextBox.Text = SteamGuardAccount.Session.SteamID.ToString();
                    LoginTextBox.Text = SteamGuardAccount.AccountName;
                    LoginTextBox.IsEnabled = false;
                    LoginTextBox.Foreground = Brushes.White;
                    LoginBox.Cursor = Cursors.No;
                }
                else
                {
                    // Файл имеет неподдерживаемый формат
                    Console.WriteLine("Неподдерживаемый формат файла!");
                }
            }
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == "Login")
            {
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                instance.Foreground = Brushes.Gray;
                instance.Text = "Login";
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == "Pasword")
            {
                PasswordIsFocused = true;
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                PasswordIsFocused = false;
                instance.Foreground = Brushes.Gray;
                instance.Text = "Pasword";
            }
        }

        private void MaFileButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "SDA file (*.maFile)|*.maFile;";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                SteamGuardAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(File.ReadAllText(openFileDialog1.FileName));
                MaFileTextBox.Text = SteamGuardAccount.Session.SteamID.ToString();
                LoginTextBox.Text = SteamGuardAccount.AccountName;
                LoginTextBox.IsEnabled = false;
                LoginTextBox.Foreground = Brushes.White;
                LoginBox.Cursor = Cursors.No;
                //openFileDialog1.FileName;
            }
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordIsFocused)
            {
                TextBox instance = (TextBox)sender;
                if(Password.Length > instance.Text.Length)
                {
                    Password = Password.Remove(Password.Length - 1 - (Password.Length - instance.Text.Length));
                    instance.Text = new string(instance.Text.Select(c => '*').ToArray());
                    instance.CaretIndex = instance.Text.Length;
                    return;
                }
                Password = Password + GetNewChar(instance.Text);
                instance.Text = new string(instance.Text.Select(c => '*').ToArray());
                instance.CaretIndex = instance.Text.Length;
            }
        }
        private string GetNewChar(string text)
        {
            foreach (char ch in text) {
                if (ch == '*') continue;
                return ch.ToString();
            }
            return "";
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCSteamAccount newSteamAccount = new JCSteamAccount();
            newSteamAccount.Login = LoginTextBox.Text.ToLower();
            newSteamAccount.Password = PasswordTextBox.Text;
            if(SteamGuardAccount != null)
            {
                newSteamAccount.MaFile = SteamGuardAccount;
            }
            AccountMenager.AddAcount(newSteamAccount);
        }

        private void Border_Drop_1(object sender, DragEventArgs e)
        {

        }
    }
}
