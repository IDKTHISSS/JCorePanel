using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JCorePanelBase;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace JCorePanel
{
    public static class AccountMenager
    {
        public static List<AccountInstance> AccountsList = new List<AccountInstance>();
        public static void LoadAccounts()
        {
            Logger.Log("Loading all accounts");
            if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts")))
            {
                string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"));
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension == ".jcfile")
                    {
                        string json = File.ReadAllText(file);

                        try
                        {
                            JCSteamAccount steamAccount = JsonConvert.DeserializeObject<JCSteamAccount>(json);
                            AccountInstance account = new AccountInstance();
                            account.AccountInfo = steamAccount;
                            account.AccountCache = LoadCache(steamAccount);
                            AccountMenager.AccountsList.Add(account);
                            Logger.Log($"Account: {steamAccount.Login} was successful loaded.");

                        }
                        catch (JsonException)
                        {
                            Logger.Log(LogLevel.Error, $"Invalid account file. File name: {Path.GetFileName(file)}" );
                        }
                    }
                }
            }else
            {
                Logger.Log("Accounts folder not found. Creating Folder");
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"));
            }
        }
        public static SteamAccountCache LoadCache(JCSteamAccount account)
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AccountsCache/{account.Login}.jcCache")))
            {
                try
                {
                    SteamAccountCache steamAccount = JsonConvert.DeserializeObject<SteamAccountCache>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AccountsCache/{account.Login}.jcCache")));
                    Logger.Log($"Account: {account.Login} Cache was loaded");
                    return steamAccount;
                }
                catch (JsonException)
                {
                    Logger.Log(LogLevel.Error, $"Account: {account.Login} Can not load Cache");
                    return null;
                }
            }
            Logger.Log(LogLevel.Warning, $"Account: {account.Login} Cache not found");
            return null;
        }

        public static void SaveCache(JCSteamAccount Account, SteamAccountCache accountCache)
        {
            string json = JsonConvert.SerializeObject(accountCache);
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AccountsCache");
            string filePath = Path.Combine(directoryPath, $"{Account.Login}.jcCache");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.Create(filePath).Close();
            File.WriteAllText(filePath, json);
        }
        
        public static void AddAcount(JCSteamAccount Account)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"));
            }
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Accounts/{Account.Login}.jcfile"), JsonConvert.SerializeObject(Account).ToString());
            AccountInstance account = new AccountInstance();
            account.AccountInfo = Account;
            
            MainWindow mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
            account.AccountCard = new AccountCard(account);
            AccountsList.Add(account);
            mainWindow.AccountsListGrid.Children.Clear();
            foreach(var acc in AccountsList)
            {
                mainWindow.AccountsListGrid.Children.Add(acc.AccountCard);
            }

        }
    
        public static void SaveAccountData(JCSteamAccount Account)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"))) return;

            string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"));
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (extension == ".jcfile")
                {
                    string json = File.ReadAllText(file);

                    try
                    {
                        JCSteamAccount steamAccount = JsonConvert.DeserializeObject<JCSteamAccount>(json);

                        if (steamAccount.Login.ToLower() == Account.Login.ToLower())
                        {
                            File.WriteAllText(file, JsonConvert.SerializeObject(Account).ToString());
                        }
                    }
                    catch (JsonException)
                    {

                    }
                }
            }
            foreach(var account in AccountsList)
            {
                if(account.AccountInfo.Login.ToLower() ==  Account.Login.ToLower()) {
                    account.AccountInfo = Account;
                }
            }
        }
    }
}
