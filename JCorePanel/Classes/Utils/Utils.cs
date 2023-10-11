using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Windows.Documents;
using System.Net;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows;

namespace JCorePanel
{
    public static class Utils
    {

        public static List<JCSteamAccountInstance> GetAccountsFromLogins(List<string> logins)
        {
            if (logins == null) return new List<JCSteamAccountInstance>();
            var accounts = new List<JCSteamAccountInstance>();
            foreach (var acc in AccountMenager.AccountsList)
            {
                foreach (var log in logins)
                {
                    if (log == acc.AccountInfo.Login)
                    {
                        accounts.Add(acc);
                    }
                }
            }
            return accounts;
        }
        public static List<AccountInstance> GetAccountInstancesFromLogins(List<string> logins)
        {
            if (logins == null) return new List<AccountInstance>();
            var accounts = new List<AccountInstance>();
            foreach (var acc in AccountMenager.AccountsList)
            {
                foreach (var log in logins)
                {
                    if (log == acc.AccountInfo.Login)
                    {
                        accounts.Add(acc);
                    }
                }
            }
            return accounts;
        }

        public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Сохраняем Bitmap в формате PNG в MemoryStream
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                // Создаем новый объект BitmapImage
                BitmapImage bitmapImage = new BitmapImage();

                // Загружаем BitmapImage из MemoryStream
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Фиксируем BitmapImage, чтобы он мог быть использован в других потоках

                return bitmapImage;
            }
        }
       
        public static JArray GetJsonArrayFromUrl(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    string json = webClient.DownloadString(url);
                    JArray jsonArray = JArray.Parse(json);
                    return jsonArray;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении JSON: " + ex.Message);
                return null;
            }
        }
       
        public static string GenerateRandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new StringBuilder(10);

            for (int i = 0; i < 10; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
        public static void ShowPopupWindow(BasePopupWindow popupWindow)
        {
            Application.Current.Dispatcher.Invoke(() => {
                var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
                grid.Children.Add(popupWindow);
                Grid.SetColumn(popupWindow, 1);
                popupWindow.VerticalAlignment = VerticalAlignment.Center;
                popupWindow.HorizontalAlignment = HorizontalAlignment.Center;
                popupWindow.Margin = new Thickness(0, 0, 0, 0);
                Grid.SetRowSpan(popupWindow, 2);

                popupWindow.Visibility = Visibility.Visible;

                if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                    foreach (var child in grid222.Children)
                    {
                        if (!(child is BasePopupWindow window) && child != popupWindow)
                        {
                            if (child is FrameworkElement cont)
                            {
                                BlurEffect blurEffect = new BlurEffect();
                                blurEffect.Radius = 5;
                                cont.Effect = blurEffect;
                                cont.IsEnabled = false;
                            }
                        }
                    }
                popupWindow.OnWindowClose += () => {
                    popupWindow.Visibility = Visibility.Collapsed;
                    foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                    {
                        if (!(child is BasePopupWindow window))
                        {
                            if (child is FrameworkElement cont)
                            {
                                cont.Effect = null;
                                cont.IsEnabled = true;
                            }
                        }
                    }
                    popupWindow = null;
                };

            });
            
        }
        public static AccountInstance GetAccountBySteamAccount(JCSteamAccountInstance account)
        {
            foreach(var acc in AccountMenager.AccountsList)
            {
                if(acc.AccountInfo.Login == account.AccountInfo.Login)
                {
                    return acc;
                }

            }
            return null;
        }
    
        public static string GetPluginProperty(string PluginName, string PropertyName)
        {
            foreach(var plugin in PluginsManager.PluginsList)
            {
                if(plugin.Name == PluginName)
                {
                    foreach(var property in plugin.Properties)
                    {
                        if(property.Name == PropertyName)
                        {
                            return property.Value;
                        }
                    }
                }
            }
            return "";
        }
        public static void AddOrUpdateProperties(List<JCPluginProperty> listToUpdate, List<JCPluginProperty> newList)
        {
            foreach (var newItem in newList)
            {
                // Проверяем, есть ли элемент с таким же Name в первом списке
                int existingIndex = listToUpdate.FindIndex(item => item.Name == newItem.Name);

                if (existingIndex != -1)
                {
                    // Если элемент с таким Name уже существует в первом списке, обновляем его Value
                    listToUpdate[existingIndex] = newItem;
                }
                else
                {
                    // Если элемента с таким Name нет в первом списке, добавляем его
                    listToUpdate.Add(newItem);
                }
            }
        }
    }
}
