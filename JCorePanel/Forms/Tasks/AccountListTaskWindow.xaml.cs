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
        private List<TaskAccountCard> AccountCards = new List<TaskAccountCard>();
        public AccountListTaskWindow(JCTaskItem taskItem)
        {
            InitializeComponent();

            TaskItem = taskItem;

            foreach(var acc in Utils.GetAccountInstancesFromLogins(TaskItem.AccountNames))
            {
                TaskAccountCard NewAccountCard = new TaskAccountCard(acc, TaskItem);
                AccountCards.Add(NewAccountCard);
                AccountListGrid.Children.Add(NewAccountCard);
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
                    AccountListGrid.Children.Add(new TaskAccountCard(acc, TaskItem));
                }
            };
            Utils.ShowPopupWindow(addAccountToTaskWindow);
        }

        private void SearchAccountBox_TextChanged(string TextToSearch)
        {
            AccountListGrid.Children.Clear();
            if (TextToSearch == "")
            {
                foreach (var acc in AccountCards)
                {
                    AccountListGrid.Children.Add(acc);
                }
                return;
            }

            foreach (var account in AccountCards)
            {
                if (account.CurrectAccount.AccountInfo.Login.ToLower().Contains(TextToSearch.ToLower()))
                {
                    AccountListGrid.Children.Add(account);
                    continue;
                }
                if (account.CurrectAccount.AccountInfo.MaFile != null && account.CurrectAccount.AccountInfo.MaFile.Session.SteamID.ToString().Contains(TextToSearch))
                {
                    AccountListGrid.Children.Add(account);
                    continue;
                }
                if (account.CurrectAccount.AccountCache != null && account.CurrectAccount.AccountCache.Nickname.ToLower().Contains(TextToSearch.ToLower()))
                {
                    AccountListGrid.Children.Add(account);
                    continue;
                }
               
            }
        }
    }
}
