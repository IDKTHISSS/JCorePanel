using JCorePanelBase;
using System.Windows.Controls;
using System.Windows.Input;

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
