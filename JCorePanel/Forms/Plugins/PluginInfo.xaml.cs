using System.Windows.Input;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для PluginInfo.xaml
    /// </summary>
    public partial class PluginInfo : BasePopupWindow
    {

        public JCPlugin CurrectPlugin;
        public PluginInfo(JCPlugin Plugin)
        {
            InitializeComponent();
            CurrectPlugin = Plugin;
            label_Copy.Content = Plugin.FrendlyName;
            label_Copy1.Content = Plugin.Name;
            label_Copy6.Content = Plugin.FrendlyVersion;
            label_Copy6.ToolTip = Plugin.Version;
            label_Copy5.Text = Plugin.Description;
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
        }

        private void label_Copy4_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
