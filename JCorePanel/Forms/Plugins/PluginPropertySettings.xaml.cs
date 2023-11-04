using System;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для PluginPropertySettings.xaml
    /// </summary>
    public partial class PluginPropertySettings : BasePopupWindow
    {
        public PluginPropertySettings(JCPlugin plugin)
        {
            InitializeComponent();

            foreach (var property in plugin.Properties)
            {
                PropreryListGrid.Children.Add(new PluginPropertyCard(plugin, property));
            }
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            OnWindowClose();
        }
    }
}
