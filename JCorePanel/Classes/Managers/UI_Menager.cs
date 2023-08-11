using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WpfAnimatedGif;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JCorePanel.Classes.Managers;
using JCorePanelBase;
using System.Runtime.Remoting;

namespace JCorePanel
{
    public static class UI_Menager
    {
       
        public static void ShowDialog(string Messege)
        {
            Utils.ShowPopupWindow(new Dialog(Messege));
        }
        
        public static void ShowDialogInput(string Messege, string Placeholder, Action<string> EndResponse)
        {
            DialogWithInput myPopupWindow = new DialogWithInput(Messege, Placeholder);
            Utils.ShowPopupWindow(myPopupWindow);
            myPopupWindow.OnCloseDialog += EndResponse;
        }

        public static void ShowDialogConfirm(string Messege, Action<bool> EndResponse)
        {
            DialogConfirm myPopupWindow = new DialogConfirm(Messege);
            Utils.ShowPopupWindow(myPopupWindow);
            myPopupWindow.OnConfirm += EndResponse;
        }

        public static void ApplyBlurAnimation(UIElement targetElement, TimeSpan duration, double targetRadius)
        {
            if (targetElement.Effect is BlurEffect blurEffect)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    To = targetRadius,
                    Duration = new Duration(duration),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                blurEffect.BeginAnimation(BlurEffect.RadiusProperty, animation);
            }
            else
            {
                blurEffect = new BlurEffect { Radius = 0 };
                targetElement.Effect = blurEffect;

                DoubleAnimation animation = new DoubleAnimation
                {
                    To = targetRadius,
                    Duration = new Duration(duration),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                blurEffect.BeginAnimation(BlurEffect.RadiusProperty, animation);
            }
        }

    }
}
