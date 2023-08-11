using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SteamAuth;
using JCorePanelBase;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace JCorePanel
{
    public class AccountInstance : JCSteamAccountInstance
    {

       
        public AccountCard AccountCard;
        public List<JCAction> ActionList = new List<JCAction>();
        public AccountInstance()
        {
            StatusChangedHandler += StatusChange;
            IsInWorkChangedHandler += IsInWorkChanged;
            ErrorChangedHandler += ErrorChanged;
            WorkStatusChangedHandler += WorkStatusChanged;
            IsInWork = false;
            SetupAction();
            if(AccountCache == null)
            {
                AccountCache = new SteamAccountCache();
            }
        }
        public void SetupAction()
        {
            ActionList.Clear();
            ActionList.AddRange(PluginsManager.GetActionsFromPlugins(AccountInfo));
            AddAction(new JCAction("UpdateInfo", "Update Info", UpdateInfo));
        }
        public void StatusChange(string NewStatus)
        {
            SetPlaceholder(NewStatus);
        }
        private void WorkStatusChanged(string NewStatus)
        {
            if (NewStatus == null) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                AccountCard.WorkStatus.Text = NewStatus.Length > 17 ? NewStatus.Substring(0, 17) + "..." : NewStatus;
            });

        }
        private void ErrorChanged(bool NewStatus)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AccountCard.HoverRectangle.Fill = NewStatus ? (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF0000") : (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#3CFF8D");
            });
        }
        private void IsInWorkChanged(bool newValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AccountCard.WorkAnimation.Opacity = newValue ? 1 : 0;

                AccountCard.WorkStatus.Opacity = newValue ? 1 : 0;

                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(newValue ? 0.5f : 0, TimeSpan.FromSeconds(0.2));
                AccountCard.HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                UI_Menager.ApplyBlurAnimation(AccountCard.ImageBorder, TimeSpan.FromSeconds(0.2), newValue ? 5 : 0);
                UI_Menager.ApplyBlurAnimation(AccountCard.TitleLabel, TimeSpan.FromSeconds(0.2), newValue ? 5 : 0);
                UI_Menager.ApplyBlurAnimation(AccountCard.LoginLabel, TimeSpan.FromSeconds(0.2), newValue ? 5 : 0);
                UI_Menager.ApplyBlurAnimation(AccountCard.PlaceholderLabel, TimeSpan.FromSeconds(0.2), newValue ? 5 : 0);
                AccountCard.QuickActionButtonImage.IsEnabled = !newValue;
                AccountCard.InfoButtonImage.IsEnabled = !newValue;
            });
           

        }
        protected void AddAction(JCAction Action)
        {
            ActionList.Add(Action);
        }
        public void SetPlaceholder(string NewPlaceholder)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AccountCard.PlaceholderLabel.Content = NewPlaceholder;
            });
        }
        public void UpdateAccountCardFromCache(SteamAccountCache AccountCache)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                string labelText = AccountCache.Nickname == null ? "Name" : AccountCache.Nickname;
                AccountCard.TitleLabel.Content = labelText.Length > 10 ? labelText.Substring(0, 10) + "..." : labelText;

                ImageBrush AvatarImage = (ImageBrush)AccountCard.ImageBorder.Background;
                AvatarImage.ImageSource = new BitmapImage(new Uri(AccountCache.AvatarURL == null ? "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg" : AccountCache.AvatarURL));
            });
        }
        
        private async Task UpdateInfo(JCSteamAccountInstance CurrectAccount)
        {
            if (CurrectAccount.AccountInfo.MaFile == null || CurrectAccount.AccountInfo.MaFile == new SteamGuardAccount())
            {
                JCorePanelBase.GlobalMenager.ShowDialog("For updating info you need upload maFile.");
                return;
            }
            SetInWork(true);
            SetWorkStatus("Logging");
            await AccountInfo.CheckSession();
            SetWorkStatus("Getting Info");
            if(AccountCache == null)
            {
                AccountCache = new SteamAccountCache();
            }
            string text = await SteamWeb.GETRequest("https://steamcommunity.com/profiles/" + AccountInfo.MaFile.Session.SteamID.ToString(), AccountInfo.MaFile.Session.GetCookies());
            string nickname = @"<span\s+class=""actual_persona_name"">([^"">]+)</span>";
            AccountCache.Nickname = Regex.Match(text, nickname).Groups[1].Value;
            string avatarHalf = @"<div\s+class=""playerAvatarAutoSizeInner"">([\s\S]+)</div>";
            string avatarReg = @"<img\s+src=""([^"">]+)"">";
            if (Regex.Match(text, avatarHalf).Groups[1].Value.Contains("profile_avatar_frame"))
            {
                Match UserAvatar = Regex.Match(Regex.Match(text, avatarHalf).Groups[1].Value, avatarReg).NextMatch();
                UserAvatar.NextMatch();
                UserAvatar.NextMatch();
                AccountCache.AvatarURL = UserAvatar.Groups[1].Value;
            }
            else
            {
                AccountCache.AvatarURL = Regex.Match(Regex.Match(text, avatarHalf).Groups[1].Value, avatarReg).Groups[1].Value;
            }
            AccountMenager.SaveCache(AccountInfo, AccountCache);
            UpdateAccountCardFromCache(AccountCache);
            SetWorkStatus("Completed");
            SetInWork(false);
        }

        public void StartAction(string Name)
        {
            foreach (var action in ActionList)
            {
                if (action.Name == Name)
                {
                    Thread t = new Thread(async () =>
                    {
                        await action.ActionFunction(this);
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.IsBackground = true;
                    t.Start();
                    return;
                }
            }
        }
    }
}
