using JCorePanel.Classes.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class TaskAccountCard : UserControl
    {
        public AccountInstance CurrectAccount;
        JCTaskItem CurrectTastItem;
        public TaskAccountCard(AccountInstance account, JCTaskItem taskItem)
        {
            InitializeComponent();
            CurrectAccount = account;
            CurrectTastItem = taskItem;
            AvatarImage.ImageSource = new BitmapImage(new Uri(account.AccountCache == null ? "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg" : account.AccountCache.AvatarURL));
            TitleLabel.Content = account.AccountCache != null ? account.AccountCache.Nickname : "Name";
            LoginLabel.Content = account.AccountInfo.Login;

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var task in TaskManager.TaskList)
            {
                if (task.TaskItem.TaskName == CurrectTastItem.TaskName)
                {
                    task.TaskItem.AccountNames.RemoveAll(item => item == CurrectAccount.AccountInfo.Login);
                    (this.Parent as UniformGrid).Children.Remove(this);
                    TaskManager.EditTask(CurrectTastItem, task.TaskItem);
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (CurrectAccount.IsInWork) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

            DoubleAnimation DeleteButtonImageAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
            DeleteButton.BeginAnimation(UIElement.OpacityProperty, DeleteButtonImageAnimation);

            UI_Menager.ApplyBlurAnimation(Avatar, TimeSpan.FromSeconds(0.2), 5);
            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 5);
            UI_Menager.ApplyBlurAnimation(LoginLabel, TimeSpan.FromSeconds(0.2), 5);
            

        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrectAccount.IsInWork) return;
            DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

            DoubleAnimation DeleteButtonImageAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
            DeleteButton.BeginAnimation(UIElement.OpacityProperty, DeleteButtonImageAnimation);

            UI_Menager.ApplyBlurAnimation(Avatar, TimeSpan.FromSeconds(0.2), 0);
            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 0);
            UI_Menager.ApplyBlurAnimation(LoginLabel, TimeSpan.FromSeconds(0.2), 0);
        }
    }
}
