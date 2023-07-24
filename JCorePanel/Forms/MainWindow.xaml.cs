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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool SearchIsFocused = false;
        public MainWindow()
        {
            InitializeComponent();
            Canvas.SetTop(PageRect, 0);
            SelectPage(0);
            foreach (var plugin in PluginsManager.GetAllPlugins())
            {
                PluginsListGrid.Children.Add(UI_Menager.GeneratePluginCard(plugin));
            }

            foreach (var task in TaskManager.GetAllTasks())
            {
                TasksListGrid.Children.Add(UI_Menager.GenerateTaskCard(task));
            }
            AccountMenager.LoadAccounts();
            LoadAccounts();

        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.GetPosition(this).Y < 30)
                {
                    try
                    {
                        DragMove();
                    }
                    catch (Exception ex)
                    {} 
                }
            }

        }
        private void SelectPage(int index)
        {
            MoveRectangle((index - JCorePanelPages.SelectedIndex)*50, 28 + (index * 50));
            JCorePanelPages.SelectedIndex = index;
            SearchAccountBox.Focusable = false;
            Keyboard.ClearFocus();
            SearchAccountBox.Focusable = true;
        }
        private void MoveRectangle(int posche,int new_y)
        {
            
            DoubleAnimation a = new DoubleAnimation();
            a.From = Canvas.GetTop(PageRect);
            a.To = posche;
            a.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            a.FillBehavior = FillBehavior.Stop;

            Storyboard.SetTargetProperty(a, new PropertyPath(Canvas.TopProperty));
            Storyboard.SetTarget(a, PageRect);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(a);

            
            var animationTaskCompletionSource = new TaskCompletionSource<object>();

            a.Completed += (sender, e) =>
            {
                PageRect.Margin = new Thickness(13, new_y, 0, 0);
            };
            storyboard.Begin();
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectPage(0);
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }

        private void labe2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectPage(1);
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }

        private void label_Copy1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectPage(2);
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }

        private void label_Copy2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectPage(3);
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }

        private void label_Copy2_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            SelectPage(4);
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void label_Copy_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            if (label.Content.Equals("Main") && JCorePanelPages.SelectedIndex == 0) return;
            if (label.Content.Equals("Accounts") && JCorePanelPages.SelectedIndex == 1) return;
            if (label.Content.Equals("Tasks") && JCorePanelPages.SelectedIndex == 2) return;
            if (label.Content.Equals("Plugins") && JCorePanelPages.SelectedIndex == 3) return;
            if (label.Content.Equals("Settings") && JCorePanelPages.SelectedIndex == 4) return;
            label.Foreground = (Brush)new BrushConverter().ConvertFrom("#8A41FB");
        }

        private void label_Copy_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Label).Foreground = new SolidColorBrush(Colors.White);
        }
        private void LoadAccounts()
        {
            foreach (var account in GlobalVars.AccountsList)
            {
                Border border = UI_Menager.GenerateAccountCard(account);
                account.AccountCard = border;
                AccountsListGrid.Children.Add(border);
            }
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
        }

        private void image1_MouseEnter(object sender, MouseEventArgs e)
        {
            Uri imageUri = new Uri("pack://application:,,,/JCorePanel;component/Images/Icons/PowerOffGreen.png");
            BitmapImage bitmap = new BitmapImage(imageUri);
            image1.Source = bitmap;
        }

        private void image1_MouseLeave(object sender, MouseEventArgs e)
        {
            Uri imageUri = new Uri("pack://application:,,,/JCorePanel;component/Images/Icons/PowerOff.png");
            BitmapImage bitmap = new BitmapImage(imageUri);
            image1.Source = bitmap;
        }
        
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == "Search")
            {
                SearchIsFocused = true;
                instance.Foreground = Brushes.White;
                instance.Text = "";
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
            {
                SearchIsFocused = false;
                instance.Foreground = Brushes.Gray;
                instance.Text = "Search";
                CancelSearchAccount();
            }
                
        }
        private void CancelSearchAccount()
        {
            AccountsListGrid.Children.Clear();
            foreach (var account in GlobalVars.AccountsList)
            {
                AccountsListGrid.Children.Add(account.AccountCard);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string TextToSearch = ((TextBox)sender).Text;
            if (TextToSearch == "")
            {
                CancelSearchAccount();
                return;
            }
            if (!SearchIsFocused) return;
            AccountsListGrid.Children.Clear();
            foreach (var account in GlobalVars.AccountsList)
            {
                if (account.AccountInfo.Login.ToLower().Contains(TextToSearch.ToLower()) ||
                    account.AccountInfo.MaFile.Session.SteamID.ToString().Contains(TextToSearch) ||
                    account.AccountCache.Nickname.ToLower().Contains(TextToSearch.ToLower()))
                {
                    AccountsListGrid.Children.Add(account.AccountCard);
                }
            }
        }

        private void Button_MouseDown(object sender, EventArgs e)
        {
            AddAccountWindow myPopupWindow = new AddAccountWindow();
            var grid = MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(75, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);
            myPopupWindow.OnWindowClose += () => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in (MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is AddAccountWindow window))
                    {
                        if (child is FrameworkElement cont)
                        {
                            cont.Effect = null;
                            cont.IsEnabled = true;
                        }
                    }
                }
                myPopupWindow = null;
            };
            myPopupWindow.Visibility = Visibility.Visible;

            if (MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is AddAccountWindow window))
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UI_Menager.ShowDialogConfirm("Its plugin mark as virus. Do you need enable this plugin?", (result) => { });

            
        }
    }
}
