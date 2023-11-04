using System;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для Dialog.xaml
    /// </summary>
    public partial class Dialog : BasePopupWindow
    {
        public Dialog(string Messege)
        {
            InitializeComponent();

            msgBox.Text = Messege;
        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            OnWindowClose();
        }
    }
}
