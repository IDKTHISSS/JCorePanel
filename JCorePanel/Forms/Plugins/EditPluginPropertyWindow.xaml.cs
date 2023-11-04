using JCorePanelBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JCorePanel
{
    /// <summary>
    /// Логика взаимодействия для EditPluginPropertyWindow.xaml
    /// </summary>
    public partial class EditPluginPropertyWindow : BasePopupWindow
    {
        JCPlugin Plugin;
        JCPluginProperty PluginProperty;
        public EditPluginPropertyWindow(JCPlugin plugin, JCPluginProperty property)
        {
            InitializeComponent();
            Plugin = plugin;
            PluginProperty = property;
            PropertyNameBox.SetText(property.Name);
            foreach (var plug in ConfigMenager.PanelConfig.PluginsSettings.ToList())
            {

                if (plug.PluginName == Plugin.Name)
                {
                    foreach (var prop in plug.PluginSettings.ToList())
                    {
                        if (prop.Name == property.Name)
                            PropertyValueBox.SetText(prop.Value);
                    }
                }
            }

        }

        private void Button_ButtonClick(object sender, EventArgs e)
        {
            PluginProperty.Value = PropertyValueBox.TextInput;
            List<JCPluginProperty> newProperties = new List<JCPluginProperty>();
            foreach (var plugin in ConfigMenager.PanelConfig.PluginsSettings.ToList())
            {
                if (plugin.PluginName == Plugin.Name)
                {
                    foreach (var property in plugin.PluginSettings.ToList())
                    {
                        if (property.Name == PluginProperty.Name)
                        {
                            plugin.PluginSettings.Remove(property);
                            plugin.PluginSettings.Add(PluginProperty);
                            ConfigMenager.SaveSettings();
                        }
                    }
                    newProperties = plugin.PluginSettings;
                }
            }
            for (int i = 0; i < PluginsManager.PluginsList.Count; i++)
            {
                if (PluginsManager.PluginsList[i].Name == Plugin.Name)
                {
                    var temp = PluginsManager.PluginsList[i];
                    temp.Properties = newProperties;
                    PluginsManager.PluginsList[i] = temp;
                }
            }
            OnWindowClose();
        }
    }
}
