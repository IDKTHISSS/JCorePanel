using JCorePanelBase;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для PluginPropertyCard.xaml
    /// </summary>
    public partial class PluginPropertyCard : UserControl
    {
        JCPlugin Plugin;
        JCPluginProperty Property;
        public PluginPropertyCard(JCPlugin plugin, JCPluginProperty property)
        {
            InitializeComponent();

            Plugin = plugin;
            Property = property;

            PluginData.Content = property.Name;
        }

        private void image_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Utils.ShowPopupWindow(new EditPluginPropertyWindow(Plugin, Property));
        }
    }
}
