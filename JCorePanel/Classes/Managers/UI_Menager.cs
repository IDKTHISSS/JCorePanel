using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WpfAnimatedGif;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JCorePanel.Classes.Managers;
using JCorePanelBase;

namespace JCorePanel
{
    public static class UI_Menager
    {
        public static Border GenerateAccountCard(AccountInstance account)
        {
            Border border = new Border();
            border.Style = (Style)Application.Current.MainWindow.FindResource("CardStyle");

            Grid grid = new Grid();
            grid.Margin = new Thickness(0, -2, -2, -2);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(75); // Update column1 width
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(1, GridUnitType.Star); // Set fixed width for column2
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Border ImageBorder = new Border();
            ImageBorder.CornerRadius = new CornerRadius(9);
            ImageBorder.Name = "Avatar";
            ImageBorder.Margin = new Thickness(5, 5, 2, 5);

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.Stretch = Stretch.Fill;
            imageBrush.ImageSource = new BitmapImage(new Uri(account.AccountCache == null ? "https://avatars.cloudflare.steamstatic.com/fef49e7fa7e1997310d705b2a6158ff8dc1cdfeb_medium.jpg" : account.AccountCache.AvatarURL));

            ImageBorder.Background = imageBrush;

            Grid.SetColumn(ImageBorder, 0);
            grid.Children.Add(ImageBorder);

            Grid innerGrid = new Grid();
            innerGrid.Name = "InfoGrid";
            innerGrid.HorizontalAlignment = HorizontalAlignment.Left; // Align the innerGrid to the right side

            Grid.SetColumn(innerGrid, 1);
            grid.Children.Add(innerGrid);

            Label titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            string labelText = account.AccountCache == null ? "Name" : account.AccountCache.Nickname;
            titleLabel.Content = labelText.Length > 10 ? labelText.Substring(0, 10) + "..." : labelText;
            titleLabel.Foreground = Brushes.White;
            titleLabel.Margin = new Thickness(0, 10, -10, -10);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(titleLabel);

            Label loginLabel = new Label();
            loginLabel.Name = "LoginLabel";
            loginLabel.Content = account.AccountInfo.Login.Length > 14 ? account.AccountInfo.Login.Substring(0, 14) + "..." : account.AccountInfo.Login;
            loginLabel.Margin = new Thickness(0, 24, -10, 20);
            loginLabel.Foreground = Brushes.Gray;
            loginLabel.RenderTransformOrigin = new Point(0.258, 0.225);
            loginLabel.FontSize = 9;
            loginLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(loginLabel);

            Label placeholderLabel = new Label();
            placeholderLabel.Name = "PlaceholderLabel";
            placeholderLabel.Content = "#Placeholder";
            placeholderLabel.Margin = new Thickness(0, 38, -10, 10);
            placeholderLabel.Foreground = (Brush)new BrushConverter().ConvertFrom("#8A41FB");
            placeholderLabel.RenderTransformOrigin = new Point(0.258, 0.225);
            placeholderLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(placeholderLabel);

            Rectangle hoverRectangle = new Rectangle();
            hoverRectangle.Name = "HoverRectangle";
            hoverRectangle.SetValue(Grid.ColumnSpanProperty, 2);
            hoverRectangle.RadiusX = 9;
            hoverRectangle.RadiusY = 9;
            hoverRectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            hoverRectangle.Opacity = 0;
            hoverRectangle.Height = 75;
            hoverRectangle.Margin = new Thickness(-2, 0, 0, 0);
            hoverRectangle.VerticalAlignment = VerticalAlignment.Center;
            hoverRectangle.Width = 163;
            hoverRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            grid.Children.Add(hoverRectangle);

            Image infoButtonImage = new Image();
            infoButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Info.png", UriKind.Relative));
            infoButtonImage.Margin = new Thickness(50, 10, 5, 51);
            infoButtonImage.Width = 12;
            infoButtonImage.Height = 12;
            infoButtonImage.Cursor = Cursors.Hand;
            infoButtonImage.Opacity = 0;
            infoButtonImage.MouseDown += (sender, e) => {
                ShowAccountInfo(account);
            };
            Grid.SetColumn(infoButtonImage, 1);
            grid.Children.Add(infoButtonImage);

            Image quickActionButtonImage = new Image();
            quickActionButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/3_Dots.png", UriKind.Relative));
            quickActionButtonImage.Margin = new Thickness(51, 58, 14, 10);
            quickActionButtonImage.Width = 15;
            quickActionButtonImage.Cursor = Cursors.Hand;
            quickActionButtonImage.Height = 12;
            quickActionButtonImage.Opacity = 0;
            ContextMenu contextMenu = new ContextMenu();
           
            quickActionButtonImage.MouseDown += (sender, e) =>
            {
                foreach (var Account in GlobalVars.AccountsList)
                {
                    Account.SetupAction();
                }
                contextMenu.Items.Clear();
                foreach (var Action in account.ActionList)
                {

                    CreateMenuItem(Action.SubMenuName == null ? null : Action.SubMenuName.Split('|'), Action.FriendlyName, contextMenu , account, Action.Name);

                }
                e.Handled = true; // Остановка события от всплывания выше
                contextMenu.PlacementTarget = quickActionButtonImage;
                contextMenu.IsOpen = true;
            };
            Grid.SetColumn(quickActionButtonImage, 1);
            grid.Children.Add(quickActionButtonImage);



            Image animatedImage = new Image();
            animatedImage.Width = 40;
            animatedImage.Height = 40;
            animatedImage.Source = Utils.ConvertBitmapToBitmapImage(Resource1.Loading);




            RotateTransform rotateTransform = new RotateTransform();
            animatedImage.HorizontalAlignment = HorizontalAlignment.Center;
            animatedImage.VerticalAlignment = VerticalAlignment.Center;
            animatedImage.RenderTransformOrigin = new Point(0.5, 0.5);
            animatedImage.Margin = new Thickness(-15, 0, 0, 0);
            animatedImage.Name = "WorkAnimation";
            // Установите точку трансформации в центр изображения
            animatedImage.RenderTransform = rotateTransform; // Примените точку трансформации к изображению
            animatedImage.Opacity = 0;


            // Создайте анимацию вращения
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = 0; // Начальный угол вращения (0 градусов)
            rotationAnimation.To = 360; // Конечный угол вращения (360 градусов)
            rotationAnimation.Duration = TimeSpan.FromSeconds(1); // Продолжительность анимации (2 секунды)
            rotationAnimation.RepeatBehavior = RepeatBehavior.Forever; // Повторять анимацию бесконечно

            // Запустите анимацию вращения
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            grid.Children.Add(animatedImage);


            // Example text
            TextBlock exampleText = new TextBlock();
            exampleText.Name = "WorkStatus";
            exampleText.Text = "Example";
            exampleText.HorizontalAlignment = HorizontalAlignment.Left;
            exampleText.VerticalAlignment = VerticalAlignment.Center;
            exampleText.Foreground = Brushes.White;
            exampleText.FontSize = 11;
            exampleText.Opacity = 0;
            exampleText.Margin = new Thickness(-15, 0, 0, 0);
            Grid.SetColumn(exampleText, 1);
            grid.Children.Add(exampleText);
            border.Child = grid;


            border.MouseEnter += (sender, e) =>
            {
                if (account.IsInWork) return;
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                quickActionButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                BlurEffect cardImageEffect = new BlurEffect();
                cardImageEffect.Radius = 5;
                ImageBorder.Effect = cardImageEffect;

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 5;
                titleLabel.Effect = titleLabelEffect;

                BlurEffect loginLabelEffect = new BlurEffect();
                loginLabelEffect.Radius = 5;
                loginLabel.Effect = loginLabelEffect;

                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 5;
                placeholderLabel.Effect = placeholderLabelEffect;
            };

            border.MouseLeave += (sender, e) =>
            {
                if (account.IsInWork) return;
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                quickActionButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                BlurEffect cardImageEffect = new BlurEffect();
                cardImageEffect.Radius = 0;
                ImageBorder.Effect = cardImageEffect;

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 0;
                titleLabel.Effect = titleLabelEffect;

                BlurEffect loginLabelEffect = new BlurEffect();
                loginLabelEffect.Radius = 0;
                loginLabel.Effect = loginLabelEffect;

                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 0;
                placeholderLabel.Effect = placeholderLabelEffect;
            };
            return border;
        }

        public static Border GeneratePluginCard(JCPlugin plugin)
        {
            Border border = new Border();
            border.Style = (Style)Application.Current.MainWindow.FindResource("CardStyle");
            border.Height = 65;

            Grid grid = new Grid();
            grid.Margin = new Thickness(0, -2, -2, -2);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(175); // Update column1 width
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(1, GridUnitType.Star); // Set fixed width for column2
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid innerGrid = new Grid();
            innerGrid.HorizontalAlignment = HorizontalAlignment.Left; // Align the innerGrid to the right side

            Grid.SetColumn(innerGrid, 0);
            grid.Children.Add(innerGrid);

            Label titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Content = plugin.FrendlyName;
            titleLabel.Width = 150;
            titleLabel.Height = 45;
            titleLabel.FontSize = 14;
            titleLabel.Foreground = Brushes.White;
            titleLabel.Margin = new Thickness(5, -10, -10, -10);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(titleLabel);

            Label placeholderLabel = new Label();
            placeholderLabel.Name = "PlaceholderLabel";
            placeholderLabel.Content = PluginsManager.PluginIsEnabled(plugin) ? "#On" : "#Off";
            placeholderLabel.FontSize = 10;
            placeholderLabel.Margin = new Thickness(5, 28, -10, 10);
            placeholderLabel.Foreground = (Brush)new BrushConverter().ConvertFrom("#8A41FB");
            placeholderLabel.RenderTransformOrigin = new Point(0.258, 0.225);
            placeholderLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(placeholderLabel);

            Rectangle hoverRectangle = new Rectangle();
            hoverRectangle.Name = "HoverRectangle";
            hoverRectangle.SetValue(Grid.ColumnSpanProperty, 2);
            hoverRectangle.RadiusX = 9;
            hoverRectangle.RadiusY = 9;
            hoverRectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            hoverRectangle.Opacity = 0;
            hoverRectangle.Height = 65;
            hoverRectangle.Margin = new Thickness(-2, 0, 0, 0);
            hoverRectangle.VerticalAlignment = VerticalAlignment.Center;
            hoverRectangle.Width = 163;
            hoverRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            grid.Children.Add(hoverRectangle);




            Image infoButtonImage = new Image();
            infoButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Info.png", UriKind.Relative));
            infoButtonImage.Margin = new Thickness(120, 24, 10, 23);
            infoButtonImage.Width = 13;
            infoButtonImage.Cursor = Cursors.Hand;
            infoButtonImage.Height = 13;
            infoButtonImage.MouseDown += (sender, e) => {
                UI_Menager.ShowPluginInfo(plugin);
            };
            infoButtonImage.Opacity = 0;
            Grid.SetColumn(infoButtonImage, 0);
            grid.Children.Add(infoButtonImage);


            Image SettingsButtonImage = new Image();
            SettingsButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Settings.png", UriKind.Relative));
            SettingsButtonImage.Margin = new Thickness(80, 24, 10, 23);
            SettingsButtonImage.Width = 17;
            SettingsButtonImage.Height = 14;
            SettingsButtonImage.Opacity = 0;
            SettingsButtonImage.Cursor = Cursors.Hand;
            Grid.SetColumn(SettingsButtonImage, 0);
            grid.Children.Add(SettingsButtonImage);

            ToggleButton pluginEnabledToggleButton = new ToggleButton();

            pluginEnabledToggleButton.Margin = new Thickness(10, 23, 121, 22);
            pluginEnabledToggleButton.Opacity = 0;
            pluginEnabledToggleButton.IsChecked = PluginsManager.PluginIsEnabled(plugin);
            pluginEnabledToggleButton.Cursor = Cursors.Hand;
            pluginEnabledToggleButton.Click += (sender, e) =>
            {
                
                if (PluginsManager.PluginIsEnabled(plugin))
                {
                    PluginsManager.DisablePlugin(plugin);
                }
                else
                {
                    PluginsManager.EnablePlugin(plugin);
                }
                placeholderLabel.Content = PluginsManager.PluginIsEnabled(plugin) ? "#On" : "#Off";
            };
            Grid.SetColumn(pluginEnabledToggleButton, 0);
            grid.Children.Add(pluginEnabledToggleButton);


            border.Child = grid;

            border.MouseEnter += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                SettingsButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                DoubleAnimation pluginEnabledButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                pluginEnabledToggleButton.BeginAnimation(UIElement.OpacityProperty, pluginEnabledButtonAnimation);


                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 5;
                titleLabel.Effect = titleLabelEffect;


                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 5;
                placeholderLabel.Effect = placeholderLabelEffect;
            };

            border.MouseLeave += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                SettingsButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                DoubleAnimation pluginEnabledButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                pluginEnabledToggleButton.BeginAnimation(UIElement.OpacityProperty, pluginEnabledButtonAnimation);

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 0;
                titleLabel.Effect = titleLabelEffect;

                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 0;
                placeholderLabel.Effect = placeholderLabelEffect;
            };
            return border;
        }
        public static Border GenerateTaskCard(JCTaskItem Task)
        {
            Border border = new Border();
            border.Style = (Style)Application.Current.MainWindow.FindResource("CardStyle");
            border.Height = 65;

            Grid grid = new Grid();
            grid.Margin = new Thickness(0, -2, -2, -2);

            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(175); // Update column1 width
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(1, GridUnitType.Star); // Set fixed width for column2
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);

            Grid innerGrid = new Grid();
            innerGrid.HorizontalAlignment = HorizontalAlignment.Left; // Align the innerGrid to the right side

            Grid.SetColumn(innerGrid, 0);
            grid.Children.Add(innerGrid);

            Label titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Content = Task.TaskName;
            titleLabel.Width = 150;
            titleLabel.Height = 45;
            titleLabel.FontSize = 14;
            titleLabel.Foreground = Brushes.White;
            titleLabel.Margin = new Thickness(5, -10, -10, -10);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(titleLabel);

            Label placeholderLabel = new Label();
            placeholderLabel.Name = "PlaceholderLabel";
            placeholderLabel.Content = "#" + Task.TaskStatus;
            placeholderLabel.FontSize = 10;
            placeholderLabel.Margin = new Thickness(5, 28, -10, 10);
            placeholderLabel.Foreground = (Brush)new BrushConverter().ConvertFrom("#8A41FB");
            placeholderLabel.RenderTransformOrigin = new Point(0.258, 0.225);
            placeholderLabel.HorizontalAlignment = HorizontalAlignment.Left;
            innerGrid.Children.Add(placeholderLabel);

            Rectangle hoverRectangle = new Rectangle();
            hoverRectangle.Name = "HoverRectangle";
            hoverRectangle.SetValue(Grid.ColumnSpanProperty, 2);
            hoverRectangle.RadiusX = 9;
            hoverRectangle.RadiusY = 9;
            hoverRectangle.HorizontalAlignment = HorizontalAlignment.Left; // Установите горизонтальное выравнивание слева
            hoverRectangle.Opacity = 0;
            hoverRectangle.Height = 65;
            hoverRectangle.Margin = new Thickness(-2, 0, 0, 0);
            hoverRectangle.VerticalAlignment = VerticalAlignment.Center;
            hoverRectangle.Width = 163;
            hoverRectangle.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 0x3C, 0xFF, 0x8D)); // Задайте цвет заполнения
            grid.Children.Add(hoverRectangle);

            Image deleteButtonImage = new Image();
            deleteButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Delete.png", UriKind.Relative));
            deleteButtonImage.Margin = new Thickness(120, 24, 10, 23);
            deleteButtonImage.Width = 15;
            deleteButtonImage.Cursor = Cursors.Hand;
            deleteButtonImage.Height = 13;
            deleteButtonImage.Opacity = 0;
            Grid.SetColumn(deleteButtonImage, 0);

            grid.Children.Add(deleteButtonImage);



            Image infoButtonImage = new Image();
            infoButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Info.png", UriKind.Relative));
            infoButtonImage.Margin = new Thickness(80, 24, 10, 23);
            infoButtonImage.Width = 13;
            infoButtonImage.Height = 13;
            infoButtonImage.Cursor = Cursors.Hand;
            infoButtonImage.Opacity = 0;
            Grid.SetColumn(infoButtonImage, 0);
            grid.Children.Add(infoButtonImage);


            Image SettingsButtonImage = new Image();
            SettingsButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Settings.png", UriKind.Relative));
            SettingsButtonImage.Margin = new Thickness(40, 24, 10, 23);
            SettingsButtonImage.Width = 17;
            SettingsButtonImage.Cursor = Cursors.Hand;
            SettingsButtonImage.Height = 14;
            SettingsButtonImage.Opacity = 0;
            Grid.SetColumn(SettingsButtonImage, 0);
            grid.Children.Add(SettingsButtonImage);

            Image TasksButtonImage = new Image();
            TasksButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Document.png", UriKind.Relative));
            TasksButtonImage.Margin = new Thickness(0, 24, 10, 23);
            TasksButtonImage.Width = 17;
            TasksButtonImage.Height = 14;
            TasksButtonImage.Cursor = Cursors.Hand;
            TasksButtonImage.Opacity = 0;
            Grid.SetColumn(TasksButtonImage, 0);
            grid.Children.Add(TasksButtonImage);


            Image AccountsButtonImage = new Image();
            AccountsButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/User.png", UriKind.Relative));
            AccountsButtonImage.Margin = new Thickness(-40, 24, 10, 23);
            AccountsButtonImage.Width = 17;
            AccountsButtonImage.Height = 14;
            AccountsButtonImage.Cursor = Cursors.Hand;
            AccountsButtonImage.Opacity = 0;
            Grid.SetColumn(AccountsButtonImage, 0);
            grid.Children.Add(AccountsButtonImage);

            Image StartButtonImage = new Image();
            StartButtonImage.Source = new BitmapImage(new Uri("/Images/Icons/Play.png", UriKind.Relative));
            StartButtonImage.Margin = new Thickness(-110, 0, 10, 0);
            StartButtonImage.Width = 34;
            StartButtonImage.Height = 28;
            StartButtonImage.Cursor = Cursors.Hand;
            StartButtonImage.Opacity = 0;
            StartButtonImage.MouseDown +=  (sender, e) =>
            {
                Thread thread = new Thread(() =>
                {
                    TaskManager.StartTaskByTaskItem(Task);
                });
                thread.Start();

                
            };
            Grid.SetColumn(StartButtonImage, 0);
            grid.Children.Add(StartButtonImage);



            border.Child = grid;

            border.MouseEnter += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation deleteButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                deleteButtonImage.BeginAnimation(UIElement.OpacityProperty, deleteButtonAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                SettingsButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                DoubleAnimation AccountsButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                AccountsButtonImage.BeginAnimation(UIElement.OpacityProperty, AccountsButtonAnimation);

                DoubleAnimation TasksButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                TasksButtonImage.BeginAnimation(UIElement.OpacityProperty, TasksButtonAnimation);

                DoubleAnimation pluginEnabledButtonAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));
                StartButtonImage.BeginAnimation(UIElement.OpacityProperty, pluginEnabledButtonAnimation);


                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 5;
                titleLabel.Effect = titleLabelEffect;


                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 5;
                placeholderLabel.Effect = placeholderLabelEffect;
            };

            border.MouseLeave += (sender, e) =>
            {
                DoubleAnimation hoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                hoverRectangle.BeginAnimation(UIElement.OpacityProperty, hoverRectangleAnimation);

                DoubleAnimation deleteButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                deleteButtonImage.BeginAnimation(UIElement.OpacityProperty, deleteButtonAnimation);

                DoubleAnimation infoButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                infoButtonImage.BeginAnimation(UIElement.OpacityProperty, infoButtonAnimation);

                DoubleAnimation quickActionButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                SettingsButtonImage.BeginAnimation(UIElement.OpacityProperty, quickActionButtonAnimation);

                DoubleAnimation AccountsButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                AccountsButtonImage.BeginAnimation(UIElement.OpacityProperty, AccountsButtonAnimation);

                DoubleAnimation TasksButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                TasksButtonImage.BeginAnimation(UIElement.OpacityProperty, TasksButtonAnimation);

                DoubleAnimation pluginEnabledButtonAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));
                StartButtonImage.BeginAnimation(UIElement.OpacityProperty, pluginEnabledButtonAnimation);

                BlurEffect titleLabelEffect = new BlurEffect();
                titleLabelEffect.Radius = 0;
                titleLabel.Effect = titleLabelEffect;

                BlurEffect placeholderLabelEffect = new BlurEffect();
                placeholderLabelEffect.Radius = 0;
                placeholderLabel.Effect = placeholderLabelEffect;
            };
            return border;
        }

        public static ContextMenu CreateMenuItem(string[] subItems, string name, ContextMenu parentMenu, AccountInstance Account, string ActionName)
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
    
        public static void ShowDialog(string Messege)
        {
            Dialog myPopupWindow = new Dialog(Messege);
            var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);
            myPopupWindow.OnWindowClose += () => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is Dialog window))
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

            if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is Dialog window))
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
        public static void ShowDialogInput(string Messege, string Placeholder, Action<string> EndResponse)
        {
            DialogWithInput myPopupWindow = new DialogWithInput(Messege, Placeholder);
            var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);
            
            myPopupWindow.Visibility = Visibility.Visible;

            if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is DialogWithInput window))
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
            myPopupWindow.OnCloseDialog += (string response) => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is DialogWithInput window))
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
            myPopupWindow.OnCloseDialog += EndResponse;
        }

        public static void ShowDialogConfirm(string Messege, Action<bool> EndResponse)
        {
            DialogConfirm myPopupWindow = new DialogConfirm(Messege);
            var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);

            myPopupWindow.Visibility = Visibility.Visible;

            if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is DialogConfirm window))
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
            myPopupWindow.OnConfirm += (bool response) => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is DialogConfirm window))
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
            myPopupWindow.OnConfirm += EndResponse;
        }
        public static void ShowPluginInfo(JCPlugin plugin)
        {
            PluginInfo myPopupWindow = new PluginInfo(plugin);
            var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(0, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);

            myPopupWindow.Visibility = Visibility.Visible;

            if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is PluginInfo window))
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
            myPopupWindow.OnWindowClose += () => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is PluginInfo window))
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
        }

        public static void ShowAccountInfo(AccountInstance Account)
        {
            AccountInfoWindow myPopupWindow = new AccountInfoWindow(Account);
            var grid = (Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid;
            grid.Children.Add(myPopupWindow);
            Grid.SetColumn(myPopupWindow, 1);
            myPopupWindow.VerticalAlignment = VerticalAlignment.Center;
            myPopupWindow.HorizontalAlignment = HorizontalAlignment.Center;
            myPopupWindow.Margin = new Thickness(75, 0, 0, 0);
            Grid.SetRowSpan(myPopupWindow, 2);

            myPopupWindow.Visibility = Visibility.Visible;

            if ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child is Grid grid222)
                foreach (var child in grid222.Children)
                {
                    if (!(child is AccountInfoWindow window))
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
            myPopupWindow.OnWindowClose += () => {
                myPopupWindow.Visibility = Visibility.Collapsed;
                foreach (var child in ((Application.Current.MainWindow as MainWindow).MainWindowXAML.Child as Grid).Children)
                {
                    if (!(child is AccountInfoWindow window))
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
        }

    }
}
