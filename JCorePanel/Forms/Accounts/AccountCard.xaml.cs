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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для AccountCard.xaml
    /// </summary>
    public partial class AccountCard : UserControl
    {
        AccountInstance CurrectAccount;
        public AccountCard(AccountInstance accountInstance)
        {
            InitializeComponent();
            CurrectAccount = accountInstance;
            CurrectAccount.AccountCard = this;
            if (CurrectAccount.AccountCache != null)
            {
                CurrectAccount.UpdateAccountCardFromCache(CurrectAccount.AccountCache);
            }
            LoginLabel.Content = CurrectAccount.AccountInfo.Login;
            SetupWorkAnimation();
        }

        private void InfoButtonImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new AccountInfoWindow(CurrectAccount));
        }

        private void QuickActionButtonImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            CurrectAccount.SetupAction();
            contextMenu.Items.Clear();
            foreach (var Action in CurrectAccount.ActionList)
            {

                CreateMenuItem(Action.SubMenuName == null ? null : Action.SubMenuName.Split('|'), Action.FriendlyName, contextMenu, CurrectAccount, Action.Name);

            }
            e.Handled = true; // Остановка события от всплывания выше
            contextMenu.PlacementTarget = QuickActionButtonImage;
            contextMenu.IsOpen = true;
        }

        private void SetupWorkAnimation()
        {
            WorkAnimation.Source = Utils.ConvertBitmapToBitmapImage(Resource1.Loading);
            RotateTransform rotateTransform = new RotateTransform();
            WorkAnimation.HorizontalAlignment = HorizontalAlignment.Center;
            WorkAnimation.VerticalAlignment = VerticalAlignment.Center;
            WorkAnimation.RenderTransformOrigin = new Point(0.5, 0.5);
            WorkAnimation.Margin = new Thickness(-15, 0, 0, 0);
            WorkAnimation.Name = "WorkAnimation";
            WorkAnimation.RenderTransform = rotateTransform;
            WorkAnimation.Opacity = 0;


            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = 0;
            rotationAnimation.To = 360;
            rotationAnimation.Duration = TimeSpan.FromSeconds(1);
            rotationAnimation.RepeatBehavior = RepeatBehavior.Forever;

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (CurrectAccount.IsInWork) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

            DoubleAnimation infoButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            InfoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

            DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            QuickActionButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

            BlurEffect cardImageEffect = new BlurEffect();
            cardImageEffect.Radius = 5;
            ImageBorder.Effect = cardImageEffect;

            BlurEffect titleLabelEffect = new BlurEffect();
            titleLabelEffect.Radius = 5;
            TitleLabel.Effect = titleLabelEffect;

            BlurEffect loginLabelEffect = new BlurEffect();
            loginLabelEffect.Radius = 5;
            LoginLabel.Effect = loginLabelEffect;

            BlurEffect placeholderLabelEffect = new BlurEffect();
            placeholderLabelEffect.Radius = 5;
            PlaceholderLabel.Effect = placeholderLabelEffect;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrectAccount.IsInWork) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

            DoubleAnimation infoButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            InfoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

            DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            QuickActionButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

            BlurEffect cardImageEffect = new BlurEffect();
            cardImageEffect.Radius = 0;
            ImageBorder.Effect = cardImageEffect;

            BlurEffect titleLabelEffect = new BlurEffect();
            titleLabelEffect.Radius = 0;
            TitleLabel.Effect = titleLabelEffect;

            BlurEffect loginLabelEffect = new BlurEffect();
            loginLabelEffect.Radius = 0;
            LoginLabel.Effect = loginLabelEffect;

            BlurEffect placeholderLabelEffect = new BlurEffect();
            placeholderLabelEffect.Radius = 0;
            PlaceholderLabel.Effect = placeholderLabelEffect;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private ContextMenu CreateMenuItem(string[] subItems, string name, ContextMenu parentMenu, AccountInstance Account, string ActionName)
        {
            if (parentMenu == null)
            {
                parentMenu = new ContextMenu();
            }
            if (subItems == null)
            {
                MenuItem nameMenuItem2 = new MenuItem { Header = name };
                nameMenuItem2.Click += (sender, e) => {
                    Account.StartAction(ActionName);
                };
                parentMenu.Items.Add(nameMenuItem2);
                return parentMenu;
            }
            MenuItem currentMenuItem = null;
            List<MenuItem> existingSubItems = parentMenu.Items.Cast<MenuItem>().ToList();

            for (int i = 0; i < subItems.Length; i++)
            {
                MenuItem subMenuItem = existingSubItems.FirstOrDefault(item => item.Header.Equals(subItems[i]));

                if (subMenuItem == null)
                {
                    subMenuItem = new MenuItem { Header = subItems[i] };
                    if (currentMenuItem == null)
                    {
                        parentMenu.Items.Add(subMenuItem);
                    }
                    else
                    {
                        currentMenuItem.Items.Add(subMenuItem);
                    }
                    existingSubItems.Add(subMenuItem);
                }

                currentMenuItem = subMenuItem;
                existingSubItems = currentMenuItem.Items.Cast<MenuItem>().ToList();
            }

            MenuItem nameMenuItem = new MenuItem { Header = name };
            nameMenuItem.Click += (sender, e) => {
                Account.StartAction(ActionName);
            };
            currentMenuItem.Items.Add(nameMenuItem);

            return parentMenu;
        }

    }
}
