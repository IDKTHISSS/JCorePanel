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
    /// Логика взаимодействия для AccountListTaskWindow.xaml
    /// </summary>
    public partial class AccountListTaskWindow : BasePopupWindow
    {
        public JCTaskItem TaskItem;
        public AccountListTaskWindow(JCTaskItem taskItem)
        {
            InitializeComponent();

            TaskItem = taskItem;

            foreach(var acc in Utils.GetAccountsFromLogins(TaskItem.AccountNames))
            {
                AccountListGrid.Children.Add(UI_Menager.GenerateAccountTaskCard(Utils.GetAccountBySteamAccount(acc), TaskItem));
            }

        }


        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            AddAccountToTaskWindow addAccountToTaskWindow = new AddAccountToTaskWindow(TaskItem);
            addAccountToTaskWindow.OnSelectedAccs = (SelectedAccs) => {
                foreach(var acc in SelectedAccs)
                {
                    AccountListGrid.Children.Add(UI_Menager.GenerateAccountTaskCard(Utils.GetAccountBySteamAccount(acc), TaskItem));
                }
            };
            Utils.ShowPopupWindow(addAccountToTaskWindow);
        }

        private void SearchAccountBox_TextChanged(string TextToSearch)
        {
            AccountListGrid.Children.Clear();
            if (TextToSearch == "")
            {
                foreach (var acc in Utils.GetAccountsFromLogins(TaskItem.AccountNames))
                {
                    AccountListGrid.Children.Add(UI_Menager.GenerateAccountTaskCard(Utils.GetAccountBySteamAccount(acc), TaskItem));
                }
                return;
            }
            foreach (var account in Utils.GetAccountsFromLogins(TaskItem.AccountNames))
            {
                if (account.AccountInfo.Login.ToLower().Contains(TextToSearch.ToLower()) ||
                    account.AccountInfo.MaFile.Session.SteamID.ToString().Contains(TextToSearch) ||
                    Utils.GetAccountBySteamAccount(account).AccountCache.Nickname.ToLower().Contains(TextToSearch.ToLower()))
                {
                    AccountListGrid.Children.Add(UI_Menager.GenerateAccountTaskCard(Utils.GetAccountBySteamAccount(account), TaskItem));
                }
                
            }

           
        }
    }
}
