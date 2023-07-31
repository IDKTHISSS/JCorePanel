using JCorePanel.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            
            AddAcounts(accounts);

        }
        private void AddAcounts(List<AccountInstance> accounts)
        {
            AccountListGrid.Children.Clear();
            foreach (var account in accounts)
            {

                AccountListGrid.Children.Add(UI_Menager.GenerateSelectAccountTaskCard(account, (IsSelected) =>
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
        }
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            JCTaskItem newItem = TaskItem;
            if(newItem.AccountNames == null) newItem.AccountNames = new List<string>();
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
                if (account.AccountInfo.Login.ToLower().Contains(TextToSearch.ToLower()) ||
                    account.AccountInfo.MaFile.Session.SteamID.ToString().Contains(TextToSearch) ||
                    account.AccountCache.Nickname.ToLower().Contains(TextToSearch.ToLower()))
                {
                    SelectedAccounts.Add(account);
                }
            }
            AddAcounts(SelectedAccounts);
        }
    }
}
