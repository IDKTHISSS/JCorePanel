using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace JCorePanel
{
    public class GlobalMenager
    {

        public delegate void TestMessageDelegate(string message);


        public static void Setup()
        {
            JCorePanelBase.GlobalMenager.ShowDialog += (messege) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Utils.ShowPopupWindow(new Dialog(messege));
                });

            };
            JCorePanelBase.GlobalMenager.ShowInput += (messege, placeholder, callback) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DialogWithInput dialogWithInput = new DialogWithInput(messege, placeholder);
                    dialogWithInput.OnCloseDialog += callback;
                    Utils.ShowPopupWindow(dialogWithInput);
                });

            };
            JCorePanelBase.GlobalMenager.ShowConfirm += (messege, callback) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DialogConfirm dialogConfirm = new DialogConfirm(messege);
                    dialogConfirm.OnConfirm += callback;
                    Utils.ShowPopupWindow(dialogConfirm);
                });

            };
            JCorePanelBase.GlobalMenager.GetProperty += (property) =>
            {
                Assembly assembly = new StackTrace().GetFrame(1).GetMethod().Module.Assembly;
                Type[] types = assembly.GetTypes();
                string PluginName = null;
                foreach (Type type in types)
                {
                    // Проверяем, является ли тип статическим классом и имеет имя JCPluginConfig
                    if (type.IsClass && type.IsSealed && type.IsAbstract && type.Name == "JCPluginConfig")
                    {
                        // Получаем поле (переменную) по имени
                        FieldInfo field = type.GetField("PLUGIN_NAME", BindingFlags.Public | BindingFlags.Static);

                        if (field != null)
                        {
                            PluginName = field.GetValue(null) as string;
                        }
                    }
                }
                if (PluginName == null) return "";

                return Utils.GetPluginProperty(PluginName, property);
            };
            JCorePanelBase.GlobalMenager.GetSteamPath += () =>
            {
                return ConfigMenager.PanelConfig.SteamPath;
            };
        }
    }

}
