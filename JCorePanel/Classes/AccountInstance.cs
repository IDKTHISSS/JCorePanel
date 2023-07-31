using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using SteamAuth;
using SteamKit2;
using SteamKit2.Authentication;
using SteamKit2.Internal;
using JCorePanelBase;
using static SteamKit2.GC.Underlords.Internal.CUserMessageVGUIMenu;
using static SteamKit2.Internal.CMsgRemoteClientBroadcastStatus;
using System.Windows.Controls;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using Image = System.Windows.Controls.Image;
using System.Windows.Media.Effects;
using Label = System.Windows.Controls.Label;
using static SteamKit2.GC.Dota.Internal.CMsgClientToGCIntegrityStatus;
using System.Windows.Media.Animation;

namespace JCorePanel
{
    public class AccountInstance : JCSteamAccountInstance
    {

        public SteamAccountCache AccountCache;
        public Border AccountCard;
        public List<JCAction> ActionList = new List<JCAction>();
        public AccountInstance()
        {
            StatusChangedHandler += StatusChange;
            IsInWorkChangedHandler += IsInWorkChanged;
            ErrorChangedHandler += ErrorChanged;
            WorkStatusChangedHandler += WorkStatusChanged;
            IsInWork = false;
            SetupAction();

        }
        public void StatusChange(string NewStatus)
        {
            SetPlaceholder(NewStatus);
        }
        private void WorkStatusChanged(string NewStatus)
        {
            if (NewStatus == null) return;
            if (AccountCard.Child is Grid grid)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is TextBlock txt && txt.Name.Equals("WorkStatus"))
                        {
                            txt.Text = NewStatus.Length > 17 ? NewStatus.Substring(0, 17) + "..." : NewStatus;
                            break;
                        }
                    }
                });
            }
        }
        private void ErrorChanged(bool NewStatus)
        {
            if (AccountCard.Child is Grid grid)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is System.Windows.Shapes.Rectangle rect && rect.Name.Equals("HoverRectangle"))
                        {
                            rect.Fill = NewStatus ? (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF0000") : (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#3CFF8D");
                            break;
                        }
                    }
                });
            }
        }
        private void IsInWorkChanged(bool newValue)
        {
            if (AccountCard.Child is Grid grid)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is Image img && img.Name.Equals("WorkAnimation"))
                        {
                            img.Opacity = newValue ? 1 : 0;
                        }
                        if (child is TextBlock txt && txt.Name.Equals("WorkStatus"))
                        {
                            txt.Opacity = newValue ? 1 : 0;
                        }
                        if (child is System.Windows.Shapes.Rectangle hover && hover.Name.Equals("HoverRectangle"))
                        {
                            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(newValue ? 0.5f : 0, TimeSpan.FromSeconds(0.2));
                            hover.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);
                        }
                        if (child is Border ImageBorder && ImageBorder.Name.Equals("Avatar"))
                        {
                            BlurEffect Effect = new BlurEffect();
                            Effect.Radius = newValue ? 5 : 0;
                            ImageBorder.Effect = Effect;
                        }
                        if (child is Grid _grid && _grid.Name.Equals("InfoGrid"))
                        {
                            foreach (var child2 in _grid.Children)
                            {
                                if (child2 is Label titleLabel && titleLabel.Name.Equals("TitleLabel"))
                                {
                                    BlurEffect Effect = new BlurEffect();
                                    Effect.Radius = newValue ? 5 : 0;
                                    titleLabel.Effect = Effect;
                                }
                                if (child2 is Label loginLabel && loginLabel.Name.Equals("LoginLabel"))
                                {
                                    BlurEffect Effect = new BlurEffect();
                                    Effect.Radius = newValue ? 5 : 0;
                                    loginLabel.Effect = Effect;
                                }
                                if (child2 is Label placeholderLabel && placeholderLabel.Name.Equals("PlaceholderLabel"))
                                {
                                    BlurEffect Effect = new BlurEffect();
                                    Effect.Radius = newValue ? 5 : 0;
                                    placeholderLabel.Effect = Effect;
                                }
                            }
                        }

                    }
                });
            }
        }
        protected void AddAction(JCAction Action)
        {
            ActionList.Add(Action);
        }
        public void SetPlaceholder(string NewPlaceholder)
        {
            if (AccountCard.Child is Grid grid)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is Grid _grid && _grid.Name.Equals("InfoGrid"))
                        {
                            foreach (var child2 in _grid.Children)
                            {
                                if (child2 is System.Windows.Controls.Label label && label.Name == "PlaceholderLabel")
                                {
                                    label.Content = NewPlaceholder;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                });
            }
        }
        public void UpdateAccountCardFromCache(SteamAccountCache AccountCache)
        {
            if (AccountCard.Child is Grid grid)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is Grid _grid && _grid.Name.Equals("InfoGrid"))
                        {
                            foreach (var child2 in _grid.Children)
                            {
                                if (child2 is System.Windows.Controls.Label label && label.Name == "TitleLabel")
                                {
                                    string labelText = AccountCache.Nickname == null ? "Name" : AccountCache.Nickname;
                                    label.Content = labelText.Length > 10 ? labelText.Substring(0, 10) + "..." : labelText;
                                    break;
                                }
                            }
                        }
                        if (child is Border Avatar && Avatar.Name == "Avatar")
                        {
                            ImageBrush AvatarImage = (ImageBrush)Avatar.Background;
                            AvatarImage.ImageSource = new BitmapImage(new Uri(AccountCache.AvatarURL == null ? "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg" : AccountCache.AvatarURL));
                        }
                    }
                });
            }
        }
        public void SetupAction()
        {
            ActionList.Clear();
            ActionList.AddRange(PluginsManager.GetActionsFromPlugins(AccountInfo));
            AddAction(new JCAction("UpdateInfo", "Update Info", UpdateInfo));
        }
        private async Task UpdateInfo(JCSteamAccountInstance CurrectAccount)
        {
            SetInWork(true);
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
