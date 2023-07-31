using JCorePanelBase;
using Newtonsoft.Json;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SteamKit2.GC.Dota.Internal.CMsgDOTALeague;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для AccountInfoWindow.xaml
    /// </summary>
    public partial class AccountInfoWindow : BasePopupWindow
    {
        private AccountInstance Info;
        private IDisposable GuardTimer;
        private SteamGuardAccount NewMaFile;
        public int Time = 30;
        public AccountInfoWindow(AccountInstance Account)
        {
            InitializeComponent();
            Info = Account;
            label1_Copy.Content = Account.AccountInfo.Login;
            PasswordTextBox.Text = Account.AccountInfo.Password;
            progressBar.Value = 24;

           
            long steamTime = TimeAligner.GetSteamTime();
            long currentSteamChunk = steamTime / 30L;
            Time = (int)(steamTime - (currentSteamChunk * 30L));
            if(Info.AccountInfo.MaFile == null)
            {
                label1.Content = "-----";
                progressBar.Value = 100;
                return;
            }
            label1.Content = Account.AccountInfo.MaFile.GenerateSteamGuardCode();
            progressBar.Value = 30 - Time;
            MaFileTextBox.Text = Info.AccountInfo.MaFile.Session.SteamID.ToString();
            GuardTimer = Observable.Interval(TimeSpan.FromSeconds(0.03))
           .Subscribe(_ =>
           {
               var time = DateTime.UtcNow;
               var date = time.Date;

               var delta = (time - date).TotalMilliseconds / 1000d % 30d;

               var value = 100d - delta / 30d * 100d;
               Dispatcher.Invoke(() =>
               {
                   
                   if(value < 0.1d)
                   {
                       label1.Content = Account.AccountInfo.MaFile.GenerateSteamGuardCodeForTime((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds+1);
                   }
                   progressBar.Value = value;
               });
           });
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(GuardTimer!=null) GuardTimer.Dispose();
            OnWindowClose();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginBox.Background = (Brush)new BrushConverter().ConvertFrom("#303436");
            label1_Copy.Content = "Copied";
            JCorePanelBase.Utils.SetTextToClipboard(Info.AccountInfo.Login);
            Task.Delay(1500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    LoginBox.Background = (Brush)new BrushConverter().ConvertFrom("#242728");
                    label1_Copy.Content = Info.AccountInfo.Login;
                    
                });
                
            });
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            PasswordBox.Background = (Brush)new BrushConverter().ConvertFrom("#303436");
            HiddenPassword.Content = "Copied";
            JCorePanelBase.Utils.SetTextToClipboard(Info.AccountInfo.Password);
            Task.Delay(1500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    PasswordBox.Background = (Brush)new BrushConverter().ConvertFrom("#242728");
                    HiddenPassword.Content = "*********";
                });

            });
           
        }

        private void Border_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            if(Info.AccountInfo.MaFile == null) return;
            GuardBox.Background = (Brush)new BrushConverter().ConvertFrom("#303436");
            label1.Content = "Copied";
            JCorePanelBase.Utils.SetTextToClipboard(Info.AccountInfo.MaFile.GenerateSteamGuardCodeForTime((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + 1));
            Task.Delay(1500).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    GuardBox.Background = (Brush)new BrushConverter().ConvertFrom("#242728");
                    label1.Content = Info.AccountInfo.MaFile.GenerateSteamGuardCodeForTime((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + 1);

                });

            });
        }

        private void PasswordBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            HiddenPassword.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordTextBox.Focus();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            if (PasswordTextBox.Visibility == Visibility.Visible)
            {
                Info.AccountInfo.Password = PasswordTextBox.Text;
                AccountMenager.SaveAccountData(Info.AccountInfo);
            }
            if (NewMaFile != null)
            {
                Info.AccountInfo.MaFile = NewMaFile;
                AccountMenager.SaveAccountData(Info.AccountInfo);
            }
        }

        private void Border_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "SDA file (*.maFile)|*.maFile;";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                NewMaFile = JsonConvert.DeserializeObject<SteamGuardAccount>(File.ReadAllText(openFileDialog1.FileName));
                if (Info.AccountInfo.Login.ToLower() != NewMaFile.AccountName.ToLower())
                {
                    
                    UI_Menager.ShowDialog("This mafile from the account " + NewMaFile.AccountName + " and cant be loaded.");
                    NewMaFile = null;
                    return;
                }
                MaFileTextBox.Text = NewMaFile.Session.SteamID.ToString();
            }
        }
    }
}
