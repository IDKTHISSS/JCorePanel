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
