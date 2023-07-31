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
    /// Логика взаимодействия для PluginPropertySettings.xaml
    /// </summary>
    public partial class PluginPropertySettings : BasePopupWindow
    {
        public PluginPropertySettings(JCPlugin plugin)
        {
            InitializeComponent();

            foreach(var property in plugin.Properties)
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
