using System;
using System.Windows.Controls;

namespace JCorePanel
{
    public partial class BasePopupWindow : UserControl
    {
        public Action OnWindowClose;
        public BasePopupWindow() { }

        public BasePopupWindow(object Item) { }
    }
}
