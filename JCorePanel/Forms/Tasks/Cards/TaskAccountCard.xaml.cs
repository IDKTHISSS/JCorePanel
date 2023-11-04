using JCorePanel.Classes.Managers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace JCorePanel
{
    public partial class TaskAccountCard : UserControl
    {
        public AccountInstance CurrectAccount;
        JCTaskItem CurrectTastItem;
        public Action OnAccountRemoved;
        public TaskAccountCard(AccountInstance account, JCTaskItem taskItem)
        {
            InitializeComponent();
            CurrectAccount = account;
            CurrectTastItem = taskItem;
            AvatarImage.ImageSource = Utils.CreateBitmapImageFromBytes(account.AccountCache.AvatarBytes);
            TitleLabel.Content = account.AccountCache != null ? account.AccountCache.Nickname : "Name";
            LoginLabel.Content = account.AccountInfo.Login;

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var task in TaskManager.TaskList)
            {
                if (task.TaskItem.TaskName == CurrectTastItem.TaskName)
                {
                    OnAccountRemoved();
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
