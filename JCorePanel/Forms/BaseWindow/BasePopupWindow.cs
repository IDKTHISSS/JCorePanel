using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
