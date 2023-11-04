using System;
using System.Windows.Input;

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
            msgBox.Text = Messege;
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
