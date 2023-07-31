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
    /// Логика взаимодействия для DialogConfirm.xaml
    /// </summary>
    public partial class DialogConfirm : BasePopupWindow
    {
        public Action<bool> OnConfirm;
        public DialogConfirm(string Messege)
        {
            InitializeComponent();
            msgBox.Content = Messege;
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            OnWindowClose();
            OnConfirm(true);
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnWindowClose();
            OnConfirm(false);
        }
    }
}
