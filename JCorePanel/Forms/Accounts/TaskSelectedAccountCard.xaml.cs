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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskSelectedAccount.xaml
    /// </summary>
    public partial class TaskSelectedAccountCard : UserControl
    {
        AccountInstance accountInstance;
        Action<bool> OnSelect;
        bool IsSelected = false;
        public TaskSelectedAccountCard(AccountInstance account, Action<bool> onSelect)
        {
            InitializeComponent();
            accountInstance = account;
            OnSelect = onSelect;
            AvatarImage.ImageSource = new BitmapImage(new Uri(account.AccountCache == null ? "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg" : account.AccountCache.AvatarURL));
            TitleLabel.Content = account.AccountCache != null ? account.AccountCache.Nickname : "Name";
            LoginLabel.Content = account.AccountInfo.Login;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsSelected = !IsSelected;
            OnSelect(IsSelected);
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(IsSelected ? 0.5 : 0, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

            UI_Menager.ApplyBlurAnimation(Avatar, TimeSpan.FromSeconds(0.2), IsSelected ? 1 : 0);
            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), IsSelected ? 1 : 0);
            UI_Menager.ApplyBlurAnimation(LoginLabel, TimeSpan.FromSeconds(0.2), IsSelected ? 1 : 0);
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsSelected) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);



            UI_Menager.ApplyBlurAnimation(Avatar, TimeSpan.FromSeconds(0.2), 5);
            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 5);
            UI_Menager.ApplyBlurAnimation(LoginLabel, TimeSpan.FromSeconds(0.2), 5);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (IsSelected) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);



            UI_Menager.ApplyBlurAnimation(Avatar, TimeSpan.FromSeconds(0.2), 0);
            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 0);
            UI_Menager.ApplyBlurAnimation(LoginLabel, TimeSpan.FromSeconds(0.2), 0);
        }
    }
}
