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
    /// Логика взаимодействия для PluginCard.xaml
    /// </summary>
    public partial class PluginCard : UserControl
    {
        JCPlugin CurrectPlugin;
        public PluginCard(JCPlugin plugin)
        {
            InitializeComponent();
            CurrectPlugin = plugin;
            TitleLabel.Content = plugin.FrendlyName;
            PlaceholderLabel.Content = plugin.IsEnabled ? "#ON" : "#OFF";
            if (plugin.Properties == null || plugin.Properties.Count == 0)
            {
                SettingsButton.Visibility = Visibility.Collapsed;
            }
            pluginEnabledToggleButton.IsChecked = plugin.IsEnabled;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation HoverRectangleAnimation = new DoubleAnimation(0.5, TimeSpan.FromSeconds(0.2));

            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, HoverRectangleAnimation);

            DoubleAnimation HoverAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.2));

            pluginEnabledToggleButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);
            InfoButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);
            SettingsButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);


            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 5);
            UI_Menager.ApplyBlurAnimation(PlaceholderLabel, TimeSpan.FromSeconds(0.2), 5);
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation HoverRectangleAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));

            HoverRectangle.BeginAnimation(UIElement.OpacityProperty, HoverRectangleAnimation);

            DoubleAnimation HoverAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.2));

            pluginEnabledToggleButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);
            InfoButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);
            SettingsButton.BeginAnimation(UIElement.OpacityProperty, HoverAnimation);


            UI_Menager.ApplyBlurAnimation(TitleLabel, TimeSpan.FromSeconds(0.2), 0);
            UI_Menager.ApplyBlurAnimation(PlaceholderLabel, TimeSpan.FromSeconds(0.2), 0);
        }

        private void pluginEnabledToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (PluginsManager.PluginIsEnabled(CurrectPlugin))
            {
                PluginsManager.DisablePlugin(CurrectPlugin);
            }
            else
            {
                PluginsManager.EnablePlugin(CurrectPlugin);
            }
            PlaceholderLabel.Content = PluginsManager.PluginIsEnabled(CurrectPlugin) ? "#ON" : "#OFF";
        }

        private void SettingsButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new PluginPropertySettings(CurrectPlugin));
        }

        private void InfoButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new PluginInfo(CurrectPlugin));
        }
    }
}
