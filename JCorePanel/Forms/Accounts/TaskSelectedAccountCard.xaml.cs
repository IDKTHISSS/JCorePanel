using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для TaskSelectedAccount.xaml
    /// </summary>
    public partial class TaskSelectedAccountCard : UserControl
    {
        public AccountInstance accountInstance;
        Action<bool> OnSelect;
        bool IsSelected = false;
        public TaskSelectedAccountCard(AccountInstance account, Action<bool> onSelect)
        {
            InitializeComponent();
            accountInstance = account;
            OnSelect = onSelect;
            AvatarImage.ImageSource = Utils.CreateBitmapImageFromBytes(account.AccountCache.AvatarBytes);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
