using JCorePanel.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для AddAccountToTaskWindow.xaml
    /// </summary>
    public partial class AddAccountToTaskWindow : BasePopupWindow
    {
        private JCTaskItem TaskItem;
        public List<AccountInstance> selectedAccs = new List<AccountInstance>();
        public Action<List<AccountInstance>> OnSelectedAccs;
        private List<TaskSelectedAccountCard> SelectedAccountCards = new List<TaskSelectedAccountCard>();
        public AddAccountToTaskWindow(JCTaskItem taskItem)
        {
            InitializeComponent();
            TaskItem = taskItem;

            List<AccountInstance> accounts = AccountMenager.AccountsList.ToList();
            if (TaskItem.AccountNames != null)
            {
                foreach (var acc in TaskItem.AccountNames)
                {
                    accounts.RemoveAll(account => account.AccountInfo.Login == acc);
                }
            }
            foreach (var account in accounts)
            {
                SelectedAccountCards.Add(new TaskSelectedAccountCard(account, (IsSelected) =>
                {
                    if (IsSelected)
                    {
                        selectedAccs.Add(account);
                    }
                    else
                    {
                        selectedAccs.Remove(account);
                    }

                }));
            }
            AddAcounts(accounts);

        }
        private void AddAcounts(List<AccountInstance> accounts)
        {
            AccountListGrid.Children.Clear();
            foreach (var account in accounts)
            {
                foreach(var card in SelectedAccountCards)
                {
                    if(card.accountInstance.AccountInfo.Login == account.AccountInfo.Login)
                    {
                        AccountListGrid.Children.Add(card);
                    }
                }
               
            }
        }
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCTaskItem newItem = TaskItem;
            if (newItem.AccountNames == null) newItem.AccountNames = new List<string>();
            foreach (var account in selectedAccs)
            {
                newItem.AccountNames.Add(account.AccountInfo.Login);
            }
            TaskManager.EditTask(TaskItem, newItem);
            OnSelectedAccs(selectedAccs);
            OnWindowClose();
        }

        private void SearchAccountBox_TextChanged(string TextToSearch)
        {
            if (TextToSearch == "")
            {
                List<AccountInstance> accounts2 = AccountMenager.AccountsList.ToList();
                foreach (var acc in TaskItem.AccountNames)
                {
                    accounts2.RemoveAll(account => account.AccountInfo.Login == acc);
                }
                AddAcounts(accounts2);
                return;
            }
            List<AccountInstance> accounts = AccountMenager.AccountsList.ToList();
            List<AccountInstance> SelectedAccounts = new List<AccountInstance>();
            foreach (var acc in TaskItem.AccountNames)
            {
                accounts.RemoveAll(account => account.AccountInfo.Login == acc);
            }

            foreach (var account in accounts)
            {
                if (account.AccountInfo.Login.ToLower().Contains(TextToSearch.ToLower()))
                {
                    SelectedAccounts.Add(account);
                    continue;
                }
                if (account.AccountInfo.MaFile != null && account.AccountInfo.MaFile.Session.SteamID.ToString().Contains(TextToSearch))
                {
                    SelectedAccounts.Add(account);
                    continue;
                }
                if (account.AccountCache != null && account.AccountCache.Nickname.ToLower().Contains(TextToSearch.ToLower()))
                {
                    SelectedAccounts.Add(account);
                    continue;
                }
            }
            AddAcounts(SelectedAccounts);
        }
    }
}
