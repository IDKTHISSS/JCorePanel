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

namespace JCorePanel
{
    public static class Utils
    {

        public static List<JCSteamAccountInstance> GetAccountsFromLogins(List<string> logins)
        {
            var accounts = new List<JCSteamAccountInstance>();
            foreach (var acc in GlobalVars.AccountsList)
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
        public static string CalculateSHA512Hash(string filePath)
        {
            using (var sha512 = SHA512.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha512.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
                }
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
        public static System.Windows.Media.Brush GetPluginColorStatus(string Status)
        {
            switch(Status)
            {

                case "Verified":
                    return (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#3CFF8D");
                case "Not Verified":
                    return (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#8A41FB");
                case "Unsafe":
                    return System.Windows.Media.Brushes.Yellow;
                case "Virus":
                    return System.Windows.Media.Brushes.Red;
            }
            return System.Windows.Media.Brushes.White;
        }
    }
}
